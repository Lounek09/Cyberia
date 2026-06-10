using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Characteristics;

public sealed record HonorPointCriterion : Criterion
{
    public int Value { get; init; }

    private HonorPointCriterion(string id, char @operator, int value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static HonorPointCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var value))
        {
            return new(id, @operator, value);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.HonorPoint.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Value);
    }
}
