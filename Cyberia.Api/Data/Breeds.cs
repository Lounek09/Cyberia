using Cyberia.Api.Data.Custom;
using Cyberia.Api.Values;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class BreedData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("sn")]
        public string Name { get; init; }

        [JsonPropertyName("ln")]
        public string LongName { get; init; }

        [JsonPropertyName("ep")]
        public int Episode { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("sd")]
        public string ShortDescription { get; init; }

        [JsonPropertyName("di")]
        public bool Diabolical { get; init; }

        [JsonPropertyName("s")]
        public List<int> SpellsId { get; init; }

        [JsonPropertyName("pt")]
        public string TemporisPassiveName { get; init; }

        [JsonPropertyName("pd")]
        public string TemporisPassiveDescription { get; init; }

        [JsonPropertyName("cc")]
        public List<object> CloseCombatInfos { get; init; }

        [JsonPropertyName("b10")]
        public List<List<int>> StrengthBoostCost { get; init; }

        [JsonPropertyName("b11")]
        public List<List<int>> VitalityBoostCost { get; init; }

        [JsonPropertyName("b12")]
        public List<List<int>> WisdomBoostCost { get; init; }

        [JsonPropertyName("b13")]
        public List<List<int>> LuckBoostCost { get; init; }

        [JsonPropertyName("b14")]
        public List<List<int>> AgilityBoostCost { get; init; }

        [JsonPropertyName("b15")]
        public List<List<int>> IntelligenceBoostCost { get; init; }

        [JsonIgnore]
        public int SpecialSpellId { get; internal set; }

        [JsonIgnore]
        public int ItemSetId { get; internal set; }

        [JsonConstructor]
        internal BreedData()
        {
            Name = string.Empty;
            LongName = string.Empty;
            Description = string.Empty;
            ShortDescription = string.Empty;
            SpellsId = [];
            TemporisPassiveName = string.Empty;
            TemporisPassiveDescription = string.Empty;
            CloseCombatInfos = [];
            StrengthBoostCost = [];
            VitalityBoostCost = [];
            WisdomBoostCost = [];
            LuckBoostCost = [];
            AgilityBoostCost = [];
            IntelligenceBoostCost = [];
        }

        public string GetIconImagePath()
        {
            return $"{DofusApi.Config.CdnUrl}/images/breeds/icons/{Id}.png";
        }

        public string GetPreferenceWeaponsImagePath()
        {
            return $"{DofusApi.Config.CdnUrl}/images/breeds/preference_weapons/weapons_{Id}.png";
        }

        public IEnumerable<SpellData> GetSpellsData()
        {
            foreach (int spellId in SpellsId)
            {
                SpellData? spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spellId);
                if (spellData is not null && (!DofusApi.Config.Temporis || spellData.SpellCategory is SpellCategory.TemporisBreed))
                {
                    yield return spellData;
                }
            }
        }


        //TODO: Re-do this piece of shit
        public string GetCaracteristics()
        {
            string[,] tab = new string[6, 6];

            tab[0, 0] = "Vita";
            tab[0, 1] = "Sasa";
            tab[0, 2] = "Fo  ";
            tab[0, 3] = "Int ";
            tab[0, 4] = "Cha ";
            tab[0, 5] = "Age ";

            foreach (List<int> l in VitalityBoostCost)
            {
                tab[l[1], 0] = "|" + l[0].ToString().PadLeft(4) + " ";
            }

            foreach (List<int> l in WisdomBoostCost)
            {
                tab[l[1], 1] = "|" + l[0].ToString().PadLeft(4) + " ";
            }

            foreach (List<int> l in StrengthBoostCost)
            {
                tab[l[1], 2] = "|" + l[0].ToString().PadLeft(4) + " ";
            }

            foreach (List<int> l in IntelligenceBoostCost)
            {
                tab[l[1], 3] = "|" + l[0].ToString().PadLeft(4) + " ";
            }

            foreach (List<int> l in LuckBoostCost)
            {
                tab[l[1], 4] = "|" + l[0].ToString().PadLeft(4) + " ";
            }

            foreach (List<int> l in AgilityBoostCost)
            {
                tab[l[1], 5] = "|" + l[0].ToString().PadLeft(4) + " ";
            }

            string value = "`    |1 / " + (Id == 11 ? 2 : 1) + "|2 / 1|3 / 1|4 / 1|5 / 1`\n";

            for (int i = 0; i < 6; i++)
            {
                value += "`";
                for (int j = 0; j < 6; j++)
                {
                    if (tab[j, i] is null)
                    {
                        value += "|     ";
                    }
                    else
                    {
                        value += tab[j, i];
                    }
                }
                value += "`\n";
            }

            return value;
        }

        public SpellData? GetSpecialSpellData()
        {
            return DofusApi.Datacenter.SpellsData.GetSpellDataById(SpecialSpellId);
        }

        public ItemSetData? GetItemSetData()
        {
            return DofusApi.Datacenter.ItemSetsData.GetItemSetDataById(ItemSetId);
        }
    }

    public sealed class BreedsData
    {
        private const string FILE_NAME = "classes.json";

        [JsonPropertyName("G")]
        public List<BreedData> Breeds { get; init; }

        [JsonConstructor]
        public BreedsData()
        {
            Breeds = [];
        }

        internal static BreedsData Load()
        {
            BreedsData data = Json.LoadFromFile<BreedsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            BreedsCustomData customData = Json.LoadFromFile<BreedsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            foreach (BreedCustomData breedCustomData in customData.Breeds)
            {
                BreedData? breedData = data.GetBreedDataById(breedCustomData.Id);
                if (breedData is not null)
                {
                    breedData.SpecialSpellId = breedCustomData.SpecialSpellId;
                    breedData.ItemSetId = breedCustomData.ItemSetId;
                }
            }

            return data;
        }

        public BreedData? GetBreedDataById(int id)
        {
            return Breeds.Find(x => x.Id == id);
        }

        public BreedData? GetBreedDataByName(string name)
        {
            return Breeds.Find(x => x.Name.NormalizeCustom().Equals(name.NormalizeCustom()));
        }

        public List<BreedData> GetBreedsDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Breeds.FindAll(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetBreedNameById(int id)
        {
            BreedData? breed = GetBreedDataById(id);

            return breed is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : breed.Name;
        }
    }
}
