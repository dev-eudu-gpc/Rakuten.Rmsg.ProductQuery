// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileReader.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Rakuten.IO.Delimited.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using LumenWorks.Framework.IO.Csv;

    /// <summary>
    /// Defines an instance that exposes functionality to read delimited files.
    /// </summary>
    /// <typeparam name="T">The type of object expected in the delimited file.</typeparam>
    public interface IFileReader<T>
    {
        /// <summary>
        /// Reads a collection of <typeparamref name="T"/> from the specified file using the column headers within the 
        /// file.
        /// </summary>
        /// <param name="filename">The path to the file including the name of the file and its extension.</param>
        /// <param name="encoding">The expected character encoding of the file.</param>
        /// <param name="errorAction">A delegate that should be invoked when an error is encountered.</param>
        /// <param name="skipLines">A number of lines to skip when reading the file.</param>
        /// <returns>A collection of <typeparamref name="T"/> read from the file.</returns>
        List<T> ReadFileByHeaders(
            string filename,
            Encoding encoding,
            Action<ReadLineException> errorAction = null,
            long skipLines = 0);

        /// <summary>
        /// Reads a collection of <typeparamref name="T"/> from the specified file using the column headers within the 
        /// file.
        /// </summary>
        /// <param name="filename">The path to the file including the name of the file and its extension.</param>
        /// <param name="errorAction">A delegate that should be invoked when an error is encountered.</param>
        /// <param name="skipLines">A number of lines to skip when reading the file.</param>
        /// <returns>A collection of <typeparamref name="T"/> read from the file.</returns>
        List<T> ReadFileByHeaders(
            string filename,
            Action<ReadLineException> errorAction = null,
            long skipLines = 0);

        /// <summary>
        /// Reads a collection of <typeparamref name="T"/> from a file using the specified <see cref="CsvReader"/> 
        /// using the column headers within the file.
        /// </summary>
        /// <param name="reader">A <see cref="CsvReader"/> instance to the file.</param>
        /// <param name="errorAction">A delegate that should be invoked when an error is encountered.</param>
        /// <param name="skipLines">A number of lines to skip when reading the file.</param>
        /// <returns>A collection of <typeparamref name="T"/> read from the file.</returns>
        IEnumerable<T> ReadFileByHeaders(
            CsvReader reader,
            Action<ReadLineException> errorAction = null,
            long skipLines = 0);

        /// <summary>
        /// Reads an instance of <typeparamref name="T"/> from a file using the specified <see cref="CsvReader"/> using 
        /// the column headers within the file.
        /// </summary>
        /// <param name="reader">A <see cref="CsvReader"/> instance to the file.</param>
        /// <returns>A collection of <typeparamref name="T"/> read from the file.</returns>
        T ReadLineByHeaders(CsvReader reader);

        /// <summary>
        /// Reads a collection of <typeparamref name="T"/> from the specified file using the order in which the columns 
        /// have been defined.
        /// </summary>
        /// <param name="filename">The path to the file including the name of the file and its extension.</param>
        /// <param name="encoding">The expected character encoding of the file.</param>
        /// <param name="hasHeaders">A value indicating whether the file has column headers.</param>
        /// <param name="errorAction">A delegate that should be invoked when an error is encountered.</param>
        /// <param name="skipLines">A number of lines to skip when reading the file.</param>
        /// <returns>A collection of <typeparamref name="T"/> read from the file.</returns>
        List<T> ReadFileByIndex(
            string filename,
            Encoding encoding,
            bool hasHeaders = false,
            Action<ReadLineException> errorAction = null,
            long skipLines = 0);

        /// <summary>
        /// Reads a collection of <typeparamref name="T"/> from the specified file using the order in which the columns 
        /// have been defined.
        /// </summary>
        /// <param name="filename">The path to the file including the name of the file and its extension.</param>
        /// <param name="hasHeaders">A value indicating whether the file has column headers.</param>
        /// <param name="errorAction">A delegate that should be invoked when an error is encountered.</param>
        /// <param name="skipLines">A number of lines to skip when reading the file.</param>
        /// <returns>A collection of <typeparamref name="T"/> read from the file.</returns>
        List<T> ReadFileByIndex(
            string filename,
            bool hasHeaders = false,
            Action<ReadLineException> errorAction = null,
            long skipLines = 0);

        /// <summary>
        /// Reads a collection of <typeparamref name="T"/> from the specified file using the order in which the columns 
        /// have been defined.
        /// </summary>
        /// <param name="reader">A <see cref="CsvReader"/> instance to the file.</param>
        /// <param name="errorAction">A delegate that should be invoked when an error is encountered.</param>
        /// <param name="skipLines">A number of lines to skip when reading the file.</param>
        /// <returns>A collection of <typeparamref name="T"/> read from the file.</returns>
        IEnumerable<T> ReadFileByIndex(
            CsvReader reader,
            Action<ReadLineException> errorAction = null,
            long skipLines = 0);

        /// <summary>
        /// Reads an instance of <typeparamref name="T"/> from a file using the specified <see cref="CsvReader"/> using 
        /// the order in which the columns have been defined.
        /// </summary>
        /// <param name="reader">A <see cref="CsvReader"/> instance to the file.</param>
        /// <returns>A collection of <typeparamref name="T"/> read from the file.</returns>
        T ReadLineByIndex(CsvReader reader);
    }
}