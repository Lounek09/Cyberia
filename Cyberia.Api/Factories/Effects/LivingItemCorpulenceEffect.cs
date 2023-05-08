using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class LivingItemCorpulenceEffect : BasicEffect
    {
        public string Corpulence { get; init; }

        public LivingItemCorpulenceEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            Corpulence = parameters.Param3 == 0 ? "Maigrichon" : parameters.Param3 == 1 ? "Rassasié" : "Obèse";
        }

        public static new LivingItemCorpulenceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(null, null, Corpulence);
        }
    }
}
