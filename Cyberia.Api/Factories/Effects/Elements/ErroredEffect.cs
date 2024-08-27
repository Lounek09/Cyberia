namespace Cyberia.Api.Factories.Effects;

public sealed record ErroredEffect : Effect
{
    public string CompressedEffect { get; init; }

    internal ErroredEffect(string compressedEffect)
        : base(0, 0, 0, [], EffectAreaFactory.Default)
    {
        CompressedEffect = compressedEffect;
    }

    public override DescriptionString GetDescription()
    {
        return new(ApiTranslations.Effect_Errored, CompressedEffect);
    }
}
