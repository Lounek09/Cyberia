namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria;

public sealed record StrengthCriterion : Criterion, ICriterion<StrengthCriterion>
{
    public int Strength { get; init; }

    private StrengthCriterion(string id, char @operator, int strength)
        : base(id, @operator)
    {
        Strength = strength;
    }

    public static StrengthCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var strength))
        {
            return new(id, @operator, strength);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Strength.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Strength);
    }
}
