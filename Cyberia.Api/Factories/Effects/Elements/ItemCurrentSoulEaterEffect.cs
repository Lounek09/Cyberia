using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemCurrentSoulEaterEffect : ParameterlessEffect
{
    private ItemCurrentSoulEaterEffect(int id)
        : base(id) { }

    internal static ItemCurrentSoulEaterEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }
}
