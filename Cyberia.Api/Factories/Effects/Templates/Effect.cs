using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record Effect
{
    public int EffectId { get; init; }
    public int Duration { get; init; }
    public int Probability { get; init; }
    public CriteriaCollection Criteria { get; init; }
    public EffectArea EffectArea { get; init; }

    protected Effect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        EffectId = effectId;
        Duration = duration;
        Probability = probability;
        Criteria = criteria;
        EffectArea = effectArea;
    }

    public EffectData? GetEffectData()
    {
        return DofusApi.Datacenter.EffectsData.GetEffectDataById(EffectId);
    }

    protected Description GetDescription(params object?[] parameters)
    {
        var effect = GetEffectData();
        if (effect is not null)
        {
            var value = effect.Description;

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

        Log.Information("Unknown {EffectData} {EffectId} ({EffectParameters})",
            nameof(EffectData),
            EffectId,
            string.Join(", ", parameters));

        return new(Resources.Effect_Unknown, EffectId.ToString(), string.Join(", ", parameters));
    }
}
