﻿using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolIncreaseSpellLevelEffect : Effect
{
    public int SpellId { get; init; }
    public int LevelIncrease { get; init; }

    private GladiatroolIncreaseSpellLevelEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int spellId, int levelIncrease)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpellId = spellId;
        LevelIncrease = levelIncrease;
    }

    internal static GladiatroolIncreaseSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3, (int)parameters.Param2);
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellDataById(SpellId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var spellName = DofusApi.Datacenter.SpellsRepository.GetSpellNameById(SpellId, culture);

        return GetDescription(culture, string.Empty, LevelIncrease, spellName);
    }
}
