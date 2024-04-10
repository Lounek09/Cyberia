using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ErroredEffect : Effect, IEffect
{
    public string CompressedEffect { get; init; }

    private ErroredEffect(string compressedEffect)
        : base(0, 0, 0, [], EffectArea.Default)
    {
        CompressedEffect = compressedEffect;
    }

    internal static ErroredEffect Create(string compressedEffect)
    {
        return new(compressedEffect);
    }

    public Description GetDescription()
    {
        return new(Resources.Effect_Errored, CompressedEffect);
    }
}
