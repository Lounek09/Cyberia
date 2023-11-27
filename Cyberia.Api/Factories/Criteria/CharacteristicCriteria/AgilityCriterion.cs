namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria;

public sealed record AgilityCriterion : Criterion, ICriterion<AgilityCriterion>
{
    public int Agility { get; init; }

    private AgilityCriterion(string id, char @operator, int agility)
        : base(id, @operator)
    {
        Agility = agility;
    }

    public static AgilityCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var agility))
        {
            return new(id, @operator, agility);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Agility.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Agility);
    }
}
