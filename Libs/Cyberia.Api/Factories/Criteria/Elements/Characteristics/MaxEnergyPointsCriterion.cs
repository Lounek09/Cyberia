using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Characteristics;

public sealed record MaxEnergyPointsCriterion : Criterion
{
    public int Value { get; init; }

    private MaxEnergyPointsCriterion(string id, char @operator, int value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static MaxEnergyPointsCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var value))
        {
            return new(id, @operator, value);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.MaxEnergyPoints.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Value);
    }
}
