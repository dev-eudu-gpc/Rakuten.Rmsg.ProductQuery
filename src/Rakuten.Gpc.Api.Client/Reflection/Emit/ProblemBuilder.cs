//---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProblemBuilder.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Web;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    using Rakuten.Gpc;

    /// <summary>
    /// Provides functionality to create dynamic types of <see cref="HttpException"/>.
    /// </summary>
    public class ProblemBuilder
    {
        /// <summary>
        /// The unique defining name given to the dynamic assembly.
        /// </summary>
        private const string AssemblyName = "Rakuten.Gpc.Models.Problems";

        /// <summary>
        /// The constructor used to instantiate a new object.
        /// </summary>
        private static readonly ConstructorInfo ObjectConstructor = typeof(object).GetConstructor(Type.EmptyTypes);

        /// <summary>
        /// The instance which will be used to generate a <see cref="AssemblyBuilder"/> instance.
        /// </summary>
        private readonly AssemblyBuilderFactory assemblyFactory;

        /// <summary>
        /// The instance which facilitates adding constructors to <see cref="TypeBuilder"/>s.
        /// </summary>
        private readonly ConstructorBuilderFactory constructorFactory;

        /// <summary>
        /// The instance which facilitates adding fields to <see cref="TypeBuilder"/>s.
        /// </summary>
        private readonly FieldBuilderFactory fieldFactory;

        /// <summary>
        /// The instance which facilitates creating instances of the 
        /// <see cref="JsonPropertyAttribute"/> via a <see cref="CustomAttributeBuilder"/>.
        /// </summary>
        private readonly JsonPropertyAttributeFactory jsonAttributeFactory;

        /// <summary>
        /// The instance which facilitates adding methods to <see cref="TypeBuilder"/>s.
        /// </summary>
        private readonly MethodBuilderFactory methodFactory;

        /// <summary>
        /// The instance which allows dynamic module creation.
        /// </summary>
        private readonly ModuleBuilderFactory moduleFactory;

        /// <summary>
        /// The instance which facilitates adding properties to <see cref="TypeBuilder"/>s.
        /// </summary>
        private readonly PropertyBuilderFactory propertyFactory;

        /// <summary>
        /// The instance which allows type creation.
        /// </summary>
        private readonly TypeBuilderFactory typeFactory;

        /// <summary>
        /// The instance which facilitates creating instances of the <see cref="XmlElementAttribute"/>
        /// via a <see cref="CustomAttributeBuilder"/>.
        /// </summary>
        private readonly XmlElementAttributeFactory xmlElementAttributeFactory;

        /// <summary>
        /// The instance which facilitates creating instances of the <see cref="XmlTypeAttribute"/>
        /// via a <see cref="CustomAttributeBuilder"/>.
        /// </summary>
        private readonly XmlTypeAttributeFactory xmlTypeAttributeFactory;

        /// <summary>
        /// The dynamically created assembly to hold the problem type definitions.
        /// </summary>
        private AssemblyBuilder assembly;

        /// <summary>
        /// The dynamically created module to hold the problem type definitions.
        /// </summary>
        private ModuleBuilder module;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemBuilder"/> class.
        /// </summary>
        /// <param name="assemblyFactory">The <see cref="AssemblyBuilder"/> provider.</param>
        /// <param name="moduleFactory">The <see cref="ModuleBuilderFactory"/> provider.</param>
        /// <param name="typeFactory">The <see cref="TypeBuilderFactory"/> provider.</param>
        /// <param name="constructorFactory">The <see cref="ConstructorBuilderFactory"/> provider.</param>
        /// <param name="fieldFactory">The <see cref="FieldBuilderFactory"/> provider.</param>
        /// <param name="propertyFactory">The <see cref="PropertyBuilderFactory"/> provider.</param>
        /// <param name="methodFactory">The <see cref="MethodBuilderFactory"/> provider.</param>
        /// <param name="xmlTypeAttributeFactory">The <see cref="XmlTypeAttributeFactory"/> provider.</param>
        /// <param name="xmlElementAttributeFactory">The <see cref="XmlElementAttributeFactory"/> provider.</param>
        /// <param name="jsonAttributeFactory">The <see cref="JsonPropertyAttributeFactory"/> provider.</param>
        public ProblemBuilder(
            AssemblyBuilderFactory assemblyFactory,
            ModuleBuilderFactory moduleFactory,
            TypeBuilderFactory typeFactory,
            ConstructorBuilderFactory constructorFactory,
            FieldBuilderFactory fieldFactory,
            PropertyBuilderFactory propertyFactory,
            MethodBuilderFactory methodFactory,
            XmlTypeAttributeFactory xmlTypeAttributeFactory,
            XmlElementAttributeFactory xmlElementAttributeFactory,
            JsonPropertyAttributeFactory jsonAttributeFactory)
        {
            if (assemblyFactory == null)
            {
                throw new ArgumentNullException("assemblyFactory");
            }

            if (moduleFactory == null)
            {
                throw new ArgumentNullException("moduleFactory");
            }

            if (typeFactory == null)
            {
                throw new ArgumentNullException("typeFactory");
            }

            if (constructorFactory == null)
            {
                throw new ArgumentNullException("constructorFactory");
            }

            if (fieldFactory == null)
            {
                throw new ArgumentNullException("fieldFactory");
            }

            if (propertyFactory == null)
            {
                throw new ArgumentNullException("propertyFactory");
            }

            if (methodFactory == null)
            {
                throw new ArgumentNullException("methodFactory");
            }

            if (xmlTypeAttributeFactory == null)
            {
                throw new ArgumentNullException("xmlTypeAttributeFactory");
            }

            if (xmlElementAttributeFactory == null)
            {
                throw new ArgumentNullException("xmlElementAttributeFactory");
            }

            if (jsonAttributeFactory == null)
            {
                throw new ArgumentNullException("jsonAttributeFactory");
            }

            this.assemblyFactory = assemblyFactory;
            this.constructorFactory = constructorFactory;
            this.fieldFactory = fieldFactory;
            this.jsonAttributeFactory = jsonAttributeFactory;
            this.methodFactory = methodFactory;
            this.moduleFactory = moduleFactory;
            this.propertyFactory = propertyFactory;
            this.typeFactory = typeFactory;
            this.xmlElementAttributeFactory = xmlElementAttributeFactory;
            this.xmlTypeAttributeFactory = xmlTypeAttributeFactory;
        }

        /// <summary>
        /// Creates a dynamic <see cref="Type"/> based on the <see cref="IApiException"/> instance specified.
        /// </summary>
        /// <param name="exception">
        /// The <see cref="IApiException"/> based on which the dynamic type should be created.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/> dynamically created.
        /// </returns>
        public Type DefineProblem(IApiException exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            return this.DefineProblem(exception.GetType());
        }

        /// <summary>
        /// Creates a dynamic <see cref="Type"/> based on the <see cref="HttpException"/> instance specified.
        /// </summary>
        /// <param name="type">
        /// The <see cref="HttpException"/> based on which the dynamic type should be created.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/> dynamically created.
        /// </returns>
        public Type DefineProblem(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            // Have we defined a dynamic assembly or not.
            if (this.assembly == null)
            {
                this.assembly = this.assemblyFactory.Create(AssemblyName);
                this.module = this.moduleFactory.Create(this.assembly);
            }

            string typeName = type.Name + "Problem";

            TypeBuilder builder = this.typeFactory.Create(this.module, typeName);
            ConstructorBuilder constructor = this.constructorFactory.Create(builder);

            this.EmitConstructorIL(constructor);

            // Define the root node name for XML serialization.
            this.AddXmlType(builder, "problem");

            // Add the standard properties; we do not need to copy these.
            this.AddProperty(builder, typeof(string), "Type", "type");
            this.AddProperty(builder, typeof(string), "Title", "title");
            this.AddProperty(builder, typeof(string), "Detail", "detail");

            if (typeof(AggregateException).IsAssignableFrom(type))
            {
                this.AddProperty(builder, typeof(object), "InnerProblems", "innerProblems");
            }

            // Copy the properties including attributes.
            // Find the properties of this exception excluding inherited properties.
            var properties = type.GetProperties(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo property in properties)
            {
                this.AddProperty(builder, property.PropertyType, property.Name, property);
            }

            return builder.CreateType();
        }

        /// <summary>
        /// Adds an XML type to the <see cref="TypeBuilder"/> with the specified name.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="TypeBuilder"/> instance to which an XML type name should be added.
        /// </param>
        /// <param name="typeName">The name of the root node when serializing in XML.</param>
        private void AddXmlType(TypeBuilder builder, string typeName)
        {
            Contract.Assume(this.xmlTypeAttributeFactory != null);

            if (!string.IsNullOrWhiteSpace(typeName))
            {
                var xmlTypeAttribute = this.xmlTypeAttributeFactory.Create(typeName);

                builder.SetCustomAttribute(xmlTypeAttribute);
            }
        }

        /// <summary>
        /// Adds a property to the <see cref="TypeBuilder"/> with the specified type and name.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="TypeBuilder"/> instance to which a property should be added.
        /// </param>
        /// <param name="type">
        /// The <see cref="Type"/> of the property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <param name="serializedName">
        /// The value to be given to the serialized representation of the property.
        /// </param>
        private void AddProperty(TypeBuilder builder, Type type, string name, string serializedName = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            PropertyBuilder property = this.DefineProperty(builder, type, name);

            if (!string.IsNullOrWhiteSpace(serializedName))
            {
                var xmlAttribute = this.xmlElementAttributeFactory.Create(serializedName);
                var jsonAttribute = this.jsonAttributeFactory.Create(serializedName);

                property.SetCustomAttribute(xmlAttribute);
                property.SetCustomAttribute(jsonAttribute);
            }
        }

        /// <summary>
        /// Adds a property to the <see cref="TypeBuilder"/> with the specified type and name copying any attributes 
        /// from the passed <see cref="PropertyInfo"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="TypeBuilder"/> instance to which a property should be added.</param>
        /// <param name="type">The <see cref="Type"/> of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="info">The <see cref="PropertyInfo"/> instance from which attributes should be cloned.</param>
        private void AddProperty(TypeBuilder builder, Type type, string name, PropertyInfo info)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            PropertyBuilder property = this.DefineProperty(builder, type, name);

            // Get the list of attributes applied to the existing property.
            IList<CustomAttributeData> attributes = CustomAttributeData.GetCustomAttributes(info);

            foreach (CustomAttributeData attribute in attributes)
            {
                var propertyArguments = new List<PropertyInfo>();
                var propertyArgumentValues = new List<object>();
                var fieldArguments = new List<FieldInfo>();
                var fieldArgumentValues = new List<object>();

                if (attribute.NamedArguments != null)
                {
                    foreach (var namedArg in attribute.NamedArguments)
                    {
                        var fi = namedArg.MemberInfo as FieldInfo;
                        var pi = namedArg.MemberInfo as PropertyInfo;

                        if (fi != null)
                        {
                            fieldArguments.Add(fi);
                            fieldArgumentValues.Add(namedArg.TypedValue.Value);
                        }
                        else if (pi != null)
                        {
                            propertyArguments.Add(pi);
                            propertyArgumentValues.Add(namedArg.TypedValue.Value);
                        }
                    }
                }

                var customAttribute = new CustomAttributeBuilder(
                  attribute.Constructor,
                  attribute.ConstructorArguments.Select(a => a.Value).ToArray(),
                  propertyArguments.ToArray(),
                  propertyArgumentValues.ToArray(),
                  fieldArguments.ToArray(),
                  fieldArgumentValues.ToArray());

                property.SetCustomAttribute(customAttribute);
            }
        }

        /// <summary>
        /// Creates a new property on the <see cref="TypeBuilder"/> with the specified type and name.
        /// </summary>
        /// <param name="builder">The <see cref="TypeBuilder"/> instance to which a property should be added.</param>
        /// <param name="type">The <see cref="Type"/> of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>The <see cref="PropertyBuilder"/> instance.</returns>
        private PropertyBuilder DefineProperty(TypeBuilder builder, Type type, string name)
        {
            string fieldName = char.ToLowerInvariant(name[0]) + name.Substring(1);
            string propertyName = char.ToUpperInvariant(name[0]) + name.Substring(1);

            FieldBuilder field = this.fieldFactory.Create(builder, type, fieldName);
            PropertyBuilder property = this.propertyFactory.Create(builder, type, propertyName);

            this.DefinePropertyGetMethod(builder, property, field, type, propertyName);
            this.DefinePropertySetMethod(builder, property, field, type, propertyName);

            return property;
        }

        /// <summary>
        /// Adds the necessary 'get' method for the property using the specified backing field.
        /// </summary>
        /// <param name="builder">The <see cref="TypeBuilder"/> instance to which the method should be added.</param>
        /// <param name="property">The property for which this method will be added.</param>
        /// <param name="field">The backing field for the property whose value will be retrieved via the method.</param>
        /// <param name="type">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        private void DefinePropertyGetMethod(
            TypeBuilder builder, 
            PropertyBuilder property, 
            FieldBuilder field, 
            Type type, 
            string name)
        {
            string methodName = "get_" + name;

            MethodBuilder method = this.methodFactory.Create(builder, type, methodName);

            ILGenerator generator = method.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, field);
            generator.Emit(OpCodes.Ret);

            property.SetGetMethod(method);
        }

        /// <summary>
        /// Adds the necessary 'set' method for the property using the specified backing field.
        /// </summary>
        /// <param name="builder">The <see cref="TypeBuilder"/> instance to which the method should be added.</param>
        /// <param name="property">The property for which this method will be added.</param>
        /// <param name="field">The backing field for the property whose value will be assigned via the method.</param>
        /// <param name="type">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        private void DefinePropertySetMethod(
            TypeBuilder builder, 
            PropertyBuilder property, 
            FieldBuilder field, 
            Type type, 
            string name)
        {
            string methodName = "set_" + name;

            MethodBuilder method = this.methodFactory.Create(builder, methodName, type);

            ILGenerator generator = method.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stfld, field);
            generator.Emit(OpCodes.Ret);

            property.SetSetMethod(method);
        }

        /// <summary>
        /// Adds the IL code for the specified <see cref="ConstructorBuilder"/> to the stack.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ConstructorBuilder"/> whose constructor will be output to the stack.
        /// </param>
        private void EmitConstructorIL(ConstructorBuilder builder)
        {
            ILGenerator il = builder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, ObjectConstructor);
            il.Emit(OpCodes.Ret);
        }
    }
}