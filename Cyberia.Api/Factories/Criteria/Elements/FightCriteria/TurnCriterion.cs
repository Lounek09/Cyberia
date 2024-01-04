namespace Cyberia.Api.Factories.Criteria;

public sealed record TurnCriterion
    : Criterion, ICriterion
{
    public string Value { get; init; }

    private TurnCriterion(string id, char @operator, string value)
        : base(id, @operator)
    {
        Value = value;
    }
    
    internal static TurnCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0]);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        if (Operator is '%')
        {
            if (Value.Equals("2:0"))
            {
                return $"Criterion.Turn.Even";
            }
            if (Value.Equals("2:1"))
            {
                return $"Criterion.Turn.Odd";
            }
        }

        return $"Criterion.Turn.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Value);
    }
}
