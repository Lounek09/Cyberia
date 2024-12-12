using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ErroredEffect : Effect
{
    public string CompressedEffect { get; init; }

    internal ErroredEffect(string compressedEffect)
        : base(0, 0, 0, [], false, EffectAreaFactory.Default)
    {
        CompressedEffect = compressedEffect;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return new DescriptionString(Translation.Get<ApiTranslations>("Effect.Errored", culture), CompressedEffect);
    }
}
