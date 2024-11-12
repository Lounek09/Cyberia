using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Others;

public sealed record UnusableItemCriterion : Criterion
{
    public string Value { get; init; }

    private UnusableItemCriterion(string id, char @operator, string value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static UnusableItemCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0]);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.UnusableItem.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, []);
    }
}
