using Cyberia.Api.Extensions;
using Cyberia.Api.Enums;

namespace Cyberia.Api.Factories.Criteria;

public sealed record GenderCriterion : Criterion
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

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Gender.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(Gender.GetDescription());
    }
}
