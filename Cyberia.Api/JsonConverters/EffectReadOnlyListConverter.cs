using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class EffectReadOnlyListConverter
    : JsonConverter<IReadOnlyList<IEffect>>
{
    public override IReadOnlyList<IEffect> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return EffectFactory.CreateMany(reader.GetString() ?? string.Empty).ToList();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<IEffect> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
