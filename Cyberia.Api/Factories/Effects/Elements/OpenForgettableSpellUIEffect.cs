using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record OpenForgettableSpellUIEffect : ParameterlessEffect
{
    private OpenForgettableSpellUIEffect(int id)
        : base(id) { }

    internal static OpenForgettableSpellUIEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId);
    }
}
