using Demo.Domain.Shared.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Demo.Infrastructure.Services
{
    public class JsonService<T> : IJsonService<T>
    {
        private JsonSerializerOptions _options;

        public JsonService()
        {
            _options = new JsonSerializerOptions
            {
                IncludeFields = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
        }

        public string ToJson(T entity)
        {
            return JsonSerializer.Serialize(entity, _options);
        }

        public T FromJson(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }
    }
}
