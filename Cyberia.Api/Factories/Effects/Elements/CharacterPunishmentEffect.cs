using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterPunishmentEffect : Effect, IEffect
{
    public int BoostEffectId { get; init; }
    public int MaxBoost { get; init; }
    public int Turn { get; init; }

    private CharacterPunishmentEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int boostEffectId, int maxBoost, int turn)
        : base(id, duration, probability, criteria, effectArea)
    {
        BoostEffectId = boostEffectId;
        MaxBoost = maxBoost;
        Turn = turn;
    }

    internal static CharacterPunishmentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public EffectData? GetBoostEffectData()
    {
        return DofusApi.Datacenter.EffectsRepository.GetEffectDataById(BoostEffectId);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, MaxBoost, Turn);
    }
}
