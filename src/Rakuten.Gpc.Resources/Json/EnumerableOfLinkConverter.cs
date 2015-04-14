//----------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableOfLinkConverter.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Json
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>Converts an <see cref="IEnumerable{Link}"/> object to and from JSON.</summary>
    public sealed class EnumerableOfLinkConverter : JsonConverter
    {
        /// <summary>Determines whether this instance can convert the specified object type.</summary>
        /// <param name="objectType">The type of the object.</param>
        /// <returns>
        /// <see langword="true"/> if this instance can convert the specified object type;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }

            return objectType.IsVisible && typeof(Link).IsAssignableFrom(objectType);
        }

        /// <summary>Reads the JSON representation of the <see cref="IEnumerable{Link}"/>.</summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">The type of the object.</param>
        /// <param name="existingValue">The existing value of the object being read.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            return new Collection<Link>(
                JObject.Load(reader).Properties().SelectMany(
                    GetCollection,
                    (prop, value) => GetLink(prop, value, serializer)).ToList());
        }

        /// <summary>Writes the JSON representation of the <see cref="IEnumerable{Link}"/>.</summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            var links = value as IEnumerable<Link>;

            if (links != null)
            {
                WriteJson(writer, links, serializer);
            }
        }

        /// <summary>Gets a collection of the values of a given property.</summary>
        /// <param name="property">The property whose values should be returned.</param>
        /// <returns>A set of values for the specified property.</returns>
        private static IEnumerable<JToken> GetCollection(JProperty property)
        {
            return property.Value is JArray ? property.Value.Children() : Enumerable.Repeat(property.Value, 1);
        }

        /// <summary>Gets a link from its JSON representation.</summary>
        /// <param name="property">The JSON property representing the link.</param>
        /// <param name="value">The value of the property.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        /// <returns>A link that is equivalent to that represented by <paramref name="value"/>.</returns>
        private static Link GetLink(JProperty property, JToken value, JsonSerializer serializer)
        {
            switch (value.Type)
            {
                case JTokenType.String:
                    return new Link { RelationType = property.Name, Target = value.Value<string>() };
                case JTokenType.Null:
                    return new Link { RelationType = property.Name };
                default:
                    var link = value.ToObject<Link>(serializer);
                    link.RelationType = property.Name;
                    return link;
            }
        }

        /// <summary>Writes the JSON representation of the <see cref="IEnumerable{Link}"/>.</summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        private static void WriteJson(JsonWriter writer, IEnumerable<Link> value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            foreach (var c in value.GroupBy(l => l.RelationType))
            {
                writer.WritePropertyName(c.Key);

                if (c.Count() == 1)
                {
                    serializer.Serialize(writer, c.FirstOrDefault());
                }
                else
                {
                    serializer.Serialize(writer, c);
                }
            }

            writer.WriteEndObject();
        }
    }
}