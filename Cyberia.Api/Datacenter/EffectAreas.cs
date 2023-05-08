using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class EffectArea
    {
        [JsonPropertyName("s")]
        public char Symbol { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        public EffectArea()
        {
            Name = string.Empty;
        }
    }

    public sealed class EffectAreasData
    {
        private const string FILE_NAME = "effectareas.json";

        [JsonPropertyName("EA")]
        public List<EffectArea> EffectAreas { get; init; }

        public EffectAreasData()
        {
            EffectAreas = new();
        }

        internal static EffectAreasData Build()
        {
            return Json.LoadFromFile<EffectAreasData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public EffectArea? GetEffectAreaBySymbol(char symbol)
        {
            return EffectAreas.Find(x => x.Symbol == symbol);
        }
    }
}
