namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria;

public sealed record PercentVitalityCriterion : Criterion, ICriterion<PercentVitalityCriterion>
{
    public int PercentVitality { get; init; }

    private PercentVitalityCriterion(string id, char @operator, int percentVitality)
        : base(id, @operator)
    {
        PercentVitality = percentVitality;
    }

    public static PercentVitalityCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var percentVitality))
        {
            return new(id, @operator, percentVitality);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.PercentVitality.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(PercentVitality);
    }
}
