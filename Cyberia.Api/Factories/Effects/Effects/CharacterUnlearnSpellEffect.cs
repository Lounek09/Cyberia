using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterUnlearnSpellEffect : Effect, IEffect<CharacterUnlearnSpellEffect>
{
    public int SpellId { get; init; }

    private CharacterUnlearnSpellEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        SpellId = spellId;
    }

    public static CharacterUnlearnSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
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
