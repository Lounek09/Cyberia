using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class ItemEffectListConverter : JsonConverter<List<IEffect>>
{
    public override List<IEffect> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return EffectFactory.GetEffectsParseFromItem(reader.GetString() ?? "").ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<IEffect> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
