using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record PetCorpulenceEffect : Effect, IEffect<PetCorpulenceEffect>
    {
        public Corpulence Corpulence { get; init; }

        private PetCorpulenceEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, Corpulence corpulence) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            Corpulence = corpulence;
        }

        public static PetCorpulenceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            Corpulence corpulence = parameters.Param2 <= 6 ? parameters.Param3 <= 6 ? Corpulence.Satisfied : Corpulence.Skinny : Corpulence.Obese;

            return new(effectId, duration, probability, criteria, effectArea, corpulence);
        }

        public Description GetDescription()
        {
            return GetDescription(Corpulence.GetDescription());
        }
    }
}
