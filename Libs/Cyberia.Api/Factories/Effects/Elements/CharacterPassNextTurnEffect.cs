using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterPassNextTurnEffect : ParameterlessEffect
{
    private CharacterPassNextTurnEffect(int id)
        : base(id) { }

    internal static CharacterPassNextTurnEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }
}
