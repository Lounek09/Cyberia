using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

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
        return DofusApi.Datacenter.SpellsRepository.GetSpellDataById(SpellId);
    }

    public override DescriptionString GetDescription()
    {
        var spellName = DofusApi.Datacenter.SpellsRepository.GetSpellNameById(SpellId);

        return GetDescription(spellName, Level);
    }
}
