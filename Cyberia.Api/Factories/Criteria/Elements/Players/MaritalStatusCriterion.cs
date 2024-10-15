using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record MaritalStatusCriterion : Criterion
{
    public MaritalStatus MaritalStatus { get; init; }

    private MaritalStatusCriterion(string id, char @operator, MaritalStatus maritalStatus)
        : base(id, @operator)
    {
        MaritalStatus = maritalStatus;
    }

    internal static MaritalStatusCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && Enum.TryParse(parameters[0], out MaritalStatus maritalStatus))
        {
            return new(id, @operator, maritalStatus);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.MaritalStatus.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, MaritalStatus.GetDescription(culture));
    }
}
