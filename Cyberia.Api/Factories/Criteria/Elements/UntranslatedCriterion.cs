namespace Cyberia.Api.Factories.Criteria;

public sealed record UntranslatedCriterion : Criterion, ICriterion
{
    public IReadOnlyList<string> Parameters { get; init; }
    public string CompressedCriterion { get; init; }

    private UntranslatedCriterion(string id, char @operator, IReadOnlyList<string> parameters, string compressedCriterion)
        : base(id, @operator)
    {
        Parameters = parameters;
        CompressedCriterion = compressedCriterion;
    }

    internal static UntranslatedCriterion Create(string id, char @operator, string compressedCriterion, params string[] parameters)
    {
        return new(id, @operator, parameters, compressedCriterion);
    }

    protected override string GetDescriptionName()
    {
        return "Criterion.Unknown";
    }

    public Description GetDescription()
    {
        return GetDescription(Id, CompressedCriterion);
    }
}
