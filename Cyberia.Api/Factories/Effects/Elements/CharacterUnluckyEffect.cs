using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterUnluckyEffect : ParameterlessEffect
{
    private CharacterUnluckyEffect(int id)
        : base(id) { }

    internal static CharacterUnluckyEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }
}
