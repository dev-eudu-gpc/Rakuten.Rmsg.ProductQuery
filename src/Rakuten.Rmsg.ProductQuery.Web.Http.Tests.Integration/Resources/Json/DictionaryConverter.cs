//----------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryConverter.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Converts <see cref="JToken"/> objects within a nested object to .NET framework objects.
    /// </summary>
    public class DictionaryConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">The type of the object.</param>
        /// <returns>
        /// <see langword="true"/> if this instance can convert the specified object type;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        /// <summary>
        /// Reads the JSON representation of the nested object.
        /// </summary>
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
            return ToDictionary(JToken.Load(reader), serializer);
        }

        /// <summary>
        /// Writes the JSON representation of the nested object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        /// <summary>
        /// Converts nested <see cref="JToken"/> objects to .NET framework objects.
        /// </summary>
        /// <param name="json">The <see cref="JToken"/> object to convert.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        /// <returns>A framework object equivalent to the supplied <see cref="JToken"/>.</returns>
        private static SortedDictionary<string, object> ToDictionary(
            IEnumerable<JToken> json,
            JsonSerializer serializer)
        {
            var obj = json as JObject;
            if (obj == null)
            {
                throw new JsonSerializationException();
            }

            return new SortedDictionary<string, object>(
                obj.Properties().ToDictionary(
                    p => p.Name,
                    p => ToPrimitiveType(p.Value, serializer)));
        }

        /// <summary>
        /// Converts <see cref="JToken"/> objects to .NET framework objects.
        /// </summary>
        /// <param name="json">The <see cref="JToken"/> object to convert.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        /// <returns>A framework object equivalent to the supplied <see cref="JToken"/>.</returns>
        private static object ToPrimitiveType(IEnumerable<JToken> json, JsonSerializer serializer)
        {
            var array = json as JArray;
            if (array != null)
            {
                return ToPrimitiveType(array, serializer);
            }

            var value = json as JValue;
            if (value != null)
            {
                return ToPrimitiveType(value, serializer);
            }

            throw new JsonSerializationException();
        }

        /// <summary>
        /// Converts <see cref="JValue"/> objects to .NET framework objects.
        /// </summary>
        /// <param name="value">The <see cref="JValue"/> object to convert.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        /// <returns>A framework object equivalent to the supplied <see cref="JValue"/>.</returns>
        private static object ToPrimitiveType(JValue value, JsonSerializer serializer)
        {
            var type = GetTypes(Enumerable.Repeat(value, 1)).FirstOrDefault() ?? typeof(object);

            try
            {
                return value.ToObject(type, serializer);
            }
            catch (JsonSerializationException)
            {
                return value.ToObject<object>(serializer);
            }
        }

        /// <summary>
        /// Converts <see cref="JArray"/> objects to .NET framework objects.
        /// </summary>
        /// <param name="array">The <see cref="JArray"/> object to convert.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        /// <returns>A framework object equivalent to the supplied <see cref="JArray"/>.</returns>
        private static object ToPrimitiveType(JArray array, JsonSerializer serializer)
        {
            var type = (GetTypes(array).FirstOrDefault() ?? typeof(object)).MakeArrayType();

            try
            {
                return array.ToObject(type, serializer);
            }
            catch (JsonSerializationException)
            {
                return array.ToObject<object>(serializer);
            }
        }

        /// <summary>
        /// Gets the primitive types for a collection of <see cref="JValue"/> objects.
        /// </summary>
        /// <param name="values">The collection of <see cref="JValue"/> objects.</param>
        /// <returns>
        /// A framework object equivalent to the supplied collection of <see cref="JValue"/> objects.
        /// </returns>
        private static IEnumerable<Type> GetTypes(IEnumerable<JToken> values)
        {
            if (values == null)
            {
                yield break;
            }

            foreach (var value in values)
            {
                switch (value.Type)
                {
                    case JTokenType.Boolean:
                        yield return typeof(bool);
                        break;
                    case JTokenType.Bytes:
                        yield return typeof(byte[]);
                        break;
                    case JTokenType.Date:
                        yield return typeof(DateTime);
                        break;
                    case JTokenType.Float:
                        yield return typeof(float);
                        break;
                    case JTokenType.Guid:
                        yield return typeof(Guid);
                        break;
                    case JTokenType.Integer:
                        yield return typeof(int);
                        break;
                    case JTokenType.String:
                        yield return typeof(string);
                        break;
                    case JTokenType.TimeSpan:
                        yield return typeof(TimeSpan);
                        break;
                    case JTokenType.Uri:
                        yield return typeof(Uri);
                        break;
                }
            }
        }
    }
}