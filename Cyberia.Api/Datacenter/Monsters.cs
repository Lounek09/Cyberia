using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class MonsterSuperRace
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("s")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int MonsterSuperRaceId { get; init; }

        public MonsterSuperRace()
        {
            Name = string.Empty;
        }
    }

    public sealed class MonsterRace
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("s")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int MonsterSuperRaceId { get; init; }

        public MonsterRace()
        {
            Name = string.Empty;
        }

        public MonsterSuperRace? GetMonsterSuperRace()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterSuperRaceById(MonsterSuperRaceId);
        }
    }



    public sealed class MonsterGrade
    {
        [JsonPropertyName("l")]
        public int Level { get; init; }

        [JsonPropertyName("r")]
        public List<int> Resistances { get; init; }

        [JsonPropertyName("lp")]
        public int? LifePoint { get; init; }

        [JsonPropertyName("ap")]
        public int? ActionPoint { get; init; }

        [JsonPropertyName("mp")]
        public int? MovementPoint { get; init; }

        public MonsterGrade()
        {
            Resistances = new();
        }

        public int GetNeutralResistance()
        {
            return Resistances.Count > 0 ? Resistances[0] : 0;
        }

        public int GetEarthResistance()
        {
            return Resistances.Count > 1 ? Resistances[1] : 0;
        }

        public int GetFireResistance()
        {
            return Resistances.Count > 2 ? Resistances[2] : 0;
        }

        public int GetWaterResistance()
        {
            return Resistances.Count > 3 ? Resistances[3] : 0;
        }

        public int GetAirResistance()
        {
            return Resistances.Count > 4 ? Resistances[4] : 0;
        }

        public int GetActionPointDodge()
        {
            return Resistances.Count > 5 ? Resistances[5] : 0;
        }

        public int GetMovementPointDodge()
        {
            return Resistances.Count > 6 ? Resistances[6] : 0;
        }
    }

    public sealed class Monster
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("b")]
        public int MonsterRaceId { get; init; }

        [JsonPropertyName("a")]
        public int AlignmentId { get; init; }

        [JsonPropertyName("k")]
        public bool Kickable { get; init; }

        [JsonPropertyName("d")]
        public bool Boss { get; init; }

        [JsonPropertyName("s")]
        public bool SearcheableInBigStore { get; init; }

        [JsonPropertyName("g1")]
        public MonsterGrade? MonsterGrade1 { get; init; }

        [JsonPropertyName("g2")]
        public MonsterGrade? MonsterGrade2 { get; init; }

        [JsonPropertyName("g3")]
        public MonsterGrade? MonsterGrade3 { get; init; }

        [JsonPropertyName("g4")]
        public MonsterGrade? MonsterGrade4 { get; init; }

        [JsonPropertyName("g5")]
        public MonsterGrade? MonsterGrade5 { get; init; }

        [JsonPropertyName("g6")]
        public MonsterGrade? MonsterGrade6 { get; init; }

        [JsonPropertyName("g7")]
        public MonsterGrade? MonsterGrade7 { get; init; }

        [JsonPropertyName("g8")]
        public MonsterGrade? MonsterGrade8 { get; init; }

        [JsonPropertyName("g9")]
        public MonsterGrade? MonsterGrade9 { get; init; }

        [JsonPropertyName("g10")]
        public MonsterGrade? MonsterGrade10 { get; init; }

        public bool BreedSummon { get; internal set; }

        public string TrelloUrl { get; internal set; }

        public Monster()
        {
            Name = string.Empty;
            TrelloUrl = string.Empty;
        }

        public async Task<string> GetImgPath()
        {
            string url = $"{DofusApi.Instance.CdnUrl}/images/artworks/{GfxId}.png";

            if (await DofusApi.Instance.HttpClient.CheckIfPageExistsAsync(url))
                return url;

            return $"{DofusApi.Instance.CdnUrl}/images/artworks/unknown.png";
        }

        public MonsterRace? GetRace()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterRaceById(MonsterRaceId);
        }

        public Alignment? GetAlignment()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentById(AlignmentId);
        }

        public string GetAlignementName()
        {
            return AlignmentId == -1 ? "" : DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentNameById(AlignmentId);
        }

        public MonsterGrade? GetGrade(int grade = 1)
        {
            switch (grade)
            {
                case 1:
                    return MonsterGrade1;
                case 2:
                    return MonsterGrade2;
                case 3:
                    return MonsterGrade3;
                case 4:
                    return MonsterGrade4;
                case 5:
                    return MonsterGrade5;
                case 6:
                    return MonsterGrade6;
                case 7:
                    return MonsterGrade7;
                case 8:
                    return MonsterGrade8;
                case 9:
                    return MonsterGrade9;
                case 10:
                    return MonsterGrade10;
                default:
                    return null;
            }
        }

        public int GetMaxGradeNumber()
        {
            for (int i = 10; i > 0; i--)
            {
                if (GetGrade(i) is not null)
                    return i;
            }

            return -1;
        }

        public int GetMinLevel()
        {
            return MonsterGrade1 is null ? -1 : MonsterGrade1.Level;
        }

        public int GetMaxLevel()
        {
            MonsterGrade? grade = GetGrade(GetMaxGradeNumber());
            return grade is null ? -1 : grade.Level;
        }
    }

    public sealed class MonstersData
    {
        private const string FILE_NAME = "monsters.json";

        [JsonPropertyName("MSR")]
        public List<MonsterSuperRace> MonsterSuperRaces { get; init; }

        [JsonPropertyName("MR")]
        public List<MonsterRace> MonsterRaces { get; init; }

        [JsonPropertyName("M")]
        public List<Monster> Monsters { get; init; }

        public MonstersData()
        {
            MonsterSuperRaces = new();
            MonsterRaces = new();
            Monsters = new();
        }

        internal static MonstersData Build()
        {
            MonstersData data = Json.LoadFromFile<MonstersData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
            MonstersCustomData customData = Json.LoadFromFile<MonstersCustomData>($"{DofusApi.CUSTOM_PATH}/{FILE_NAME}");

            foreach (MonsterCustom monsterCustom in customData.MonstersCustom)
            {
                Monster? monster = data.GetMonsterById(monsterCustom.Id);
                if (monster is not null)
                {
                    monster.BreedSummon = monsterCustom.BreedSummon;
                    monster.TrelloUrl = monsterCustom.TrelloUrl;
                }
            }

            return data;
        }

        public MonsterSuperRace? GetMonsterSuperRaceById(int id)
        {
            return MonsterSuperRaces.Find(x => x.Id == id);
        }

        public string GetMonsterSuperRaceNameById(int id)
        {
            MonsterSuperRace? monsterSuperRace = GetMonsterSuperRaceById(id);

            return monsterSuperRace is null ? $"Inconnu ({id})" : monsterSuperRace.Name;
        }

        public MonsterRace? GetMonsterRaceById(int id)
        {
            return MonsterRaces.Find(x => x.Id == id);
        }

        public string GetMonsterRaceNameById(int id)
        {
            MonsterRace? monsterRace = GetMonsterRaceById(id);

            return monsterRace is null ? $"Inconnu ({id})" : monsterRace.Name;
        }

        public Monster? GetMonsterById(int id)
        {
            return Monsters.Find(x => x.Id == id);
        }

        public Monster? GetMonsterByName(string name)
        {
            return Monsters.Find(m => m.Name.RemoveDiacritics().Equals(name.RemoveDiacritics()));
        }

        public List<Monster> GetMonstersByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return Monsters.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public string GetMonsterNameById(int id)
        {
            Monster? monster = GetMonsterById(id);

            return monster is null ? $"Inconnu ({id})" : monster.Name;
        }
    }
}
