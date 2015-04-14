//------------------------------------------------------------------------------
// <copyright file="ResourceXmlMediaTypeFormatter.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Net.Http.Formatting
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Http.Formatting;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>A <see cref="MediaTypeFormatter"/> for handling XML.</summary>
    public class ResourceXmlMediaTypeFormatter : XmlMediaTypeFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceXmlMediaTypeFormatter"/> class.
        /// </summary>
        public ResourceXmlMediaTypeFormatter()
        {
            this.UseXmlSerializer = true;
            this.WriterSettings.OmitXmlDeclaration = false;
        }

        /// <summary>Called during deserialization to get the XML serializer.</summary>
        /// <param name="type">The type of object that will be serialized or deserialized.</param>
        /// <returns>The <see cref="XmlSerializer"/> used to serialize the object.</returns>
        public override XmlSerializer CreateXmlSerializer(Type type)
        {
            return this.UseXmlSerializer ? new NamespaceFreeXmlSerializer(type) : base.CreateXmlSerializer(type);
        }

        /// <summary>
        /// Serializes and deserializes objects into and from XML documents, omitting ambient namespaces.
        /// </summary>
        private class NamespaceFreeXmlSerializer : XmlSerializer
        {
            /// <summary>
            /// An empty set of namespaces.
            /// </summary>
            private static readonly XmlSerializerNamespaces Namespaces =
                new XmlSerializerNamespaces(new[] { new XmlQualifiedName(string.Empty, string.Empty) });

            /// <summary>
            /// Provides base functionality for serializing and de-serializing objects to XML.
            /// </summary>
            private readonly XmlSerializer serializer;

            /// <summary>
            /// Initializes a new instance of the <see cref="NamespaceFreeXmlSerializer"/> class.
            /// </summary>
            /// <param name="type">The type of the object that this <see cref="XmlSerializer"/> can serialize.</param>
            public NamespaceFreeXmlSerializer(Type type)
            {
                this.serializer = new XmlSerializer(type);
            }

            /// <summary>Returns a writer used to serialize the object.</summary>
            /// <returns>
            /// An instance that implements the <see cref="XmlSerializationWriter"/> class.
            /// </returns>
            protected override XmlSerializationWriter CreateWriter()
            {
                return new NamespaceFreeXmlSerializationWriter();
            }

            /// <summary>
            /// Serializes the specified <see cref="Object"/> and writes the XML document to a file using the specified
            /// <see cref="XmlSerializationWriter"/>.
            /// </summary>
            /// <param name="o">The <see cref="T:System.Object"/> to serialize. </param>
            /// <param name="writer">The <see cref="XmlSerializationWriter"/> used to write the XML document.</param>
            protected override void Serialize(object o, XmlSerializationWriter writer)
            {
                Contract.Assume(writer is NamespaceFreeXmlSerializationWriter);

                this.serializer.Serialize((NamespaceFreeXmlSerializationWriter)writer, o, Namespaces);
            }

            /// <summary>
            /// Controls the current serialization.
            /// </summary>
            private class NamespaceFreeXmlSerializationWriter : XmlSerializationWriter
            {
                /// <summary>
                /// Converts a serialization writer to a <see cref="XmlWriter"/> instance.
                /// </summary>
                /// <param name="writer">The serialization writer to convert.</param>
                /// <returns>A <see cref="XmlWriter"/> instance.</returns>
                public static implicit operator XmlWriter(NamespaceFreeXmlSerializationWriter writer)
                {
                    return writer.Writer;
                }

                /// <summary>
                /// Initializes an instances of the <see cref="XmlSerializationWriteCallback"/>
                /// delegate to serialize SOAP-encoded XML data.
                /// </summary>
                protected override void InitCallbacks()
                {
                }
            }
        }
    }
}