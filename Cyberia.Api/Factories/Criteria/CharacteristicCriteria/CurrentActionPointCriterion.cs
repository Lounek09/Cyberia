namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria;

public sealed record CurrentActionPointCriterion : Criterion, ICriterion<CurrentActionPointCriterion>
{
    public int ActionPoint { get; init; }

    private CurrentActionPointCriterion(string id, char @operator, int actionPoint)
        : base(id, @operator)
    {
        ActionPoint = actionPoint;
    }

    public static CurrentActionPointCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var actionPoint))
        {
            return new(id, @operator, actionPoint);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.CurrentActionPoint.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(ActionPoint);
    }
}
