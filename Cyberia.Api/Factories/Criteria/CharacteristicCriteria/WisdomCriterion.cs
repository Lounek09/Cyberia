namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria;

public sealed record WisdomCriterion : Criterion, ICriterion<WisdomCriterion>
{
    public int Wisdom { get; init; }

    private WisdomCriterion(string id, char @operator, int wisdom)
        : base(id, @operator)
    {
        Wisdom = wisdom;
    }

    public static WisdomCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var wisdom))
        {
            return new(id, @operator, wisdom);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Wisdom.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Wisdom);
    }
}
