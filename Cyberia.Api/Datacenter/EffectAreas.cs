using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class EffectArea
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        public EffectArea()
        {
            Name = string.Empty;
        }

        public string GetImagePath()
        {
            return $"{DofusApi.Instance.Config.CdnUrl}/images/effectareas/{Id}.png";
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

        public EffectArea? GetEffectAreaById(int id)
        {
            return EffectAreas.Find(x => x.Id == id);
        }
    }
}
