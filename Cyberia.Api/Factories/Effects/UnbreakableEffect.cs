using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record UnbreakableEffect : BasicEffect
    {
        public UnbreakableEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {

        }

        public static new UnbreakableEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters();
        }
    }
}
