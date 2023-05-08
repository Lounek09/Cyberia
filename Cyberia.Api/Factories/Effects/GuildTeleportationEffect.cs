using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class GuildTeleportationEffect : BasicEffect
    {
        public string Where { get; init; }

        public GuildTeleportationEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : base(effectId, parameters, duration, probability, area)
        {
            Where = parameters.Param3 == 1 ? "téléporte dans une maison de guilde" : parameters.Param3 == 2 ? "téléporte devant un enclos de guilde" : parameters.Param3.ToString();
        }

        public static new GuildTeleportationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(null, null, Where);
        }
    }
}
