using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record LaunchSpellLevelEffect : Effect, IEffect<LaunchSpellLevelEffect>
{
    public int SpellLevelId { get; init; }

    private LaunchSpellLevelEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellLevelId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        SpellLevelId = spellLevelId;
    }

    public static LaunchSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
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
