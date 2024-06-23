namespace Cyberia.Api.Factories.Criteria;

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

    public override Description GetDescription()
    {
        return GetDescription(Id, CompressedCriterion);
    }
}
