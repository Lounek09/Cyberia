using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record PlayerRightsCriterion : Criterion
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

    protected override string GetDescriptionKey()
    {
        return $"Criterion.PlayerRights.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, RightsLevel);
    }
}
