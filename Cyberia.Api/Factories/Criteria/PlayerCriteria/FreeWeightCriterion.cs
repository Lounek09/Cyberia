namespace Cyberia.Api.Factories.Criteria.PlayerCriteria;

public sealed record FreeWeightCriterion : Criterion, ICriterion<FreeWeightCriterion>
{
    public int Weight { get; init; }

    private FreeWeightCriterion(string id, char @operator, int weight)
        : base(id, @operator)
    {
        Weight = weight;
    }

    public static FreeWeightCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var weight))
        {
            return new(id, @operator, weight);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.FreeWeight.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Weight);
    }
}
