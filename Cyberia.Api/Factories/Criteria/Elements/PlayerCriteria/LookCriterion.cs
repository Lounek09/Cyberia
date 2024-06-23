namespace Cyberia.Api.Factories.Criteria;

public sealed record LookCriterion : Criterion
{
    public string LookId { get; init; }

    private LookCriterion(string id, char @operator, string lookId)
        : base(id, @operator)
    {
        LookId = lookId;
    }

    internal static LookCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0]);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Look.{GetOperatorDescriptionKey()}";
    }

    public override Description GetDescription()
    {
        return GetDescription(LookId);
    }
}
