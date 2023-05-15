using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class PaddockItemEffectivenessEffect : BasicEffect
    {
        public int Effectiveness { get; init; }

        public PaddockItemEffectivenessEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            Effectiveness = parameters.Param3;
        }

        public static new PaddockItemEffectivenessEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(null, null, Effectiveness.ToString());
        }
    }
}
