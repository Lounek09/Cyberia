using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterPunishmentEffect : Effect, IEffect<CharacterPunishmentEffect>
{
    public int BoostEffectId { get; init; }
    public int MaxBoost { get; init; }
    public int Turn { get; init; }

    private CharacterPunishmentEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int boostEffectId, int maxBoost, int turn)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        BoostEffectId = boostEffectId;
        MaxBoost = maxBoost;
        Turn = turn;
    }

    public static CharacterPunishmentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2, parameters.Param3);
    }

    public EffectData? GetBoostEffectData()
    {
        return DofusApi.Datacenter.EffectsData.GetEffectDataById(BoostEffectId);
    }

    public Description GetDescription()
    {
        return GetDescription(null, MaxBoost, Turn);
    }
}
