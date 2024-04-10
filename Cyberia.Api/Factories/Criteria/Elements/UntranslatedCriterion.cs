namespace Cyberia.Api.Factories.Criteria;

public sealed record UntranslatedCriterion : Criterion, ICriterion
{
    IReadOnlyList<string> Parameters { get; init; }

    private UntranslatedCriterion(string id, char @operator, IReadOnlyList<string> parameters)
        : base(id, @operator)
    {
        Parameters = parameters;
    }

    internal static UntranslatedCriterion Create(string id, char @operator, params string[] parameters)
    {
        return new(id, @operator, parameters);
    }

    protected override string GetDescriptionName()
    {
        return "Criterion.Unknown";
    }

    public Description GetDescription()
    {
        return GetDescription(Parameters.ToArray());
    }
}
