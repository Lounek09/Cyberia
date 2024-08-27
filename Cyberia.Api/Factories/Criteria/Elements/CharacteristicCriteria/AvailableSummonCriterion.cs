namespace Cyberia.Api.Factories.Criteria;

public sealed record AvailableSummonCriterion : Criterion
{
    public int Value { get; init; }

    private AvailableSummonCriterion(string id, char @operator, int value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static AvailableSummonCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var value))
        {
            return new(id, @operator, value);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.AvailableSummon.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(Value);
    }
}
