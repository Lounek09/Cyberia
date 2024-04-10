namespace Cyberia.Api.Factories.Criteria;

public sealed record UnusableItemCriterion : Criterion, ICriterion
{
    public string Value { get; init; }

    private UnusableItemCriterion(string id, char @operator, string value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static UnusableItemCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0]);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.UnusableItem.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return base.GetDescription();
    }
}
