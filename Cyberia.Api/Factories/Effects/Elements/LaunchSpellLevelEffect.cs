using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record LaunchSpellLevelEffect : Effect, IEffect
{
    public int SpellLevelId { get; init; }

    private LaunchSpellLevelEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int spellLevelId)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpellLevelId = spellLevelId;
    }

    internal static LaunchSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public SpellLevelData? GetSpellLevelData()
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellLevelDataById(SpellLevelId);
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
