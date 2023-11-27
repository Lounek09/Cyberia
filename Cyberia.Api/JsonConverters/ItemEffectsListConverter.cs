using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class ItemEffectsListConverter : JsonConverter<List<IEnumerable<IEffect>>>
{
    public override List<IEnumerable<IEffect>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var compressedEffects = JsonSerializer.Deserialize<string[]>(ref reader, options) ?? [];
        return compressedEffects.Select(x => EffectFactory.GetEffectsParseFromItem(x)).ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<IEnumerable<IEffect>> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
