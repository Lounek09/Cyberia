using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LaunchSpellLevelAnimationEffect : BasicEffect
    {
        public int SpellLevelId { get; init; }

        public LaunchSpellLevelAnimationEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            SpellLevelId = parameters.Param1;
        }

        public static new LaunchSpellLevelAnimationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(null, null, SpellLevelId.ToString());
        }
    }
}
