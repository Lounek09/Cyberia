using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Incarnation
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("i")]
        public int ItemId { get; init; }

        [JsonPropertyName("s")]
        public List<int> SpellsId { get; init; }

        [JsonPropertyName("e")]
        [JsonConverter(typeof(ItemEffectsJsonConverter))]
        public List<IEffect> EffectsFromLeveling { get; init; }

        public Incarnation()
        {
            Name = string.Empty;
            SpellsId = new();
            EffectsFromLeveling = new();
        }

        public async Task<string> GetImgPath()
        {
            string url = $"{DofusApi.Instance.CdnUrl}/images/artworks/{GfxId}.png";

            if (await DofusApi.Instance.HttpClient.CheckIfPageExistsAsync(url))
                return url;

            return $"{DofusApi.Instance.CdnUrl}/images/artworks/unknown.png";
        }

        public Item? GetItem()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemById(ItemId);
        }

        public List<Spell> GetSpells()
        {
            List<Spell> spells = new();

            foreach (int spellId in SpellsId)
            {
                Spell? spell = DofusApi.Instance.Datacenter.SpellsData.GetSpellById(spellId);
                if (spell is not null)
                    spells.Add(spell);
            }

            return spells;
        }

        public List<IEffect> GetEffects()
        {
            Item? item = GetItem();
            if (item is not null)
            {
                ItemStats? itemStats = item.GetItemStat();
                if (itemStats is not null)
                {
                    List<IEffect> effects = itemStats.Effects.Where(x => x is not ExchangeableFromDateTimeEffect).ToList();
                    effects.AddRange(EffectsFromLeveling);

                    return effects;
                }
            }

            return new();
        }
    }

    public sealed class IncarnationsData
    {
        private const string FILE_NAME = "incarnation.json";

        [JsonPropertyName("INCA")]
        public List<Incarnation> Incarnations { get; init; }

        public IncarnationsData()
        {
            Incarnations = new();
        }

        internal static IncarnationsData Build()
        {
            return Json.LoadFromFile<IncarnationsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Incarnation? GetIncarnationById(int id)
        {
            return Incarnations.Find(x => x.Id == id);
        }

        public List<Incarnation> GetIncarnationsByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return Incarnations.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public Incarnation? GetIncarnationByItemId(int id)
        {
            return Incarnations.Find(x => x.ItemId == id);
        }
    }
}
