using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class EffectsReadOnlyListOfReadOnlyListConverter : JsonConverter<IReadOnlyList<IReadOnlyList<IEffect>>>
{
    public override IReadOnlyList<IReadOnlyList<IEffect>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var compressedEffects = JsonSerializer.Deserialize<string[]>(ref reader, options) ?? [];

        return compressedEffects
            .Select(x => EffectFactory.CreateMany(x))
            .ToList();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<IReadOnlyList<IEffect>> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
