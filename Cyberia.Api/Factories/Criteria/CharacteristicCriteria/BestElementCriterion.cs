using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria;

public sealed record BestElementCriterion : Criterion, ICriterion<BestElementCriterion>
{
    public Element Element { get; init; }

    private BestElementCriterion(string id, char @operator, Element element)
        : base(id, @operator)
    {
        Element = element;
    }

    public static BestElementCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && Enum.TryParse(parameters[0], out Element element))
        {
            return new(id, @operator, element);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.BestElement.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Element.GetDescription());
    }
}
