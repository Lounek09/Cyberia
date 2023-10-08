using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Parser.JsonConverter
{
    public sealed class ItemEffectListListJsonConverter : JsonConverter<List<List<IEffect>>>
    {
        public override List<List<IEffect>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not JsonTokenType.StartArray)
                throw new JsonException("Invalid JSON format: expected an array.");

            string[]? compressedEffects = JsonSerializer.Deserialize<string[]>(ref reader, options) ?? throw new JsonException($"Invalid JSON format: unable to deserialize into an array of string.");
            return compressedEffects.Select(x => EffectFactory.GetEffectsParseFromItem(x).ToList()).ToList();
        }

        public override void Write(Utf8JsonWriter writer, List<List<IEffect>> values, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
