using System.Text;
using Newtonsoft.Json;

namespace NetMQ.ReactiveExtensions;

/// <summary>
/// Intent: Allow us to serialize using Newtonsoft.Json.
/// </summary>
public static class SerializeViaNewtonsoftJson
{
    public static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
    };
    internal static byte[] SerializeJson<T>(this T message)
    {
        var json = JsonConvert.SerializeObject(message, JsonSerializerSettings);
        var result = Encoding.UTF8.GetBytes(json);
        return result;
    }

    internal static T DeserializeJson<T>(this byte[] bytes)
    {
        var json = Encoding.UTF8.GetString(bytes);
        var result = JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings)!;
        return result;
    }
}