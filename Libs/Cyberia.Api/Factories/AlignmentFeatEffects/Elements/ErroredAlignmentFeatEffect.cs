using System.Globalization;

namespace Cyberia.Api.Factories.AlignmentFeatEffects.Elements;

public sealed record ErroredAlignmentFeatEffect : AlignmentFeatEffect
{
    IReadOnlyList<int> Parameters { get; init; }

    internal ErroredAlignmentFeatEffect(int id, IReadOnlyList<int> parameter)
        : base(id)
    {
        Parameters = parameter;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return new DescriptionString(Translation.Get<ApiTranslations>("AlignmentFeatEffect.Errored", culture),
            Id.ToString(),
            string.Join(", ", Parameters));
    }
}
