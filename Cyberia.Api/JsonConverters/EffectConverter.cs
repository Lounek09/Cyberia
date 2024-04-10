using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class EffectConverter : JsonConverter<IEffect>
{
    public override IEffect Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return EffectFactory.Create(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, IEffect values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
