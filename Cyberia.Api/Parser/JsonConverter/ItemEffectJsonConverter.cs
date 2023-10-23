using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Parser.JsonConverter
{
    public sealed class ItemEffectJsonConverter : JsonConverter<IEffect>
    {
        public override IEffect Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonElement element = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
            List<IEffect> effects = EffectFactory.GetEffectsParseFromItem(element.GetString() ?? "").ToList();

            if (effects.Count == 0)
            {
                throw new JsonException("Invalid JSON format: unable to parse item effect");
            }

            return effects[0];
        }

        public override void Write(Utf8JsonWriter writer, IEffect values, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
