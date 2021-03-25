using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrchardCore.Queries.Sql;

namespace OrchardCore.Queries.Converters
{
    internal class SqlParameterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(object));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type != JTokenType.Array)
            {
                return token.ToObject<object>();
            }

            // array type based on first item type
            var arrayType = token.First?.Type switch
            {
                // for Floats and Integers we will use Float array, in case of mixed types array
                JTokenType.Float => typeof(float[]),
                JTokenType.Integer => typeof(float[]),
                JTokenType.Boolean => typeof(bool[]),
                JTokenType.String => typeof(string[]),
                null => typeof(string[]),
                _ => throw new SqlParameterException("Array can only contains types like: int, float, string or bool.")
            };

            try
            {
                return token.ToObject(arrayType);
            }
            catch (Exception)
            {
                throw new SqlParameterException("Array can only contains same type elements.");
            }
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
