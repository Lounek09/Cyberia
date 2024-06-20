using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Text;

namespace Cyberia.Api.Factories.Effects;

public abstract record Effect(int Id, int Duration, int Probability, CriteriaReadOnlyCollection Criteria, EffectArea EffectArea)
{
    public EffectData? GetEffectData()
    {
        return DofusApi.Datacenter.EffectsRepository.GetEffectDataById(Id);
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
            return new(ApiTranslations.Effect_Unknown,
                Id.ToString(),
                string.Join(',', parameters));
        }

        StringBuilder builder = new();

        if (Probability > 0)
        {
            builder.Append(Translation.Format(ApiTranslations.Effect_Probability, Probability));
            builder.Append(" : ");
        }

        builder.Append(effectData.Description);

        if (Duration <= -1 || Duration >= 63)
        {
            builder.Append(" (");
            builder.Append(ApiTranslations.Infinity);
            builder.Append(')');
        }
        else if (Duration != 0)
        {
            builder.Append(" (");
            builder.Append(Translation.Format(ApiTranslations.Effect_Turn, Duration));
            builder.Append(')');
        }

        return new(builder.ToString(), parameters);
    }
}
