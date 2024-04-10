namespace Cyberia.Api.Factories.Criteria;

public sealed record PlayerRightsCriterion : Criterion, ICriterion
{
    public int RightsLevel { get; init; }

    private PlayerRightsCriterion(string id, char @operator, int rightsLevel)
        : base(id, @operator)
    {
        RightsLevel = rightsLevel;
    }

    internal static PlayerRightsCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var rightsLevel))
        {
            return new(id, @operator, rightsLevel);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.PlayerRights.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(RightsLevel);
    }
}
