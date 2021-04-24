using Demo.Domain.Shared.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Demo.Infrastructure.Services
{
    public class JsonService<T> : IJsonService<T>
    {
        private JsonSerializerSettings _settings;

        public JsonService()
        {
            _settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            _settings.Converters.Add(new StringEnumConverter());
        }

        public string ToJson(T entity)
        {
            return JsonConvert.SerializeObject(entity, _settings);
        }

        public T FromJson(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}
