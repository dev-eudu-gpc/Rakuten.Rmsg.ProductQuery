//------------------------------------------------------------------------------
// <copyright file="TraceWriter.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides trace writing facilities.
    /// </summary>
    public static class TraceWriter
    {
        /// <summary>
        /// Writes a line to the trace writer file.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void Write(string message)
        {
            using (var writer = new StreamWriter(@"c:\temp\rmsgprqint.txt", true))
            {
                writer.WriteLine(string.Format("{0} - {1}", DateTime.UtcNow, message));
                writer.Flush();
            }
        }
    }
}
