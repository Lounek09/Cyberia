using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class IncarnationData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("s")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> SpellsId { get; init; }

        [JsonPropertyName("e")]
        [JsonInclude]
        [JsonConverter(typeof(ItemEffectListConverter))]
        private List<IEffect> _effectsFromLeveling;

        [JsonIgnore]
        public ReadOnlyCollection<IEffect> EffectsFromLeveling => _effectsFromLeveling.AsReadOnly();

        [JsonConstructor]
        internal IncarnationData()
        {
            Name = string.Empty;
            SpellsId = ReadOnlyCollection<int>.Empty;
            _effectsFromLeveling = [];
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

        public ReadOnlyCollection<IEffect> GetEffects()
        {
            ItemData? itemData = GetItemData();
            if (itemData is not null)
            {
                ItemStatsData? itemStatsData = itemData.GetItemStatsData();
                if (itemStatsData is not null)
                {
                    List<IEffect> effects = itemStatsData.Effects.Where(x => x is not ExchangeableEffect).ToList();
                    effects.AddRange(_effectsFromLeveling);

                    return effects.AsReadOnly();
                }
            }

            return EffectsFromLeveling;
        }
    }

    public sealed class IncarnationsData : IDofusData
    {
        private const string FILE_NAME = "incarnation.json";

        [JsonPropertyName("INCA")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, IncarnationData>))]
        public FrozenDictionary<int, IncarnationData> Incarnations { get; init; }

        [JsonConstructor]
        internal IncarnationsData()
        {
            Incarnations = FrozenDictionary<int, IncarnationData>.Empty;
        }

        internal static IncarnationsData Load()
        {
            return Datacenter.LoadDataFromFile<IncarnationsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public IncarnationData? GetIncarnationDataByItemId(int id)
        {
            Incarnations.TryGetValue(id, out IncarnationData? incarnationData);
            return incarnationData;
        }

        public IEnumerable<IncarnationData> GetIncarnationsDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Incarnations.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
        }
    }
}
