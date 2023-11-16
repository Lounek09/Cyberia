using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class EffectData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("c")]
        public int CharacteristicId { get; init; }

        [JsonPropertyName("o")]
        public string Operator { get; init; }

        [JsonPropertyName("t")]
        public bool ShowInTooltip { get; init; }

        [JsonPropertyName("j")]
        public bool ShowInDiceModePossible { get; init; }

        [JsonPropertyName("e")]
        public string Element { get; init; }

        [JsonConstructor]
        internal EffectData()
        {
            Description = string.Empty;
            Operator = string.Empty;
            Element = string.Empty;
        }
    }

    public sealed class EffectsData
    {
        private const string FILE_NAME = "effects.json";

        [JsonPropertyName("E")]
        public List<EffectData> Effects { get; init; }

        [JsonConstructor]
        public EffectsData()
        {
            Effects = [];
        }

        internal static EffectsData Load()
        {
            return Json.LoadFromFile<EffectsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public EffectData? GetEffectDataById(int id)
        {
            return Effects.Find(x => x.Id == id);
        }
    }
}
