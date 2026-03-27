using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolBoostNoEffectEffect : ParameterlessEffect
{
    private GladiatroolBoostNoEffectEffect(int id)
        : base(id) { }

    internal static GladiatroolBoostNoEffectEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }
}
