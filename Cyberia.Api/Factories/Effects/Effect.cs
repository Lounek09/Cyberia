using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public abstract record Effect(int Id, int Duration, int Probability, CriteriaCollection Criteria, EffectArea EffectArea)
{
    public EffectData? GetEffectData()
    {
        return DofusApi.Datacenter.EffectsData.GetEffectDataById(Id);
    }

    protected Description GetDescription<T>(T parameter)
    {
        return GetDescription(parameter?.ToString() ?? string.Empty);
    }

    protected Description GetDescription<T0, T1>(T0 parameter0, T1 parameter1)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    protected Description GetDescription<T0, T1, T2>(T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    protected Description GetDescription<T0, T1, T2, T3>(T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty,
            parameter3?.ToString() ?? string.Empty);
    }

    protected Description GetDescription(params string[] parameters)
    {
        var effectData = GetEffectData();
        if (effectData is null)
        {
            Log.Information("Unknown EffectData {@Effect}", this);
            return new(Resources.Effect_Unknown,
                Id.ToString(),
                string.Join(',', parameters));
        }

        var value = effectData.Description;

        if (Probability > 0)
        {
            value = $"{PatternDecoder.Description(Resources.Effect_Probability, Probability)} : " + value;
        }

        if (Duration <= -1 || Duration >= 63)
        {
            value += $" ({Resources.Infinity})";
        }
        else if (Duration != 0)
        {
            value += $" ({PatternDecoder.Description(Resources.Effect_Turn, Duration)})";
        }

        return new(value, parameters);
    }
}
