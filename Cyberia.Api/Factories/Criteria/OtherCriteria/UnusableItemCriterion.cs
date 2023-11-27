namespace Cyberia.Api.Factories.Criteria.OtherCriteria;

public sealed record UnusableItemCriterion : Criterion, ICriterion<UnusableItemCriterion>
{
    public string Value { get; init; }

    private UnusableItemCriterion(string id, char @operator, string value)
        : base(id, @operator)
    {
        Value = value;
    }

    public static UnusableItemCriterion? Create(string id, char @operator, params string[] parameters)
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
