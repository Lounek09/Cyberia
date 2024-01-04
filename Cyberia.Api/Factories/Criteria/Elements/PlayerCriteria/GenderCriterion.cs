using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Criteria;

public sealed record GenderCriterion
    : Criterion, ICriterion
{
    public Gender Gender { get; init; }

    private GenderCriterion(string id, char @operator, Gender gender)
        : base(id, @operator)
    {
        Gender = gender;
    }

    internal static GenderCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && Enum.TryParse(parameters[0], out Gender gender))
        {
            return new(id, @operator, gender);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Gender.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Gender.GetDescription());
    }
}
