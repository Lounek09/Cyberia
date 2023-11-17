using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
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
        [JsonConverter(typeof(ItemEffectListConverter))]
        public List<IEffect> EffectsFromLeveling { get; init; }

        [JsonConstructor]
        internal IncarnationData()
        {
            Name = string.Empty;
            SpellsId = [];
            EffectsFromLeveling = [];
        }

        public async Task<string> GetImgPath()
        {
            string url = $"{DofusApi.Config.CdnUrl}/images/artworks/{GfxId}.png";

            if (await DofusApi.HttpClient.ExistsAsync(url))
            {
                return url;
            }

            return $"{DofusApi.Config.CdnUrl}/images/artworks/unknown.png";
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Datacenter.ItemsData.GetItemDataById(Id);
        }

        public IEnumerable<SpellData> GetSpellsData()
        {
            foreach (int spellId in SpellsId)
            {
                SpellData? spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spellId);
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
                ItemStatsData? itemStatsData = itemData.GetItemStatsData();
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

        [JsonConstructor]
        public IncarnationsData()
        {
            Incarnations = [];
        }

        internal static IncarnationsData Load()
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
