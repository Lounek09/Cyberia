using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Characteristics;

public sealed record BestElementCriterion : Criterion
{
    public Element Element { get; init; }

    private BestElementCriterion(string id, char @operator, Element element)
        : base(id, @operator)
    {
        Element = element;
    }

    internal static BestElementCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && Enum.TryParse(parameters[0], out Element element))
        {
            return new(id, @operator, element);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.BestElement.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Element.GetDescription(culture));
    }
}
