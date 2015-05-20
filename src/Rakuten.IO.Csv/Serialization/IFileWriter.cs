// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileWriter.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited.Serialization
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an instance that exposes functionality to write delimited files.
    /// </summary>
    /// <typeparam name="T">The type of object to be written to a file.</typeparam>
    public interface IFileWriter<in T>
    {
        /// <summary>
        /// Writes the collection of <typeparamref name="T"/> to the specified file using the given 
        /// <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="filename">The name of the file to which the items should be written.</param>
        /// <param name="lines">The collection of items to be written.</param>
        /// <param name="includeHeader">A value indicating whether headers should be written to the file.</param>
        /// <param name="writer">The <see cref="StreamWriter"/> instance using which the file should be written.</param>
        void WriteFile(string filename, IEnumerable<T> lines, bool includeHeader, StreamWriter writer);

        /// <summary>
        /// Writes the collection of <typeparamref name="T"/> to the specified file in the given <see cref="Encoding"/>.
        /// </summary>
        /// <param name="filename">The name of the file to which the items should be written.</param>
        /// <param name="lines">The collection of items to be written.</param>
        /// <param name="includeHeader">A value indicating whether headers should be written to the file.</param>
        /// <param name="encoding">The character set in which the file should be written.</param>
        void WriteFile(string filename, IEnumerable<T> lines, bool includeHeader, Encoding encoding = null);

        /// <summary>
        /// Writes the headers defined in <typeparamref name="T"/> to the <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="writer">The instance to which the headers should be written.</param>
        void WriteHeaders(StreamWriter writer);

        /// <summary>
        /// Asynchronously writes the headers defined in <typeparamref name="T"/> to the <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="writer">The instance to which the headers should be written.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        Task WriteHeadersAsync(StreamWriter writer);

        /// <summary>
        /// Writes the given instance of <typeparamref name="T"/> to the <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="lineobject">The instance to write.</param>
        /// <param name="writer">The instance to which the headers should be written.</param>
        void WriteLine(T lineobject, StreamWriter writer);

        /// <summary>
        /// Asynchronously writes the given instance of <typeparamref name="T"/> to the <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="lineobject">The instance to write.</param>
        /// <param name="writer">The instance to which the headers should be written.</param>
        Task WriteLineAsync(T lineobject, StreamWriter writer);
    }
}