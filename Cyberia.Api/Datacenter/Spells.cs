﻿using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class SpellIcon
    {
        [JsonPropertyName("up")]
        public int UpId { get; init; }

        [JsonPropertyName("pc")]
        public List<int> PaleColor { get; init; }

        [JsonPropertyName("b")]
        public int BackId { get; init; }

        [JsonPropertyName("fc")]
        public List<int> FlashyColor { get; init; }

        [JsonPropertyName("bc")]
        public List<int> BlackAndWhiteColor { get; init; }

        public SpellIcon()
        {
            PaleColor = new();
            FlashyColor = new();
            BlackAndWhiteColor = new();
        }
    }

    public sealed class SpellLevel
    {
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

        public int SpellLevelCategoryId { get; init; }

        public int LaunchCountByTurn { get; init; }

        public int LaunchCountByPlayerByTurn { get; init; }

        public int DelayBetweenLaunch { get; init; }

        public List<int> RequiredStatesId { get; init; }

        public List<int> ForbiddenStatesId { get; init; }

        public int NeededLevel { get; init; }

        public bool CricalFailureEndTheTurn { get; init; }

        public SpellLevel()
        {
            Effects = new();
            CriticalEffects = new();
            RequiredStatesId = new();
            ForbiddenStatesId = new();
        }

        public List<IEffect> GetTrapEffects()
        {
            foreach (IEffect effect in Effects)
            {
                if (effect is TrapSpellEffect trapSpellEffect)
                {
                    Spell? trapSpell = trapSpellEffect.GetSpell();
                    if (trapSpell is not null)
                    {
                        SpellLevel? trapSpellLevel = trapSpell.GetSpellLevel(trapSpellEffect.Level);
                        if (trapSpellLevel is not null)
                            return trapSpellLevel.Effects;
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
                    Spell? trapSpell = glyphSpellEffect.GetSpell();
                    if (trapSpell is not null)
                    {
                        SpellLevel? glyphSpellLevel = trapSpell.GetSpellLevel(glyphSpellEffect.Level);
                        if (glyphSpellLevel is not null)
                            return glyphSpellLevel.Effects;
                    }
                }
            }

            return new();
        }

        public SpellLevelCategory? GetSpellLevelCategory()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellLevelCategoryById(SpellLevelCategoryId);
        }

        public List<State> GetRequiredStates()
        {
            List<State> states = new();

            foreach (int stateId in RequiredStatesId)
            {
                State? state = DofusApi.Instance.Datacenter.StatesData.GetStateById(stateId);
                if (state is not null)
                    states.Add(state);
            }

            return states;
        }

        public List<State> GetForbiddenStates()
        {
            List<State> states = new();

            foreach (int stateId in ForbiddenStatesId)
            {
                State? state = DofusApi.Instance.Datacenter.StatesData.GetStateById(stateId);
                if (state is not null)
                    states.Add(state);
            }

            return states;
        }
    }

    public sealed class Spell
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("i")]
        public SpellIcon Icon { get; init; }

        [JsonPropertyName("p")]
        public bool Passive { get; init; }

        [JsonPropertyName("g")]
        public bool GlobalInterval { get; init; }

        [JsonPropertyName("b")]
        public int BreedId { get; init; }

        [JsonPropertyName("t")]
        public int SpellTypeId { get; init; }

        [JsonPropertyName("o")]
        public int SpellOriginId { get; init; }

        [JsonPropertyName("c")]
        public int SpellCategoryId { get; init; }

        [JsonPropertyName("l1")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevel? SpellLevel1 { get; init; }

        [JsonPropertyName("l2")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevel? SpellLevel2 { get; init; }

        [JsonPropertyName("l3")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevel? SpellLevel3 { get; init; }

        [JsonPropertyName("l4")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevel? SpellLevel4 { get; init; }

        [JsonPropertyName("l5")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevel? SpellLevel5 { get; init; }

        [JsonPropertyName("l6")]
        [JsonConverter(typeof(SpellLevelJsonConverter))]
        public SpellLevel? SpellLevel6 { get; init; }

        public Spell()
        {
            Name = string.Empty;
            Description = string.Empty;
            Icon = new();
        }

        public async Task<string> GetImgPath()
        {
            string url = $"{DofusApi.Instance.CdnUrl}/images/spells/{Id}.png";

            if (await DofusApi.Instance.HttpClient.CheckIfPageExistsAsync(url))
                return url;

            return $"{DofusApi.Instance.CdnUrl}/images/spells/unknown.png";
        }

        public Breed? GetBreed()
        {
            return DofusApi.Instance.Datacenter.BreedsData.GetBreedById(BreedId) ?? DofusApi.Instance.Datacenter.BreedsData.Breeds.Find(x => x.BreedSpellId == Id);
        }

        public SpellOrigin? GetSpellOrigin()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellOriginById(SpellOriginId);
        }

        public SpellType? GetSpellType()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellTypeById(SpellTypeId);
        }

        public SpellCategory? GetSpellCategory()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellCategoryById(SpellCategoryId);
        }

        public SpellLevel? GetSpellLevel(int level = 1)
        {
            switch (level)
            {
                case 1:
                    return SpellLevel1;
                case 2:
                    return SpellLevel2;
                case 3:
                    return SpellLevel3;
                case 4:
                    return SpellLevel4;
                case 5:
                    return SpellLevel5;
                case 6:
                    return SpellLevel6;
                default:
                    return null;
            }
        }

        public int GetMaxLevelNumber()
        {
            for (int i = 6; i > 0; i--)
            {
                if (GetSpellLevel(i) is not null)
                    return i;
            }

            return -1;
        }

        public int GetNeededLevel()
        {
            return SpellLevel1 is null ? -1 : SpellLevel1.NeededLevel;
        }

        public Incarnation? GetIncarnation()
        {
            foreach (Incarnation incarnation in DofusApi.Instance.Datacenter.IncarnationsData.Incarnations)
            {
                if (incarnation.SpellsId.Contains(Id))
                    return incarnation;
            }

            return null;
        }
    }

    public sealed class SpellsData
    {
        private const string FILE_NAME = "spells.json";

        [JsonPropertyName("S")]
        public List<Spell> Spells { get; init; }

        public List<SpellType> SpellTypes { get; private set; }

        public List<SpellOrigin> SpellOrigins { get; private set; }

        public List<SpellCategory> SpellCategories { get; private set; }

        public List<SpellLevelCategory> SpellLevelCategories { get; private set; }

        public SpellsData()
        {
            Spells = new();
            SpellTypes = new();
            SpellOrigins = new();
            SpellCategories = new();
            SpellLevelCategories = new();
        }

        internal static SpellsData Build()
        {
            SpellsData data = Json.LoadFromFile<SpellsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
            SpellsCustomData customData = Json.LoadFromFile<SpellsCustomData>($"{DofusApi.CUSTOM_PATH}/{FILE_NAME}");

            data.SpellTypes = customData.SpellTypesCustom;
            data.SpellOrigins = customData.SpellOriginsCustom;
            data.SpellCategories = customData.SpellCategoriesCustom;
            data.SpellLevelCategories = customData.SpellLevelCategoriesCustom;

            return data;
        }

        public Spell? GetSpellById(int id)
        {
            return Spells.Find(x => x.Id == id);
        }

        public Spell? GetSpellByName(string name)
        {
            return Spells.Find(x => x.Name.RemoveDiacritics().Equals(name.RemoveDiacritics()));
        }

        public List<Spell> GetSpellsByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return Spells.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public string GetSpellNameById(int id)
        {
            Spell? spell = GetSpellById(id);

            return spell is null ? $"Inconnu ({id})" : spell.Name;
        }

        public SpellType? GetSpellTypeById(int id)
        {
            return SpellTypes.Find(x => x.Id == id);
        }

        public SpellOrigin? GetSpellOriginById(int id)
        {
            return SpellOrigins.Find(x => x.Id == id);
        }

        public SpellCategory? GetSpellCategoryById(int id)
        {
            return SpellCategories.Find(x => x.Id == id);
        }

        public SpellLevelCategory? GetSpellLevelCategoryById(int id)
        {
            return SpellLevelCategories.Find(x => x.Id == id);
        }
    }
}