using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class PetCorpulenceEffect : BasicEffect
    {
        public string Corpulence { get; init; }

        public PetCorpulenceEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            Corpulence = parameters.Param2 <= 6 ? parameters.Param3 <= 6 ? "Normal" : "Maigrichon" : "Obèse";
        }

        public static new PetCorpulenceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(Corpulence);
        }
    }
}
