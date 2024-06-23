namespace Cyberia.Api.Factories.Criteria;

public sealed record ChanceCriterion : Criterion
{
    public int Value { get; init; }

    private ChanceCriterion(string id, char @operator, int value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static ChanceCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var value))
        {
            return new(id, @operator, value);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Chance.{GetOperatorDescriptionKey()}";
    }

    public override Description GetDescription()
    {
        return GetDescription(Value);
    }
}
