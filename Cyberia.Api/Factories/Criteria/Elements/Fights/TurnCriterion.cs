using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Fights;

public sealed record TurnCriterion : Criterion
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

    protected override string GetDescriptionKey()
    {
        if (Operator == '%')
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

        return $"Criterion.Turn.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Value);
    }
}
