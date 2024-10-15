using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterBoostDealtAndReceivedDamagePercentMultiplierEffect : Effect
{
    public int MinDealtDamagePercent { get; init; }
    public int MinReceivedDamagePercent { get; init; }
    public int MaxDealtDamagePercent { get; init; }
    public int MaxReceivedDamagePercent { get; init; }

    private CharacterBoostDealtAndReceivedDamagePercentMultiplierEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int minDealtDamagePercent, int minReceivedDamagePercent, int maxDealtDamagePercent, int maxReceivedDamagePercent)
        : base(id, duration, probability, criteria, effectArea)
    {
        MinDealtDamagePercent = minDealtDamagePercent;
        MinReceivedDamagePercent = minReceivedDamagePercent;
        MaxDealtDamagePercent = maxDealtDamagePercent;
        MaxReceivedDamagePercent = maxReceivedDamagePercent;
    }

    internal static CharacterBoostDealtAndReceivedDamagePercentMultiplierEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        int param1, param2, param3, param4;

        if (parameters.Param1 == 0 && parameters.Param2 == 0)
        {
            param1 = (int)parameters.Param3;
            param2 = (int)parameters.Param3 * 3 - 14;
            param3 = 0;
            param4 = 0;
        }
        else
        {
            var tempDealt = parameters.Param1 + parameters.Param3;
            var tempReceived = parameters.Param2 + parameters.Param3;

            param1 = (int)tempDealt;
            param2 = (int)tempDealt * 3 - 14;
            param3 = (int)tempReceived;
            param4 = (int)tempReceived * 3 - 14;
        }

        return new(effectId, duration, probability, criteria, effectArea, param1, param2, param3, param4);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var maxDealtDamagePercent = MaxDealtDamagePercent == 0 ? string.Empty : MaxDealtDamagePercent.ToString();
        var maxReceivedDamagePercent = MaxReceivedDamagePercent == 0 ? string.Empty : MaxReceivedDamagePercent.ToString();

        return GetDescription(culture, MinDealtDamagePercent, MinReceivedDamagePercent, maxDealtDamagePercent, maxReceivedDamagePercent);
    }
}
