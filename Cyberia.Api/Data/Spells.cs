using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;
using Cyberia.Api.Values;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class SpellIconData : IDofusData
    {
        public const int INDEX_REMASTERED = 0;
        public const int INDEX_CONTRAST = 1;
        public const int INDEX_CLASSIC_ANGELIC = 2;
        public const int INDEX_CLASSIC_DIABOLIC = 3;

        [JsonPropertyName("up")]
        public int UpGfxId { get; init; }

        [JsonPropertyName("pc")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> PrintColors { get; init; }

        [JsonPropertyName("b")]
        public int BackgroundGfxId { get; init; }

        [JsonPropertyName("fc")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> FrameColors { get; init; }

        [JsonPropertyName("bc")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> BackgroundColors { get; init; }

        [JsonConstructor]
        internal SpellIconData()
        {
            PrintColors = ReadOnlyCollection<int>.Empty;
            FrameColors = ReadOnlyCollection<int>.Empty;
            BackgroundColors = ReadOnlyCollection<int>.Empty;
        }
    }

    public sealed class SpellLevelData : IDofusData<int>
    {
        public int Id { get; init; }

        public ReadOnlyCollection<IEffect> Effects { get; init; }

        public ReadOnlyCollection<IEffect> CriticalEffects { get; init; }

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

        public ReadOnlyCollection<int> RequiredStatesId { get; init; }

        public ReadOnlyCollection<int> ForbiddenStatesId { get; init; }

        public int NeededLevel { get; init; }

        public bool CricalFailureEndTheTurn { get; init; }

        public int SpellId { get; internal set; }

        public int Level { get; internal set; }

        internal SpellLevelData()
        {
            Effects = ReadOnlyCollection<IEffect>.Empty;
            CriticalEffects = ReadOnlyCollection<IEffect>.Empty;
            RequiredStatesId = ReadOnlyCollection<int>.Empty;
            ForbiddenStatesId = ReadOnlyCollection<int>.Empty;
        }

        public SpellData? GetSpellData()
        {
            return DofusApi.Datacenter.SpellsData.GetSpellDataById(SpellId);
        }

        public ReadOnlyCollection<IEffect> GetTrapEffects()
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

            return ReadOnlyCollection<IEffect>.Empty;
        }

        public ReadOnlyCollection<IEffect> GetGlyphEffects()
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

            return ReadOnlyCollection<IEffect>.Empty;
        }

        public IEnumerable<StateData> GetRequiredStatesData()
        {
            foreach (int stateId in RequiredStatesId)
            {
                StateData? stateData = DofusApi.Datacenter.StatesData.GetStateDataById(stateId);
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
                StateData? stateData = DofusApi.Datacenter.StatesData.GetStateDataById(stateId);
                if (stateData is not null)
                {
                    yield return stateData;
                }
            }
        }
    }

    public sealed class SpellData : IDofusData<int>
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
        [JsonConverter(typeof(SpellLevelConverter))]
        public SpellLevelData? SpellLevelData1 { get; init; }

        [JsonPropertyName("l2")]
        [JsonConverter(typeof(SpellLevelConverter))]
        public SpellLevelData? SpellLevelData2 { get; init; }

        [JsonPropertyName("l3")]
        [JsonConverter(typeof(SpellLevelConverter))]
        public SpellLevelData? SpellLevelData3 { get; init; }

        [JsonPropertyName("l4")]
        [JsonConverter(typeof(SpellLevelConverter))]
        public SpellLevelData? SpellLevelData4 { get; init; }

        [JsonPropertyName("l5")]
        [JsonConverter(typeof(SpellLevelConverter))]
        public SpellLevelData? SpellLevelData5 { get; init; }

        [JsonPropertyName("l6")]
        [JsonConverter(typeof(SpellLevelConverter))]
        public SpellLevelData? SpellLevelData6 { get; init; }

        [JsonConstructor]
        internal SpellData()
        {
            Name = string.Empty;
            Description = string.Empty;
            Icon = new();
        }

        public async Task<string> GetImagePath()
        {
            string url = $"{DofusApi.Config.CdnUrl}/images/spells/{Id}.jpg";

            if (await DofusApi.HttpClient.ExistsAsync(url))
            {
                return url;
            }

            return $"{DofusApi.Config.CdnUrl}/images/spells/unknown.png";
        }

        public BreedData? GetBreedData()
        {
            return DofusApi.Datacenter.BreedsData.GetBreedDataById(BreedId) ??
                DofusApi.Datacenter.BreedsData.Breeds.Values.FirstOrDefault(x => x.SpecialSpellId == Id);
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
            foreach (IncarnationData incarnation in DofusApi.Datacenter.IncarnationsData.Incarnations.Values)
            {
                if (incarnation.SpellsId.Contains(Id))
                {
                    return incarnation;
                }
            }

            return null;
        }
    }

    public sealed class SpellsData : IDofusData
    {
        private const string FILE_NAME = "spells.json";

        [JsonPropertyName("S")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SpellData>))]
        public FrozenDictionary<int, SpellData> Spells { get; init; }

        [JsonConstructor]
        internal SpellsData()
        {
            Spells = FrozenDictionary<int, SpellData>.Empty;
        }

        internal static SpellsData Load()
        {
            return Datacenter.LoadDataFromFile<SpellsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public SpellData? GetSpellDataById(int id)
        {
            Spells.TryGetValue(id, out SpellData? spellData);
            return spellData;
        }

        public IEnumerable<SpellData> GetSpellsDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Spells.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetSpellNameById(int id)
        {
            SpellData? spellData = GetSpellDataById(id);

            return spellData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : spellData.Name;
        }

        public SpellLevelData? GetSpellLevelDataById(int id)
        {
            foreach (SpellData spellData in Spells.Values)
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
