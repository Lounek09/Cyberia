using Cyberia.Api.DatacenterNS.Custom;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class MonsterSuperRaceData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("s")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int MonsterSuperRaceId { get; init; }

        public MonsterSuperRaceData()
        {
            Name = string.Empty;
        }
    }

    public sealed class MonsterRaceData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("s")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int MonsterSuperRaceId { get; init; }

        public MonsterRaceData()
        {
            Name = string.Empty;
        }

        public MonsterSuperRaceData? GetMonsterSuperRaceData()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterSuperRaceDataById(MonsterSuperRaceId);
        }
    }



    public sealed class MonsterGradeData
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

        public MonsterGradeData()
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

    public sealed class MonsterData
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
        public MonsterGradeData? MonsterGradeData1 { get; init; }

        [JsonPropertyName("g2")]
        public MonsterGradeData? MonsterGradeData2 { get; init; }

        [JsonPropertyName("g3")]
        public MonsterGradeData? MonsterGradeData3 { get; init; }

        [JsonPropertyName("g4")]
        public MonsterGradeData? MonsterGradeData4 { get; init; }

        [JsonPropertyName("g5")]
        public MonsterGradeData? MonsterGradeData5 { get; init; }

        [JsonPropertyName("g6")]
        public MonsterGradeData? MonsterGradeData6 { get; init; }

        [JsonPropertyName("g7")]
        public MonsterGradeData? MonsterGradeData7 { get; init; }

        [JsonPropertyName("g8")]
        public MonsterGradeData? MonsterGradeData8 { get; init; }

        [JsonPropertyName("g9")]
        public MonsterGradeData? MonsterGradeData9 { get; init; }

        [JsonPropertyName("g10")]
        public MonsterGradeData? MonsterGradeData10 { get; init; }

        public bool BreedSummon { get; internal set; }

        public string TrelloUrl { get; internal set; }

        public MonsterData()
        {
            Name = string.Empty;
            TrelloUrl = string.Empty;
        }

        public async Task<string> GetImagePath()
        {
            string url = $"{DofusApi.Instance.Config.CdnUrl}/images/artworks/{GfxId}.png";

            if (await DofusApi.Instance.HttpClient.ExistsAsync(url))
            {
                return url;
            }

            return $"{DofusApi.Instance.Config.CdnUrl}/images/artworks/unknown.png";
        }

        public MonsterRaceData? GetMonsterRaceData()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterRaceDataById(MonsterRaceId);
        }

        public AlignmentData? GetAlignmentData()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
        }

        public MonsterGradeData? GetMonsterGradeData(int grade = 1)
        {
            return grade switch
            {
                1 => MonsterGradeData1,
                2 => MonsterGradeData2,
                3 => MonsterGradeData3,
                4 => MonsterGradeData4,
                5 => MonsterGradeData5,
                6 => MonsterGradeData6,
                7 => MonsterGradeData7,
                8 => MonsterGradeData8,
                9 => MonsterGradeData9,
                10 => MonsterGradeData10,
                _ => null,
            };
        }

        public int GetMaxGradeNumber()
        {
            for (int i = 10; i > 0; i--)
            {
                if (GetMonsterGradeData(i) is not null)
                    return i;
            }

            return -1;
        }

        public int GetMinLevel()
        {
            return MonsterGradeData1 is null ? -1 : MonsterGradeData1.Level;
        }

        public int GetMaxLevel()
        {
            MonsterGradeData? monsterGradeData = GetMonsterGradeData(GetMaxGradeNumber());
            return monsterGradeData is null ? -1 : monsterGradeData.Level;
        }
    }

    public sealed class MonstersData
    {
        private const string FILE_NAME = "monsters.json";

        [JsonPropertyName("MSR")]
        public List<MonsterSuperRaceData> MonsterSuperRaces { get; init; }

        [JsonPropertyName("MR")]
        public List<MonsterRaceData> MonsterRaces { get; init; }

        [JsonPropertyName("M")]
        public List<MonsterData> Monsters { get; init; }

        public MonstersData()
        {
            MonsterSuperRaces = new();
            MonsterRaces = new();
            Monsters = new();
        }

        internal static MonstersData Build()
        {
            MonstersData data = Json.LoadFromFile<MonstersData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            MonstersCustomData customData = Json.LoadFromFile<MonstersCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            foreach (MonsterCustomData monsterCustomData in customData.MonstersCustom)
            {
                MonsterData? monsterData = data.GetMonsterDataById(monsterCustomData.Id);
                if (monsterData is not null)
                {
                    monsterData.BreedSummon = monsterCustomData.BreedSummon;
                    monsterData.TrelloUrl = monsterCustomData.TrelloUrl;
                }
            }

            return data;
        }

        public MonsterSuperRaceData? GetMonsterSuperRaceDataById(int id)
        {
            return MonsterSuperRaces.Find(x => x.Id == id);
        }

        public string GetMonsterSuperRaceNameById(int id)
        {
            MonsterSuperRaceData? monsterSuperRaceData = GetMonsterSuperRaceDataById(id);

            return monsterSuperRaceData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : monsterSuperRaceData.Name;
        }

        public MonsterRaceData? GetMonsterRaceDataById(int id)
        {
            return MonsterRaces.Find(x => x.Id == id);
        }

        public string GetMonsterRaceNameById(int id)
        {
            MonsterRaceData? monsterRaceData = GetMonsterRaceDataById(id);

            return monsterRaceData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : monsterRaceData.Name;
        }

        public MonsterData? GetMonsterDataById(int id)
        {
            return Monsters.Find(x => x.Id == id);
        }

        public MonsterData? GetMonsterDataByName(string name)
        {
            return Monsters.Find(m => m.Name.RemoveDiacritics().Equals(name.RemoveDiacritics()));
        }

        public List<MonsterData> GetMonstersDataByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return Monsters.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public string GetMonsterNameById(int id)
        {
            MonsterData? monsterData = GetMonsterDataById(id);

            return monsterData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : monsterData.Name;
        }
    }
}
