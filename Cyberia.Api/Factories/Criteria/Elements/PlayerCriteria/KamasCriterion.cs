namespace Cyberia.Api.Factories.Criteria;

public sealed record KamasCriterion : Criterion
{
    public int Value { get; init; }

    private KamasCriterion(string id, char @operator, int value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static KamasCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var value))
        {
            return new(id, @operator, value);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Kamas.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(Value);
    }
}
