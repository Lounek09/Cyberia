﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public class BasicEffect : IEffect
    {
        public int EffectId { get; init; }
        public EffectParameters Parameters { get; init; }
        public int Duration { get; init; }
        public int Probability { get; init; }
        public string Criteria { get; init; }
        public Area Area { get; init; }

        public BasicEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            EffectId = effectId;
            Parameters = parameters;
            Duration = duration;
            Probability = probability;
            Criteria = criteria;
            Area = area;
        }

        public static BasicEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public Effect? GetEffect()
        {
            return DofusApi.Instance.Datacenter.EffectsData.GetEffectById(EffectId);
        }

        public virtual string GetDescription()
        {
            string? param1 = Parameters.Param1 == 0 ? null : Parameters.Param1.ToString();
            string? param2 = Parameters.Param2 == 0 ? null : Parameters.Param2.ToString();
            string? param3 = Parameters.Param3 == 0 ? null : Parameters.Param3.ToString();

            return GetDescriptionFromParameters(param1, param2, param3, Parameters.Param4);
        }

        protected string GetDescriptionFromParameters(params string?[] parameters)
        {
            Effect? effect = GetEffect();

            if (effect is not null)
            {
                string value = "";

                if (Probability > 0)
                    value += $"Dans {Probability}% des cas : ";

                string[] nonNullableParameters = new string[4] { "", "", "", "" };
                for (int i = 0; i < parameters.Length; i++)
                {
                    string? parameter = parameters[i];
                    if (!string.IsNullOrEmpty(parameter))
                        nonNullableParameters[i] = parameter.Bold();
                }

                value += PatternDecoder.DecodeDescription(effect.Description, nonNullableParameters);

                if (Duration <= -1 || Duration >= 63)
                    value += " (inf.)";
                else if (Duration != 0)
                    value += $" ({Duration} tour{(Duration > 1 ? "s" : "")})";

                return value;
            }

            return $"Effet {EffectId.ToString().Bold()} non référencé ({string.Join(", ", parameters)})";
        }
    }
}