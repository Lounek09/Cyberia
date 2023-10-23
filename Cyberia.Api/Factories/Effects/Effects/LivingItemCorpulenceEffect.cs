using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LivingItemCorpulenceEffect : Effect, IEffect<LivingItemCorpulenceEffect>
    {
        public Corpulence Corpulence { get; init; }

        private LivingItemCorpulenceEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, Corpulence corpulence) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            Corpulence = corpulence;
        }

        public static LivingItemCorpulenceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, (Corpulence)parameters.Param3);
        }

        public Description GetDescription()
        {
            return GetDescription(null, null, Corpulence.GetDescription());
        }
    }
}
