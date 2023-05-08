using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class PaddockItemEffectivenessEffect : BasicEffect
    {
        public int Effectiveness { get; init; }

        public PaddockItemEffectivenessEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) :
            base(effectId, parameters, duration, probability, area)
        {
            Effectiveness = parameters.Param3;
        }

        public static new PaddockItemEffectivenessEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(null, null, Effectiveness.ToString());
        }
    }
}
