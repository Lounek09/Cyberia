using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LaunchSpellGfxAnimationEffect : BasicEffect
    {
        public int GfxId { get; init; }

        public LaunchSpellGfxAnimationEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            GfxId = parameters.Param3;
        }

        public static new LaunchSpellGfxAnimationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(null, null, GfxId.ToString());
        }
    }
}
