using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public interface IEffect
    {
        public int EffectId { get; init; }
        public EffectParameters Parameters { get; init; }
        public int Duration { get; init; }
        public int Probability { get; init; }
        public Area Area { get; init; }

        public Effect? GetEffect();

        public string GetDescription();
    }
}