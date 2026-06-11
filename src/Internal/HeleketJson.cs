using Newtonsoft.Json;
using Flurl.Http.Configuration;

namespace Heleket.Internal;

internal static class HeleketJson
{
    internal static readonly JsonSerializerSettings Settings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.None
    };

    internal static readonly ISerializer FlurlSerializer = new NewtonsoftFlurlSerializer(Settings);

    internal static string Serialize(object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return JsonConvert.SerializeObject(value, Settings);
    }

    internal static T? Deserialize<T>(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        return JsonConvert.DeserializeObject<T>(json, Settings);
    }

    private sealed class NewtonsoftFlurlSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public NewtonsoftFlurlSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T Deserialize<T>(string s)
        {
            return JsonConvert.DeserializeObject<T>(s, _settings)!;
        }

        public T Deserialize<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            var serializer = JsonSerializer.Create(_settings);

            return serializer.Deserialize<T>(jsonReader)!;
        }
    }
}
