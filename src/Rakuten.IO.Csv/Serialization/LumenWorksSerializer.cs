// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LumenWorksSerializer.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using FastMember;

    using LumenWorks.Framework.IO.Csv;

    /// <summary>
    /// Serializes delimited files using <see cref="LumenWorks"/>.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    public class LumenWorksSerializer<T> where T : new()
    {
        /// <summary>
        /// The character that specifies the beginning of a comment.
        /// </summary>
        private readonly char commentCharacter = '#';

        /// <summary>
        /// The default value to use for strings when the value is not supplied
        /// </summary>
        private readonly string defaultStringValue;

        /// <summary>
        /// The character to use to delimit fields when writing files
        /// </summary>
        private readonly char delimiterCharacter = ',';

        /// <summary>
        /// The escape character.
        /// </summary>
        private readonly char escapeCharacter = '\'';

        /// <summary>
        /// Determines the order in which fields are written to the delimited file
        /// </summary>
        private readonly FieldOrder fieldWriteOrder = FieldOrder.OrderByClass;

        /// <summary>
        /// The set of fields that should be read
        /// </summary>
        private readonly List<FieldMapping> fieldsToRead;

        /// <summary>
        /// The set of fields that should be written.
        /// </summary>
        private readonly List<FieldMapping> fieldsToWrite;

        /// <summary>
        /// Gets the header line for writing to a file
        /// </summary>
        private readonly string headerLine;

        /// <summary>
        /// The set of fields that should be read only if they are present
        /// </summary>
        private readonly List<FieldMapping> optionalFieldsToRead;

        /// <summary>
        /// The quote character to use when writing quoted strings
        /// </summary>
        private readonly char quoteCharacter = '"';

        /// <summary>
        /// The string type.
        /// </summary>
        private readonly Type stringType = typeof(string);

        /// <summary>
        /// The type accessor for the transcribed type
        /// </summary>
        private readonly TypeAccessor typeAccessor;

        /// <summary>
        /// The value trimming options.
        /// </summary>
        private readonly ValueTrimmingOptions valueTrimmingOptions = ValueTrimmingOptions.UnquotedOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="LumenWorksSerializer{T}" /> class.
        /// </summary>
        public LumenWorksSerializer()
        {
            Type lineType = typeof(T);

            // Get the default string value to use for this line type
            this.defaultStringValue = lineType.GetCustomAttribute<DefaultStringsToNullAttribute>() == null
                                          ? string.Empty
                                          : null;

            // Get the quote character to use when writing quoted strings
            var csvOptionsAttribute = lineType.GetCustomAttribute<FileAttribute>();
            if (csvOptionsAttribute != null)
            {
                this.quoteCharacter = csvOptionsAttribute.QuoteCharacter;
                this.delimiterCharacter = csvOptionsAttribute.DelimiterCharacter;
                this.escapeCharacter = csvOptionsAttribute.EscapeCharacter;
                this.commentCharacter = csvOptionsAttribute.CommentCharacter;
                this.valueTrimmingOptions = csvOptionsAttribute.ValueTrimmingOptions;
                this.fieldWriteOrder = csvOptionsAttribute.FieldWriteOrder;
            }

            // Create the property accessor
            this.typeAccessor = TypeAccessor.Create(lineType);

            // Create the field mappings
            var fieldMappings = new List<FieldMapping>(
                from property in lineType.GetProperties()
                let fieldDirectionAttribute = property.GetCustomAttribute<FieldAttribute>()
                where fieldDirectionAttribute == null || !fieldDirectionAttribute.Operations.HasFlag(FieldOperations.Ignore)
                select this.TranscriptionDetails(property));

            // Get the culture to use for conversions, if specified
            var fieldCultureAttribute = lineType.GetCustomAttribute<CultureAttribute>();

            // If it is specified, then loop through all the mappings, and set the culture
            // on all those that haven't already got their culture set. This allows the culture
            // to be set at the class level and overridden at the property level
            if (fieldCultureAttribute != null && fieldCultureAttribute.Culture != null)
            {
                foreach (FieldMapping mapping in fieldMappings
                    .Where(mapping => mapping.Culture == null))
                {
                    mapping.Culture = fieldCultureAttribute.Culture;
                }
            }

            // This is the list of fields to read from a file during the read process. Any other fields in the file will be ignored
            // It is also the list of fields that is used when validating field headers
            this.fieldsToRead = new List<FieldMapping>(
                fieldMappings
                    .Where(f => f.Operations.HasFlag(FieldOperations.Read) && !f.Optional));

            // This is the list of optional fields to read from a file during the read process
            // ##############################################################################
            // Note: Specifying Optional on a field has different behaviour depending on whether
            // you are reading by headers or by index
            // By Header:
            // If the header does not exist, then the field will be set to its default value,
            // as opposed to an exception being thrown.
            // By Index:
            // If no field exists at the specified index, then the field will be set to its
            // default value, as opposed to an exception being thrown. (However: Note that the number of fields 
            // that can be read by the LumenworksCsvReader is defined by the number of items in the 
            // first row in the file, even if that first row contains headers.)
            // ---
            // If you need to handle missing fields in a particular row (i.e. some rows contain
            // fewer fields than the first row), use the Lumenworks option MissingFieldAction.ReplaceByNull. 
            // Note that, since a CSV file is simply a list of items separated by commas, *missing* fields 
            // are always deemed to be missing from the *end* of the list of items.
            // ##############################################################################
            this.optionalFieldsToRead = new List<FieldMapping>(
                fieldMappings.Where(f => f.Operations.HasFlag(FieldOperations.Read) && f.Optional));

            // This is the list of fields to write to a file during the write process.
            this.fieldsToWrite = new List<FieldMapping>(
                fieldMappings.Where(f => f.Operations.HasFlag(FieldOperations.Write)));

            // Work out the header line for writing to a file. Do this now cos it's not gonna change
            IEnumerable<FieldMapping> fields;

            switch (this.fieldWriteOrder)
            {
                case FieldOrder.OrderByClass:
                    fields = this.fieldsToWrite;
                    break;
                case FieldOrder.OrderByProperty:
                    fields = this.fieldsToWrite.OrderBy(m => m.PropertyName);
                    break;
                case FieldOrder.OrderByFieldName:
                    fields = this.fieldsToWrite.OrderBy(m => m.FieldName);
                    break;
                case FieldOrder.OrderByIndex:
                    var fieldWithoutIndex = this.fieldsToWrite.FirstOrDefault(m => !m.HasIndex);
                    if (fieldWithoutIndex != null)
                    {
                        throw new IndexMissingException(fieldWithoutIndex.PropertyName);
                    }

                    fields = this.fieldsToWrite.OrderBy(m => m.FieldIndex);
                    break;
                default:
                    throw new InvalidOperationException(this.fieldWriteOrder + " is not a value field order.");
            }

            this.headerLine = string.Join(
                this.delimiterCharacter.ToString(CultureInfo.InvariantCulture), 
                fields.Select(f => f.FieldName));

            // Ensure that the client hasn't specified some stupid default value for a field. i.e. a string value for an int field.
            this.ValidateDefaults();
        }

        /// <summary>
        /// Returns a <see cref="CsvReader"/> that can be used to read the file
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="hasHeaders">
        /// The has headers.
        /// </param>
        /// <param name="supportsMultiline">
        /// The supports Multiline.
        /// </param>
        /// <returns>
        /// The <see cref="CsvReader"/>.
        /// </returns>
        public CsvReader GetReader(
            string filename, 
            bool hasHeaders = true, 
            bool supportsMultiline = true)
        {
            var reader = new StreamReader(filename, Encoding.Default);
            return new CsvReader(
                reader, 
                hasHeaders, 
                this.delimiterCharacter, 
                this.quoteCharacter, 
                this.escapeCharacter, 
                this.commentCharacter, 
                this.valueTrimmingOptions)
            {
                SupportsMultiline = supportsMultiline
            };
        }

        /// <summary>
        /// Reads a file using the header values specified on the line object
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="encoding">
        /// The encoding.
        /// </param>
        /// <param name="errorAction">
        /// The error action.
        /// </param>
        /// <param name="skipLines">
        /// The skip lines.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public List<T> ReadFileByHeaders(
            string filename, 
            Encoding encoding, 
            Action<ReadLineException> errorAction = null, 
            long skipLines = 0)
        {
            using (var streamReader = new StreamReader(filename, encoding))
            using (var reader = new CsvReader(streamReader, true, this.delimiterCharacter, this.quoteCharacter))
            {
                return this.ReadFileByHeaders(reader, errorAction, skipLines).ToList();
            }
        }

        /// <summary>
        /// Reads a file using the header values specified on the line object
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="errorAction">
        /// The error action.
        /// </param>
        /// <param name="skipLines">
        /// The skip lines.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public List<T> ReadFileByHeaders(
            string filename, 
            Action<ReadLineException> errorAction = null, 
            long skipLines = 0)
        {
            using (var streamReader = new StreamReader(filename))
            using (var reader = new CsvReader(streamReader, true, this.delimiterCharacter, this.quoteCharacter))
            {
                return this.ReadFileByHeaders(reader, errorAction, skipLines).ToList();
            }
        }

        /// <summary>
        /// Reads a file using the header values specified on the line object
        /// </summary>
        /// <param name="csvReader">
        /// The csv reader.
        /// </param>
        /// <param name="errorAction">
        /// The error Action.
        /// </param>
        /// <param name="skipLines">
        /// An optional number of lines to skip at the beginning of the file
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of csv line objects.
        /// </returns>
        public IEnumerable<T> ReadFileByHeaders(
            CsvReader csvReader, 
            Action<ReadLineException> errorAction = null, 
            long skipLines = 0)
        {
            this.ValidateHeaders(csvReader);

            int currentLine = 0;
            while (csvReader.ReadNextRecord())
            {
                T item;

                try
                {
                    item = this.ReadLineByHeaders(csvReader);
                }
                catch (ReadLineException ex)
                {
                    if (currentLine++ < skipLines)
                    {
                        continue;
                    }

                    if (errorAction == null)
                    {
                        throw;
                    }

                    errorAction(ex);
                    continue;
                }

                if (++currentLine > skipLines)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Reads a line using the header values specified on the line object
        /// </summary>
        /// <param name="csvReader">
        /// The csv reader.
        /// </param>
        /// <returns>
        /// A line object of type <see cref="T"/>.
        /// </returns>
        public T ReadLineByHeaders(CsvReader csvReader)
        {
            try
            {
                var lineObject = new T();

                foreach (FieldMapping mapping in this.fieldsToRead)
                {
                    this.SetProperty(lineObject, mapping, csvReader[mapping.FieldName]);
                }

                if (this.optionalFieldsToRead.Any())
                {
                    var headers = csvReader.GetFieldHeaders();

                    foreach (FieldMapping mapping in this.optionalFieldsToRead)
                    {
                        if (headers.Contains(mapping.FieldName) && csvReader[mapping.FieldName] != null)
                        {
                            this.SetProperty(lineObject, mapping, csvReader[mapping.FieldName]);
                        }
                        else
                        {
                            this.typeAccessor[lineObject, mapping.PropertyName] = mapping.DefaultValue;
                        }
                    }
                }

                return lineObject;
            }
            catch (Exception ex)
            {
                throw new ReadLineException(csvReader.CurrentRecordIndex + 1, ex);
            }
        }

        /// <summary>
        /// Reads a file using the index values specified on the line object
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="encoding">
        /// The encoding.
        /// </param>
        /// <param name="hasHeaders">
        /// The has Headers.
        /// </param>
        /// <param name="errorAction">
        /// The error action.
        /// </param>
        /// <param name="skipLines">
        /// The skip lines.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public List<T> ReadFileByIndex(
            string filename, 
            Encoding encoding, 
            bool hasHeaders = false, 
            Action<ReadLineException> errorAction = null, 
            long skipLines = 0)
        {
            using (var streamReader = new StreamReader(filename, encoding))
            using (var reader = new CsvReader(streamReader, hasHeaders, this.delimiterCharacter, this.quoteCharacter))
            {
                return this.ReadFileByIndex(reader, errorAction, skipLines).ToList();
            }
        }

        /// <summary>
        /// Reads a file using the index values specified on the line object
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="hasHeaders">
        /// The has Headers.
        /// </param>
        /// <param name="errorAction">
        /// The error action.
        /// </param>
        /// <param name="skipLines">
        /// The skip lines.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public List<T> ReadFileByIndex(
            string filename, 
            bool hasHeaders = false, 
            Action<ReadLineException> errorAction = null, 
            long skipLines = 0)
        {
            using (var streamReader = new StreamReader(filename))
            using (var reader = new CsvReader(streamReader, hasHeaders, this.delimiterCharacter, this.quoteCharacter))
            {
                return this.ReadFileByIndex(reader, errorAction, skipLines).ToList();
            }
        }

        /// <summary>
        /// Reads a file using the index values specified on the line object
        /// </summary>
        /// <param name="csvReader">
        /// The csv reader.
        /// </param>
        /// <param name="errorAction">
        /// The error Action.
        /// </param>
        /// <param name="skipLines">
        /// The skip Lines.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of csv line objects.
        /// </returns>
        public IEnumerable<T> ReadFileByIndex(
            CsvReader csvReader, 
            Action<ReadLineException> errorAction = null, 
            long skipLines = 0)
        {
            int currentLine = 0;
            while (csvReader.ReadNextRecord())
            {
                T item;

                try
                {
                    item = this.ReadLineByIndex(csvReader);
                }
                catch (ReadLineException ex)
                {
                    if (currentLine++ < skipLines)
                    {
                        continue;
                    }

                    if (errorAction == null)
                    {
                        throw;
                    }

                    errorAction(ex);
                    continue;
                }

                if (++currentLine > skipLines)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Reads a line using the index values specified on the line object
        /// </summary>
        /// <param name="csvReader">
        /// The csv reader.
        /// </param>
        /// <returns>
        /// A line object of type <see cref="T"/>.
        /// </returns>
        public T ReadLineByIndex(CsvReader csvReader)
        {
            try
            {
                var lineObject = new T();

                foreach (FieldMapping mapping in this.fieldsToRead.Where(m => m.HasIndex))
                {
                    this.SetProperty(lineObject, mapping, csvReader[mapping.FieldIndex]);
                }

                if (this.optionalFieldsToRead.Any())
                {
                    // Check that the field exists - FieldIndex is zero-based.
                    foreach (FieldMapping mapping in this.optionalFieldsToRead
                        .Where(m => m.HasIndex))
                    {
                        if (mapping.FieldIndex < csvReader.FieldCount && csvReader[mapping.FieldIndex] != null)
                        {
                            this.SetProperty(lineObject, mapping, csvReader[mapping.FieldIndex]);
                        }
                        else
                        {
                            this.typeAccessor[lineObject, mapping.PropertyName] = mapping.DefaultValue;
                        }
                    }
                }

                return lineObject;
            }
            catch (Exception ex)
            {
                throw new ReadLineException(csvReader.CurrentRecordIndex + 1, ex);
            }
        }

        /// <summary>
        /// Validates that headers are actually defined in this file, i.e. that it can be read.
        /// </summary>
        /// <param name="csvReader">
        /// The csv reader.
        /// </param>
        public void ValidateHeaders(CsvReader csvReader)
        {
            foreach (FieldMapping mapping in this.fieldsToRead
                .Where(mapping => !csvReader.GetFieldHeaders().Contains(mapping.FieldName)))
            {
                throw new HeaderMissingException(mapping.FieldName);
            }
        }

        /// <summary>
        /// Writes the headers to the supplied <see cref="StreamWriter"/>
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task WriteHeadersAsync(StreamWriter writer)
        {
            await writer.WriteLineAsync(this.GetCsvHeaders());
        }

        /// <summary>
        /// Writes the headers to the supplied <see cref="StreamWriter"/>
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public void WriteHeaders(StreamWriter writer)
        {
            writer.WriteLine(this.GetCsvHeaders());
        }

        /// <summary>
        /// Writes the supplied <see cref="T"/> to the supplied <see cref="StreamWriter"/>
        /// </summary>
        /// <param name="lineobject">
        /// The lineobject.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task WriteLineAsync(T lineobject, StreamWriter writer)
        {
            await writer.WriteLineAsync(this.GetCsvLine(lineobject));
        }

        /// <summary>
        /// Writes the supplied <see cref="T"/> to the supplied <see cref="StreamWriter"/>
        /// </summary>
        /// <param name="lineobject">
        /// The lineobject.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public void WriteLine(T lineobject, StreamWriter writer)
        {
            writer.WriteLine(this.GetCsvLine(lineobject));
        }

        /// <summary>
        /// Writes the <see cref="IEnumerable{T}"/> to the specified file. If the file already exists, it will be overwritten
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="lines">
        /// The lines.
        /// </param>
        /// <param name="includeHeader">
        /// The include header.
        /// </param>
        /// <param name="encoding">
        /// The encoding.
        /// </param>
        public void WriteFile(
            string filename, 
            IEnumerable<T> lines, 
            bool includeHeader, 
            Encoding encoding = null)
        {
            IEnumerable<string> header =
                includeHeader
                    ? this.GetCsvHeaders().Yield()
                    : Enumerable.Empty<string>();

            if (encoding == null)
            {
                File.WriteAllLines(
                    filename, 
                    header.Concat(lines.Select(this.GetCsvLine)));
            }
            else
            {
                File.WriteAllLines(
                    filename, 
                    header.Concat(lines.Select(this.GetCsvLine)), 
                    encoding);
            }
        }

        /// <summary>
        /// Writes the <see cref="IEnumerable{T}"/> to the specified <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="lines">
        /// The lines.
        /// </param>
        /// <param name="includeHeader">
        /// The include header.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public void WriteFile(string filename, IEnumerable<T> lines, bool includeHeader, StreamWriter writer)
        {
            IEnumerable<string> header =
                includeHeader
                    ? this.GetCsvHeaders().Yield()
                    : Enumerable.Empty<string>();

            foreach (string line in header.Concat(lines.Select(this.GetCsvLine)))
            {
                writer.WriteLine(line);
            }
        }

        /// <summary>
        /// Gets the formatted csv line for the supplied <see cref="T"/>
        /// </summary>
        /// <param name="lineobject">
        /// The lineobject.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetCsvLine(T lineobject)
        {
            IEnumerable<FieldMapping> fields;

            switch (this.fieldWriteOrder)
            {
                case FieldOrder.OrderByClass:
                    fields = this.fieldsToWrite;
                    break;
                case FieldOrder.OrderByProperty:
                    fields = this.fieldsToWrite.OrderBy(m => m.PropertyName);
                    break;
                case FieldOrder.OrderByFieldName:
                    fields = this.fieldsToWrite.OrderBy(m => m.FieldName);
                    break;
                case FieldOrder.OrderByIndex:
                    var fieldWithoutIndex = this.fieldsToWrite.FirstOrDefault(m => !m.HasIndex);
                    if (fieldWithoutIndex != null)
                    {
                        throw new IndexMissingException(fieldWithoutIndex.PropertyName);
                    }

                    fields = this.fieldsToWrite.OrderBy(m => m.FieldIndex);
                    break;
                default:
                    throw new InvalidOperationException(this.fieldWriteOrder + " is not a value field order.");
            }

            return string.Join(
                this.delimiterCharacter.ToString(CultureInfo.InvariantCulture), 
                fields.Select(m => this.GetWriteValue(lineobject, m)));
        }

        /// <summary>
        /// Gets the headers for the current file
        /// </summary>
        /// <returns>
        /// The formatted header line
        /// </returns>
        public string GetCsvHeaders()
        {
            return this.headerLine;
        }

        /// <summary>
        /// Gets the string to write for the supplied <see cref="FieldMapping"/>
        /// </summary>
        /// <param name="lineObject">
        /// The line Object.
        /// </param>
        /// <param name="mapping">
        /// The mapping.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetWriteValue(T lineObject, FieldMapping mapping)
        {
            object fieldValue = this.typeAccessor[lineObject, mapping.PropertyName];

            string retValue =
                fieldValue == null ? string.Empty :
                    mapping.HasEnumConverter ? mapping.EnumConverter.GetStringValueByEnum((Enum)fieldValue) :
                        mapping.IsEnumType ? fieldValue.AsType(mapping.MappingType, mapping.Culture).ToString() :
                            fieldValue.AsType(typeof(string), mapping.Culture).ToString();

            return this.GetQuotedField(retValue);
        }

        /// <summary>
        /// Returns a quoted version of the field if necessary, or the original field if not
        /// </summary>
        /// <param name="field">
        /// The field.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetQuotedField(string field)
        {
            return field.IndexOfAny(new[] { this.delimiterCharacter, '\r', '\n' }) != -1
                       ? string.Concat(this.quoteCharacter, field, this.quoteCharacter)
                       : field;
        }

        /// <summary>
        /// Validate the default values that have been specified for this object
        /// </summary>
        private void ValidateDefaults()
        {
            var tempObj = new T();

            foreach (FieldMapping mapping in this.fieldsToRead.Concat(this.optionalFieldsToRead))
            {
                try
                {
                    this.typeAccessor[tempObj, mapping.PropertyName] = mapping.DefaultValue;
                }
                catch (Exception ex)
                {
                    throw new InvalidDefaultException(mapping.DefaultValue, mapping.PropertyName, mapping.PropertyType, ex);
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="FieldMapping"/> object for the supplied <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The <see cref="FieldMapping"/>.
        /// </returns>
        private FieldMapping TranscriptionDetails(PropertyInfo property)
        {
            var fieldTitleAttribute = property.GetCustomAttribute<HeaderAttribute>();
            var fieldIndexAttribute = property.GetCustomAttribute<IndexAttribute>();
            var fieldDefaultAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
            var fieldEnumConverterAttribute = property.GetCustomAttribute<EnumConverterAttribute>();
            var fieldAttributesAttribute = property.GetCustomAttribute<FieldAttribute>();
            var fieldOptionalAttribute = property.GetCustomAttribute<OptionalAttribute>();
            var fieldCultureAttribute = property.GetCustomAttribute<CultureAttribute>();

            var mapping = new FieldMapping
            {
                PropertyType = property.PropertyType, 
                MappingType = property.PropertyType.GetTypeOrNullableUnderlyingType(), 
                Culture = fieldCultureAttribute.IfNotNull(a => a.Culture), 
                PropertyName = property.Name, 
                FieldName = fieldTitleAttribute == null ? property.Name : fieldTitleAttribute.Value,
                Operations = fieldAttributesAttribute == null ? FieldOperations.ReadWrite : fieldAttributesAttribute.Operations, 
                Optional = fieldOptionalAttribute != null
            };

            if (fieldIndexAttribute != null)
            {
                mapping.HasIndex = true;
                mapping.FieldIndex = fieldIndexAttribute.Index;
            }

            // If the property type is an Enum, store the type of the enum 
            // and set the mapping type to the underlying type of the enum (e.g. int)
            // Also check to see if an EnumConverter has been specified - if so add it to the mapping.
            if (mapping.MappingType.IsEnum)
            {
                mapping.IsEnumType = true;
                mapping.EnumType = mapping.MappingType;
                mapping.MappingType = Enum.GetUnderlyingType(mapping.MappingType);
                mapping.EnumConverter = fieldEnumConverterAttribute;
                mapping.HasEnumConverter = fieldEnumConverterAttribute != null;
            }

            // If the property type is string, set the default value to the default string value.
            // If there's no default value attribute specified, then set it to the default value of the type
            // else if a default value is specified and it's null, then set it to null,
            // else if the property is an enum, then set it to the default value converted to an enum
            // else try to set it to the default value converted to the type of the property
            if (mapping.PropertyType == this.stringType)
            {
                mapping.IsStringType = true;
                mapping.DefaultValue = fieldDefaultAttribute == null
                                           ? this.defaultStringValue
                                           : fieldDefaultAttribute.Value;
            }
            else
            {
                try
                {
                    mapping.DefaultValue =
                        fieldDefaultAttribute == null ? property.PropertyType.DefaultValue() :
                            fieldDefaultAttribute.Value == null ? null :
                                mapping.IsEnumType ? Enum.ToObject(mapping.EnumType, fieldDefaultAttribute.Value) :
                                    fieldDefaultAttribute.Value.AsType(mapping.MappingType, mapping.Culture);
                }
                catch (Exception ex)
                {
                    if (!(ex is InvalidCastException || ex is FormatException || ex is OverflowException))
                    {
                        throw;
                    }

                    // if defaultValueAttribute were null, then the default would have been set to
                    // the default of the property type. If this hasn't worked, then something else is wrong!
                    Contract.Assume(fieldDefaultAttribute != null, "Unexpected null value of 'fieldDefaultAttribute'");

                    throw new InvalidDefaultException(fieldDefaultAttribute.Value, mapping.PropertyName, mapping.PropertyType, ex);
                }
            }

            return mapping;
        }

        /// <summary>
        /// Sets a property on the <paramref name="lineObject"/>
        /// </summary>
        /// <param name="lineObject">
        /// The line object.
        /// </param>
        /// <param name="mapping">
        /// The field mapping for this property
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        private void SetProperty(T lineObject, FieldMapping mapping, object value)
        {
            try
            {
                var stringValue = value as string;

                // If there's no value, set the default value
                // If the property is a string, just set it to the value
                // If we have an enum converter, convert the string to an enum and set it
                // If it's an enum type (without a converter), convert the value to the enum type and set it
                // otherwise, just convert the string to the type of the property and set it
                this.typeAccessor[lineObject, mapping.PropertyName] =
                    stringValue == string.Empty ? mapping.DefaultValue :
                        mapping.IsStringType ? value :
                            mapping.HasEnumConverter ? mapping.EnumConverter.GetEnumValueByString(stringValue) :
                                mapping.IsEnumType ? value.AsEnum(mapping.EnumType, mapping.MappingType) :
                                    value.AsType(mapping.MappingType, mapping.Culture);
            }
            catch (FormatException e)
            {
                throw new FieldFormatException(value, mapping.PropertyType, mapping.FieldName, mapping.FieldIndex, mapping.PropertyName, e);
            }
        }
    }
}