using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record MountRideEffect : ParameterlessEffect
{
    private MountRideEffect(int id)
        : base(id) { }

    internal static MountRideEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }
}
