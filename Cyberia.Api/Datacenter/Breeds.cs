using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Breed
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

        public int BreedSpellId { get; internal set; }

        public int ItemSetId { get; internal set; }

        public Breed()
        {
            Name = string.Empty;
            LongName = string.Empty;
            Description = string.Empty;
            ShortDescription = string.Empty;
            SpellsId = new();
            TemporisPassiveName = string.Empty;
            TemporisPassiveDescription = string.Empty;
            CloseCombatInfos = new();
            StrengthBoostCost = new();
            VitalityBoostCost = new();
            WisdomBoostCost = new();
            LuckBoostCost = new();
            AgilityBoostCost = new();
            IntelligenceBoostCost = new();
        }

        public string GetIconImgPath()
        {
            return $"{DofusApi.Instance.CdnUrl}/images/breeds/icons/{Id}.png";
        }

        public string GetPreferenceWeaponsImgPath()
        {
            return $"{DofusApi.Instance.CdnUrl}/images/breeds/preference_weapons/weapons_{Id}.png";
        }

        public List<Spell> GetSpells()
        {
            List<Spell> spells = new();

            foreach (int spellId in SpellsId)
            {
                Spell? spell = DofusApi.Instance.Datacenter.SpellsData.GetSpellById(spellId);
                if (spell is not null && (!DofusApi.Instance.Temporis || spell.SpellLevel4 is null))
                    spells.Add(spell);
            }

            return spells;
        }

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
                tab[l[1], 0] = "|" + l[0].ToString().PadLeft(4) + " ";
            foreach (List<int> l in WisdomBoostCost)
                tab[l[1], 1] = "|" + l[0].ToString().PadLeft(4) + " ";
            foreach (List<int> l in StrengthBoostCost)
                tab[l[1], 2] = "|" + l[0].ToString().PadLeft(4) + " ";
            foreach (List<int> l in IntelligenceBoostCost)
                tab[l[1], 3] = "|" + l[0].ToString().PadLeft(4) + " ";
            foreach (List<int> l in LuckBoostCost)
                tab[l[1], 4] = "|" + l[0].ToString().PadLeft(4) + " ";
            foreach (List<int> l in AgilityBoostCost)
                tab[l[1], 5] = "|" + l[0].ToString().PadLeft(4) + " ";

            string value = "`    |1 / " + (Id == 11 ? 2 : 1) + "|2 / 1|3 / 1|4 / 1|5 / 1`\n";

            for (int i = 0; i < 6; i++)
            {
                value += "`";
                for (int j = 0; j < 6; j++)
                {
                    if (tab[j, i] is null)
                        value += "|     ";
                    else
                        value += tab[j, i];
                }
                value += "`\n";
            }

            return value;
        }

        public Spell? GetBreedSpell()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellById(BreedSpellId);
        }

        public ItemSet? GetItemSet()
        {
            return DofusApi.Instance.Datacenter.ItemSetsData.GetItemSetById(ItemSetId);
        }
    }

    public sealed class BreedsData
    {
        private const string FILE_NAME = "classes.json";

        [JsonPropertyName("G")]
        public List<Breed> Breeds { get; init; }

        public BreedsData()
        {
            Breeds = new();
        }

        internal static BreedsData Build()
        {
            BreedsData data = Json.LoadFromFile<BreedsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
            BreedsCustomData customData = Json.LoadFromFile<BreedsCustomData>($"{DofusApi.CUSTOM_PATH}/{FILE_NAME}");

            foreach (BreedCustom breedCustom in customData.BreedsCustom)
            {
                Breed? breed = data.GetBreedById(breedCustom.Id);
                if (breed is not null)
                {
                    breed.BreedSpellId = breedCustom.BreedSpellId;
                    breed.ItemSetId = breedCustom.ItemSetId;
                }
            }

            return data;
        }

        public Breed? GetBreedById(int id)
        {
            return Breeds.Find(x => x.Id == id);
        }

        public Breed? GetBreedByName(string name)
        {
            return Breeds.Find(x => x.Name.RemoveDiacritics().Equals(name.RemoveDiacritics()));
        }

        public List<Breed> GetBreedsByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return Breeds.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public string GetBreedNameById(int id)
        {
            Breed? breed = GetBreedById(id);

            return breed is null ? $"Inconnu ({id})" : breed.Name;
        }
    }
}
