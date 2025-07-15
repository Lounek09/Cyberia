using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Characteristics;

public sealed record HomeownerCriterion : Criterion
{
    public bool IsHomeowner { get; init; }

    private HomeownerCriterion(string id, char @operator, bool isHomeowner)
        : base(id, @operator)
    {
        IsHomeowner = isHomeowner;
    }

    internal static HomeownerCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0].Equals("1"));
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Homeowner.{GetOperatorDescriptionKey()}.{IsHomeowner}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Array.Empty<string>());
    }
}
