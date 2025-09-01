using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Characteristics;

public sealed record BaseVitalityCriterion : Criterion
{
    public int Value { get; init; }

    public bool IsScroll { get; init; }

    private BaseVitalityCriterion(string id, char @operator, int value, bool isScroll)
        : base(id, @operator)
    {
        Value = value;
        IsScroll = isScroll;
    }

    internal static BaseVitalityCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 1 && int.TryParse(parameters[0], out var value))
        {
            return new(id, @operator, value, parameters[1].Equals("scroll", StringComparison.OrdinalIgnoreCase));
        }

        if (parameters.Length > 0 && int.TryParse(parameters[0], out value))
        {
            return new(id, @operator, value, false);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.BaseVitality.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Value);
    }
}
