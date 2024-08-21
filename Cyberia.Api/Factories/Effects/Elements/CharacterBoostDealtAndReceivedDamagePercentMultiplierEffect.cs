using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterBoostDealtAndReceivedDamagePercentMultiplierEffect : Effect
{
    public int MinDealtDamagePercent { get; init; }
    public int MaxDealtDamagePercent { get; init; }
    public int MinReceivedDamagePercent { get; init; }
    public int MaxReceivedDamagePercent { get; init; }

    private CharacterBoostDealtAndReceivedDamagePercentMultiplierEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int minDealtDamagePercent, int maxDealtDamagePercent, int minReceivedDamagePercent, int maxReceivedDamagePercent)
        : base(id, duration, probability, criteria, effectArea)
    {
        MinDealtDamagePercent = minDealtDamagePercent;
        MaxDealtDamagePercent = maxDealtDamagePercent;
        MinReceivedDamagePercent = minReceivedDamagePercent;
        MaxReceivedDamagePercent = maxReceivedDamagePercent;
    }

    internal static CharacterBoostDealtAndReceivedDamagePercentMultiplierEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        //TODO: Param4 is not always a string now...
        var maxReceivedDamagePercent = string.IsNullOrEmpty(parameters.Param4) ? 0 : int.Parse(parameters.Param4);

        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param3, (int)parameters.Param2, maxReceivedDamagePercent);
    }

    public override Description GetDescription()
    {
        var maxDealtDamagePercent = MaxDealtDamagePercent == 0 ? string.Empty : MaxDealtDamagePercent.ToString();
        var maxReceivedDamagePercent = MaxReceivedDamagePercent == 0 ? string.Empty : MaxReceivedDamagePercent.ToString();

        return GetDescription(MinDealtDamagePercent, maxDealtDamagePercent, MaxDealtDamagePercent, maxReceivedDamagePercent);
    }
}
