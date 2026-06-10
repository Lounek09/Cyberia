using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements;

public sealed record UntranslatedCriterion : Criterion
{
    public IReadOnlyList<string> Parameters { get; init; }
    public string CompressedCriterion { get; init; }

    internal UntranslatedCriterion(string id, char @operator, IReadOnlyList<string> parameters, string compressedCriterion)
        : base(id, @operator)
    {
        Parameters = parameters;
        CompressedCriterion = compressedCriterion;
    }

    protected override string GetDescriptionKey()
    {
        return "Criterion.Unknown";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Id, CompressedCriterion);
    }
}
