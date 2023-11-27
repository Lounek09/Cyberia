namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria;

public sealed record HomeownerCriterion : Criterion, ICriterion<HomeownerCriterion>
{
    public bool Homeowner { get; init; }

    private HomeownerCriterion(string id, char @operator, bool homeowner)
        : base(id, @operator)
    {
        Homeowner = homeowner;
    }

    public static HomeownerCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0].Equals("1"));
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Homeowner.{GetOperatorDescriptionName()}.{Homeowner}";
    }

    public Description GetDescription()
    {
        return GetDescription(Homeowner);
    }
}
