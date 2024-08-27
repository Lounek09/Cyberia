namespace Cyberia.Api.Factories.Criteria;

public sealed record BaseIntelligenceCriterion : Criterion
{
    public int Value { get; init; }

    private BaseIntelligenceCriterion(string id, char @operator, int value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static BaseIntelligenceCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var value))
        {
            return new(id, @operator, value);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.BaseIntelligence.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(Value);
    }
}
