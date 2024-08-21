using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterBoostDealtAndReceivedDamagePercentMultiplierEffect : Effect
{
    private const double c_ratio = 1.5;

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
        int param1, param2, param3, param4;

        if (parameters.Param1 == 0 && parameters.Param2 == 0)
        {
            param1 = (int)parameters.Param3;
            param2 = (int)Math.Round(parameters.Param3 * c_ratio);
            param3 = 0;
            param4 = 0;
        }
        else
        {
            var tempDealt = parameters.Param1 + parameters.Param3;
            var tempReceived = parameters.Param2 + parameters.Param3;

            param1 = (int)tempDealt;
            param2 = (int)Math.Round(tempDealt * c_ratio);
            param3 = (int)tempReceived;
            param4 = (int)Math.Round(tempReceived * c_ratio);
        }

        return new(effectId, duration, probability, criteria, effectArea, param1, param3, param2, param4);
    }

    public override Description GetDescription()
    {
        var maxDealtDamagePercent = MaxDealtDamagePercent == 0 ? string.Empty : MaxDealtDamagePercent.ToString();
        var maxReceivedDamagePercent = MaxReceivedDamagePercent == 0 ? string.Empty : MaxReceivedDamagePercent.ToString();

        return GetDescription(MinDealtDamagePercent, MinReceivedDamagePercent, maxDealtDamagePercent, maxReceivedDamagePercent);
    }
}
