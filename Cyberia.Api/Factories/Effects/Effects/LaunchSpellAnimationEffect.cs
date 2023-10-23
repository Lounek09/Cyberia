using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LaunchSpellGfxAnimationEffect : Effect, IEffect<LaunchSpellGfxAnimationEffect>
    {
        public int GfxId { get; init; }

        private LaunchSpellGfxAnimationEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int gfxId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            GfxId = gfxId;
        }

        public static LaunchSpellGfxAnimationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public Description GetDescription()
        {
            return GetDescription(null, null, GfxId);
        }
    }
}
