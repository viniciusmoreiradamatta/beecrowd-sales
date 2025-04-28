using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SalesDomain.Extensions;

public static class SerializationExtensions
{
    public static T Deserialize<T>(this string value)
    {
        return JsonSerializer.Deserialize<T>(value, options: DefaultOptions());
    }

    public static T DeserializeBytes<T>(this byte[] bytes)
    {
        return Deserialize<T>(Encoding.UTF8.GetString(bytes));
    }

    public static string Serialize<T>(this T value)
    {
        return JsonSerializer.Serialize(value, options: DefaultOptions());
    }

    public static byte[] SerializeBytes<T>(this T value)
    {
        return Encoding.UTF8.GetBytes(value.Serialize());
    }

    private static JsonSerializerOptions DefaultOptions()
    {
        return new JsonSerializerOptions { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = false };
    }
}