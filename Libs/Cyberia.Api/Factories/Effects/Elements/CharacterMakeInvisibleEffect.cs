using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterMakeInvisibleEffect : ParameterlessEffect
{
    private CharacterMakeInvisibleEffect(int id)
        : base(id) { }

    internal static CharacterMakeInvisibleEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }
}
