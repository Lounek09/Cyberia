using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class IncarnationData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("s")]
        public List<int> SpellsId { get; init; }

        [JsonPropertyName("e")]
        [JsonConverter(typeof(ItemEffectListJsonConverter))]
        public List<IEffect> EffectsFromLeveling { get; init; }

        public IncarnationData()
        {
            Name = string.Empty;
            SpellsId = new();
            EffectsFromLeveling = new();
        }

        public async Task<string> GetImgPath()
        {
            string url = $"{DofusApi.Instance.Config.CdnUrl}/images/artworks/{GfxId}.png";

            if (await DofusApi.Instance.HttpClient.ExistsAsync(url))
            {
                return url;
            }

            return $"{DofusApi.Instance.Config.CdnUrl}/images/artworks/unknown.png";
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(Id);
        }

        public IEnumerable<SpellData> GetSpellsData()
        {
            foreach (int spellId in SpellsId)
            {
                SpellData? spellData = DofusApi.Instance.Datacenter.SpellsData.GetSpellDataById(spellId);
                if (spellData is not null)
                {
                    yield return spellData;
                }
            }
        }

        public List<IEffect> GetEffects()
        {
            ItemData? itemData = GetItemData();
            if (itemData is not null)
            {
                ItemStatsData? itemStatsData = itemData.GetItemStatData();
                if (itemStatsData is not null)
                {
                    List<IEffect> effects = itemStatsData.Effects.Where(x => x is not ExchangeableEffect).ToList();
                    effects.AddRange(EffectsFromLeveling);

                    return effects;
                }
            }

            return EffectsFromLeveling;
        }
    }

    public sealed class IncarnationsData
    {
        private const string FILE_NAME = "incarnation.json";

        [JsonPropertyName("INCA")]
        public List<IncarnationData> Incarnations { get; init; }

        public IncarnationsData()
        {
            Incarnations = new();
        }

        internal static IncarnationsData Build()
        {
            return Json.LoadFromFile<IncarnationsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public IncarnationData? GetIncarnationDataByItemId(int id)
        {
            return Incarnations.Find(x => x.Id == id);
        }

        public List<IncarnationData> GetIncarnationsDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Incarnations.FindAll(x => names.All(x.Name.NormalizeCustom().Contains));
        }
    }
}
