using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public interface IEffect
    {
        int EffectId { get; init; }
        EffectParameters Parameters { get; init; }
        int Duration { get; init; }
        int Probability { get; init; }
        string Criteria { get; init; }
        Area Area { get; init; }

        Effect? GetEffect();

        string GetDescription();
    }
}