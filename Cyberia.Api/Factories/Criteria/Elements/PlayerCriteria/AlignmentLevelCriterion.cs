namespace Cyberia.Api.Factories.Criteria;

public sealed record AlignmentLevelCriterion
    : Criterion, ICriterion
{
    public int Level { get; init; }

    private AlignmentLevelCriterion(string id, char @operator, int level)
        : base(id, @operator)
    {
        Level = level;
    }

    internal static AlignmentLevelCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var level))
        {
            return new(id, @operator, level);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.AlignmentLevel.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Level);
    }
}
