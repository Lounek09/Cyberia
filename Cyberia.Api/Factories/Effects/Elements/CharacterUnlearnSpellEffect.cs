using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterUnlearnSpellEffect : Effect
{
    public int SpellId { get; init; }

    private CharacterUnlearnSpellEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int spellId)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpellId = spellId;
    }

    internal static CharacterUnlearnSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellDataById(SpellId);
    }

    public override DescriptionString GetDescription()
    {
        var spellName = DofusApi.Datacenter.SpellsRepository.GetSpellNameById(SpellId);

        return GetDescription(string.Empty, string.Empty, spellName);
    }
}
