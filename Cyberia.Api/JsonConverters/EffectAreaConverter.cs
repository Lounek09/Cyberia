using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Managers;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class EffectAreaConverter : JsonConverter<EffectArea>
{
    public override EffectArea Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return EffectAreaFactory.Create(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, EffectArea value, JsonSerializerOptions options)
    {
        var encodedValue = $"{(char)value.Id}{PatternDecoder.Base64IndexToChar(value.Size)}";
        writer.WriteStringValue(encodedValue);
    }
}
