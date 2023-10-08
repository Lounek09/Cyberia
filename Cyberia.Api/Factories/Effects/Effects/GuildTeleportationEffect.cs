using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record GuildTeleportationEffect : Effect, IEffect<GuildTeleportationEffect>
    {
        public GuildTeleportation GuildTeleportation { get; init; }

        private GuildTeleportationEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, GuildTeleportation guildTeleportation) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            GuildTeleportation = guildTeleportation;
        }

        public static GuildTeleportationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, (GuildTeleportation)parameters.Param3);
        }

        public Description GetDescription()
        {
            return GetDescription(null, null, GuildTeleportation.GetDescription());
        }
    }
}
