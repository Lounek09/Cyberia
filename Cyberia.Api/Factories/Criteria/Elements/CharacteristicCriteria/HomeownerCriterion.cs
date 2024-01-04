namespace Cyberia.Api.Factories.Criteria;

public sealed record HomeownerCriterion
    : Criterion, ICriterion
{
    public bool IsHomeowner { get; init; }

    private HomeownerCriterion(string id, char @operator, bool isHomeowner)
        : base(id, @operator)
    {
        IsHomeowner = isHomeowner;
    }

    internal static HomeownerCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0].Equals("1"));
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Homeowner.{GetOperatorDescriptionName()}.{IsHomeowner}";
    }

    public Description GetDescription()
    {
        return GetDescription(IsHomeowner);
    }
}
