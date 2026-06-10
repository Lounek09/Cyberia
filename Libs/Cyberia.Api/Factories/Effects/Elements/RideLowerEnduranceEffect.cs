using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record RideLowerEnduranceEffect : ParameterlessEffect
{
    private RideLowerEnduranceEffect(int id)
        : base(id) { }

    internal static RideLowerEnduranceEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }
}
