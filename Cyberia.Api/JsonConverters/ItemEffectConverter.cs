using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters
{
    public sealed class ItemEffectConverter : JsonConverter<IEffect>
    {
        public override IEffect Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            IEnumerable<IEffect> effects = EffectFactory.GetEffectsParseFromItem(reader.GetString() ?? string.Empty);
            return effects.First();
        }

        public override void Write(Utf8JsonWriter writer, IEffect values, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
