using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record LearnSpellLevelEffect : Effect, IEffect<LearnSpellLevelEffect>
{
    public int SpellLevelId { get; init; }

    private LearnSpellLevelEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellLevelId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        SpellLevelId = spellLevelId;
    }

    public static LearnSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public SpellLevelData? GetSpellLevelData()
    {
        return DofusApi.Datacenter.SpellsData.GetSpellLevelDataById(SpellLevelId);
    }

    public Description GetDescription()
    {
        var spellLevel = GetSpellLevelData();
        if (spellLevel is null)
        {
            return GetDescription($"{nameof(SpellLevelData)} {PatternDecoder.Description(Resources.Unknown_Data, SpellLevelId)}", 0);
        }

        var spellName = DofusApi.Datacenter.SpellsData.GetSpellNameById(spellLevel.SpellId);

        return GetDescription(spellName, spellLevel.Level);
    }
}
