using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Effect
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

        public Effect()
        {
            Description = string.Empty;
            Operator = string.Empty;
            Element = string.Empty;
        }

        public bool IsDamagingEffect()
        {
            return DofusApi.Instance.Datacenter.EffectsData.DamagingEffectsId.Contains(Id);
        }

        public bool IsHealingEffect()
        {
            return DofusApi.Instance.Datacenter.EffectsData.HealingEffectsId.Contains(Id);
        }
    }

    public sealed class EffectsData
    {
        private const string FILE_NAME = "effects.json";

        [JsonPropertyName("E")]
        public List<Effect> Effects { get; init; }

        [JsonPropertyName("EDMG")]
        public List<int> DamagingEffectsId { get; init; }

        [JsonPropertyName("EHEL")]
        public List<int> HealingEffectsId { get; init; }

        public EffectsData()
        {
            Effects = new();
            DamagingEffectsId = new();
            HealingEffectsId = new();
        }

        internal static EffectsData Build()
        {
            return Json.LoadFromFile<EffectsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Effect? GetEffectById(int id)
        {
            return Effects.Find(x => x.Id == id);
        }
    }
}
