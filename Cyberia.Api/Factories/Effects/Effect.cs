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

    protected Description GetDescription(params object?[] parameters)
    {
        var effectData = GetEffectData();
        if (effectData is null)
        {
            var commaSeparatedParameters = string.Join(',', parameters);

            Log.Information("Unknown {EffectData} {EffectId} ({EffectParameters})",
                nameof(EffectData),
                Id,
                commaSeparatedParameters);

            return new(Resources.Effect_Unknown, Id.ToString(), commaSeparatedParameters);
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

        return new(value, Array.ConvertAll(parameters, x => x?.ToString() ?? string.Empty));
    }
}
