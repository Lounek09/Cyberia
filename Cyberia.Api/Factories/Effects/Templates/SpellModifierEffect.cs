using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record SpellModifierEffect : Effect
{
    public int SpellId { get; init; }
    public int Value { get; init; }

    protected SpellModifierEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int value)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        SpellId = spellId;
        Value = value;
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsData.GetSpellDataById(SpellId);
    }

    public Description GetDescription()
    {
        var spellName = DofusApi.Datacenter.SpellsData.GetSpellNameById(SpellId);

        return GetDescription(spellName, null, Value);
    }
}
