using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Parser.JsonConverter;
using Cyberia.Api.Values;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class SpellIconData
    {
        public const int INDEX_REMASTERED = 0;
        public const int INDEX_CONTRAST = 1;
        public const int INDEX_CLASSIC_ANGELIC = 2;
        public const int INDEX_CLASSIC_DIABOLIC = 3;

        [JsonPropertyName("up")]
        public int UpGfxId { get; init; }

        [JsonPropertyName("pc")]
        public List<int> PrintColors { get; init; }

        [JsonPropertyName("b")]
        public int BackgroundGfxId { get; init; }

        [JsonPropertyName("fc")]
        public List<int> FrameColors { get; init; }

        [JsonPropertyName("bc")]
        public List<int> BackgroundColors { get; init; }

        public SpellIconData()
        {
            PrintColors = new();
            FrameColors = new();
            BackgroundColors = new();
        }
    }

    public sealed class SpellLevelData
    {
        public int Id { get; init; }

        public List<IEffect> Effects { get; init; }

        public List<IEffect> CriticalEffects { get; init; }

        public int ActionPointCost { get; init; }

        public int MinRange { get; init; }

        public int MaxRange { get; init; }

        public int CriticalHitRate { get; init; }

        public int CriticalFailureRate { get; init; }

        public bool LineOnly { get; init; }

        public bool LineOfSight { get; init; }

        public bool NeedFreeCell { get; init; }

        public bool CanBoostRange { get; init; }

        public SpellLevelCategory SpellLevelCategory { get; init; }

        public int LaunchCountByTurn { get; init; }

        public int LaunchCountByPlayerByTurn { get; init; }

        public int DelayBetweenLaunch { get; init; }

        public List<int> RequiredStatesId { get; init; }

        public List<int> ForbiddenStatesId { get; init; }

        public int NeededLevel { get; init; }

        public bool CricalFailureEndTheTurn { get; init; }

        public int SpellId { get; internal set; }

        public int Level { get; internal set; }

        public SpellLevelData()
        {
            Effects = new();
            CriticalEffects = new();
            RequiredStatesId = new();
            ForbiddenStatesId = new();
        }

        public SpellData? GetSpellData()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellDataById(SpellId);
        }

        public List<IEffect> GetTrapEffects()
        {
            foreach (IEffect effect in Effects)
            {
                if (effect is TrapSpellEffect trapSpellEffect)
                {
                    SpellData? trapSpellData = trapSpellEffect.GetSpellData();
                    if (trapSpellData is not null)
                    {
                        SpellLevelData? trapSpellLevelData = trapSpellData.GetSpellLevelData(trapSpellEffect.Level);
                        if (trapSpellLevelData is not null)
                        {
                            return trapSpellLevelData.Effects;
                        }
                    }
                }
            }

            return new();
        }

        public List<IEffect> GetGlyphEffects()
        {
            foreach (IEffect effect in Effects)
            {
                if (effect is GlyphSpellEffect glyphSpellEffect)
                {
                    SpellData? glyphSpellData = glyphSpellEffect.GetSpellData();
                    if (glyphSpellData is not null)
                    {
                        SpellLevelData? glyphSpellLevelData = glyphSpellData.GetSpellLevelData(glyphSpellEffect.Level);
                        if (glyphSpellLevelData is not null)
                        {
                            return glyphSpellLevelData.Effects;
                        }
                    }
                }
            }

            return new();
        }

        public IEnumerable<StateData> GetRequiredStatesData()
        {
            foreach (int stateId in RequiredStatesId)
            {
                StateData? stateData = DofusApi.Instance.Datacenter.StatesData.GetStateDataById(stateId);
                if (stateData is not null)
                {
                    yield return stateData;
                }
            }
        }

        public IEnumerable<StateData> GetForbiddenStatesData()
        {
            foreach (int stateId in ForbiddenStatesId)
            {
                StateData? stateData = DofusApi.Instance.Datacenter.StatesData.GetStateDataById(stateId);
                if (stateData is not null)
                {
                    yield return stateData;
                }
            }
        }
    }

    public sealed class SpellData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("i")]
        public SpellIconData Icon { get; init; }

        [JsonPropertyName("p")]
        public bool Passive { get; init; }

        [JsonPropertyName("g")]
        public bool GlobalInterval { get; init; }

        [JsonPropertyName("b")]
        public int BreedId { get; init; }

        [JsonPropertyName("t")]
        public SpellType SpellType { get; init; }

        [JsonPropertyName("o")]
        public SpellOrigin SpellOrigin { get; init; }

        [JsonPropertyName("c")]
        public SpellCategory SpellCategory { get; init; }

        [JsonPropertyName("l1")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevelData? SpellLevelData1 { get; init; }

        [JsonPropertyName("l2")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevelData? SpellLevelData2 { get; init; }

        [JsonPropertyName("l3")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevelData? SpellLevelData3 { get; init; }

        [JsonPropertyName("l4")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevelData? SpellLevelData4 { get; init; }

        [JsonPropertyName("l5")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevelData? SpellLevelData5 { get; init; }

        [JsonPropertyName("l6")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevelData? SpellLevelData6 { get; init; }

        public SpellData()
        {
            Name = string.Empty;
            Description = string.Empty;
            Icon = new();
        }

        public async Task<string> GetImagePath()
        {
            string url = $"{DofusApi.Instance.Config.CdnUrl}/images/spells/{Id}.jpg";

            if (await DofusApi.Instance.HttpClient.ExistsAsync(url))
            {
                return url;
            }

            return $"{DofusApi.Instance.Config.CdnUrl}/images/spells/unknown.png";
        }

        public BreedData? GetBreedData()
        {
            return DofusApi.Instance.Datacenter.BreedsData.GetBreedDataById(BreedId) ?? DofusApi.Instance.Datacenter.BreedsData.Breeds.Find(x => x.SpecialSpellId == Id);
        }

        public SpellLevelData? GetSpellLevelData(int level = 1)
        {
            return level switch
            {
                1 => SpellLevelData1,
                2 => SpellLevelData2,
                3 => SpellLevelData3,
                4 => SpellLevelData4,
                5 => SpellLevelData5,
                6 => SpellLevelData6,
                _ => null,
            };
        }

        public int GetMaxLevelNumber()
        {
            for (int i = 6; i > 0; i--)
            {
                if (GetSpellLevelData(i) is not null)
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetNeededLevel()
        {
            return SpellLevelData1 is null ? -1 : SpellLevelData1.NeededLevel;
        }

        public IncarnationData? GetIncarnationData()
        {
            foreach (IncarnationData incarnation in DofusApi.Instance.Datacenter.IncarnationsData.Incarnations)
            {
                if (incarnation.SpellsId.Contains(Id))
                {
                    return incarnation;
                }
            }

            return null;
        }
    }

    public sealed class SpellsData
    {
        private const string FILE_NAME = "spells.json";

        [JsonPropertyName("S")]
        public List<SpellData> Spells { get; init; }

        public SpellsData()
        {
            Spells = new();
        }

        internal static SpellsData Build()
        {
            return Json.LoadFromFile<SpellsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public SpellData? GetSpellDataById(int id)
        {
            return Spells.Find(x => x.Id == id);
        }

        public SpellData? GetSpellDataByName(string name)
        {
            return Spells.Find(x => x.Name.NormalizeCustom().Equals(name.NormalizeCustom()));
        }

        public List<SpellData> GetSpellsDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Spells.FindAll(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetSpellNameById(int id)
        {
            SpellData? spellData = GetSpellDataById(id);

            return spellData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : spellData.Name;
        }

        public SpellLevelData? GetSpellLevelDataById(int id)
        {
            foreach (SpellData spellData in Spells)
            {
                for (int i = 1; i <= 6; i++)
                {
                    SpellLevelData? spellLevelData = spellData.GetSpellLevelData(i);
                    if (spellLevelData is null)
                    {
                        break;
                    }

                    if (spellLevelData.Id == id)
                    {
                        return spellLevelData;
                    }
                }
            }

            return null;
        }
    }
}
