﻿using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterUnlearnGuildSpellEffect : Effect, IEffect<CharacterUnlearnGuildSpellEffect>
{
    public int SpellId { get; init; }

    private CharacterUnlearnGuildSpellEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpellId = spellId;
    }

    public static CharacterUnlearnGuildSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsData.GetSpellDataById(SpellId);
    }

    public Description GetDescription()
    {
        var spellName = DofusApi.Datacenter.SpellsData.GetSpellNameById(SpellId);

        return GetDescription(null, null, spellName);
    }
}