using System.Globalization;

namespace Cyberia.Api.Factories.AlignmentFeatEffects.Elements;

public sealed record UntranslatedAlignmentFeatEffect : AlignmentFeatEffect
{
    public IReadOnlyList<int> Parameters { get; init; }

    internal UntranslatedAlignmentFeatEffect(int id, IReadOnlyList<int> parameters)
        : base(id)
    {
        Parameters = parameters;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Parameters.ToArray());
    }
}
