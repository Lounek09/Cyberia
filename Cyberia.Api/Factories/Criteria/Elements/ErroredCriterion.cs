using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements;

public sealed record ErroredCriterion : Criterion
{
    public IReadOnlyList<string> Parameters { get; init; }
    public string CompressedCriterion { get; init; }

    internal ErroredCriterion(string compressedCriterion)
        : base(string.Empty, char.MinValue)
    {
        Parameters = [];
        CompressedCriterion = compressedCriterion;
    }

    internal ErroredCriterion(string id, char @operator, IReadOnlyList<string> parameters, string compressedCriterion)
        : base(id, @operator)
    {
        Parameters = parameters;
        CompressedCriterion = compressedCriterion;
    }

    protected override string GetDescriptionKey()
    {
        return "Criterion.Errored";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, CompressedCriterion);
    }
}
