using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLearnSpellLevelEffect : Effect, IEffect<CharacterLearnSpellLevelEffect>
{
    public int SpellLevelId { get; init; }

    private CharacterLearnSpellLevelEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellLevelId)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpellLevelId = spellLevelId;
    }

    public static CharacterLearnSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public SpellLevelData? GetSpellLevelData()
    {
        return DofusApi.Datacenter.SpellsData.GetSpellLevelDataById(SpellLevelId);
    }

    public Description GetDescription()
    {
        var spellLevelData = GetSpellLevelData();
        if (spellLevelData is null)
        {
            return GetDescription($"{nameof(SpellLevelData)} {PatternDecoder.Description(Resources.Unknown_Data, SpellLevelId)}", 0);
        }

        return GetDescription(spellLevelData.SpellData.Name, spellLevelData.Rank);
    }
}
