using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LaunchSpellLevelEffect : BasicEffect
    {
        public int SpellLevelId { get; init; }

        public LaunchSpellLevelEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            SpellLevelId = parameters.Param3;
        }

        public static new LaunchSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(null, null, SpellLevelId.ToString());
        }
    }
}
