using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class EffectData : IDofusData<int>
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

    public sealed class EffectsData : IDofusData
    {
        private const string FILE_NAME = "effects.json";

        [JsonPropertyName("E")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, EffectData>))]
        public FrozenDictionary<int, EffectData> Effects { get; init; }

        [JsonConstructor]
        internal EffectsData()
        {
            Effects = FrozenDictionary<int, EffectData>.Empty;
        }

        internal static EffectsData Load()
        {
            return Datacenter.LoadDataFromFile<EffectsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public EffectData? GetEffectDataById(int id)
        {
            Effects.TryGetValue(id, out EffectData? effectData);
            return effectData;
        }
    }
}
