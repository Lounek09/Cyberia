using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class ReadOnlyCollectionConverter<T> : JsonConverter<ReadOnlyCollection<T>>
{
    public override ReadOnlyCollection<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var values = JsonSerializer.Deserialize<List<T>>(ref reader, options) ?? [];
        return values.AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, ReadOnlyCollection<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, options);
        }

        writer.WriteEndArray();
    }
}
