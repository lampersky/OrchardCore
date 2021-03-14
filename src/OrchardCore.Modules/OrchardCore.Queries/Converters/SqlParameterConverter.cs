using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            if (token.Type == JTokenType.Array)
            {
                if (token.First != null)
                {
                    // result type based on first item type
                    switch (token.First.Type)
                    {
                        // for Floats and Integers we will use Float array, in case of mixed types array
                        case JTokenType.Float:
                        case JTokenType.Integer:
                            try
                            {
                                return token.ToObject<float[]>();
                            }
                            catch (Exception)
                            {
                                // in case of mixed types
                                return token.ToObject<object[]>();
                            }
                        case JTokenType.String:
                            return token.ToObject<string[]>();
                    };
                }
                return token.ToObject<object[]>();
            }
            return token;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
