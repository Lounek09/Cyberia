﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public abstract record Effect(int EffectId, int Duration, int Probability, List<ICriteriaElement> Criteria, EffectArea EffectArea)
    {
        public EffectData? GetEffectData()
        {
            return DofusApi.Instance.Datacenter.EffectsData.GetEffectDataById(EffectId);
        }

        protected Description GetDescription(params object?[] parameters)
        {
            EffectData? effect = GetEffectData();
            if (effect is not null)
            {
                string value = effect.Description;

                if (Probability > 0)
                    value = $"{PatternDecoder.Description(Resources.Effect_Probability, Probability)} : " + value;

                if (Duration <= -1 || Duration >= 63)
                    value += $" ({Resources.Infinity})";
                else if (Duration != 0)
                    value += $" ({PatternDecoder.Description(Resources.Effect_Turn, Duration)})";

                return new(value, Array.ConvertAll(parameters, x =>
                {
                    //Workaround before the translation of ALL effects
                    if (x is int i && i == 0)
                        return string.Empty;

                    return x?.ToString() ?? string.Empty;
                }));
            }

            Log.Information("Unknown {name} {effectId} (parameters)", nameof(EffectData), EffectId, string.Join(", ", parameters));
            return new(Resources.Effect_Unknown, EffectId.ToString(), string.Join(", ", parameters));
        }
    }
}