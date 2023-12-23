using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record LaunchSpellEffect : Effect, IEffect<LaunchSpellEffect>
{
    public int SpellId { get; init; }
    public int Rank { get; init; }

    private LaunchSpellEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int rank)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpellId = spellId;
        Rank = rank;
    }

    public static LaunchSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsData.GetSpellDataById(SpellId);
    }

    public Description GetDescription()
    {
        var spellName = DofusApi.Datacenter.SpellsData.GetSpellNameById(SpellId);

        return GetDescription(spellName, Rank);
    }
}
