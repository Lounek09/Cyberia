using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class ConvertToReadOnlyListConverter<T> : JsonConverter<IReadOnlyList<T>>
{
    public override IReadOnlyList<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        if (typeof(T) == typeof(string))
        {
            return (IReadOnlyList<T>)elements.Select(x =>
            {
                return x.ValueKind switch
                {
                    JsonValueKind.String => x.GetString() ?? string.Empty,
                    JsonValueKind.Number => x.GetDouble().ToString(),
                    _ => throw new JsonException(),
                };
            })
            .ToList();
        }

        return elements.Select(x => x.Deserialize<T>(options) ?? throw new JsonException()).ToList();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<T> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
