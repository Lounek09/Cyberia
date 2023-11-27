namespace Cyberia.Api.Factories.Criteria;

public sealed record ErroredCriterion : Criterion, ICriterion<ErroredCriterion>
{
    string[] Parameters { get; init; }

    private ErroredCriterion(string id, char @operator, string[] parameters)
        : base(id, @operator)
    {
        Parameters = parameters;
    }

    public static ErroredCriterion Create(string id, char @operator, params string[] parameters)
    {
        return new(id, @operator, parameters);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Errored.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Parameters);
    }
}
