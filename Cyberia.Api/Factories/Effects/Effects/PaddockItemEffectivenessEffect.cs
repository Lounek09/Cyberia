using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record PaddockItemEffectivenessEffect : Effect, IEffect<PaddockItemEffectivenessEffect>
    {
        public int Effectiveness { get; init; }

        private PaddockItemEffectivenessEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int effectiveness) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            Effectiveness = effectiveness;
        }

        public static PaddockItemEffectivenessEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public Description GetDescription()
        {
            return GetDescription(null, null, Effectiveness);
        }
    }
}
