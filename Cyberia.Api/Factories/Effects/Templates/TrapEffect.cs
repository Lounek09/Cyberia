using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record TrapEffect : Effect
{
    public int SpellId { get; init; }
    public int Level { get; init; }

    protected TrapEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int spellId, int level)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpellId = spellId;
        Level = level;
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsData.GetSpellDataById(SpellId);
    }

    public Description GetDescription()
    {
        var spellName = DofusApi.Datacenter.SpellsData.GetSpellNameById(SpellId);

        return GetDescription(spellName, Level);
    }
}
