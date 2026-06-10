using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterDivorceWifeOrHusbandEffect : ParameterlessEffect
{
    private CharacterDivorceWifeOrHusbandEffect(int id)
        : base(id) { }

    internal static CharacterDivorceWifeOrHusbandEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }
}
