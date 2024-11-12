using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record AlignmentLevelCriterion : Criterion
{
    public int Level { get; init; }

    private AlignmentLevelCriterion(string id, char @operator, int level)
        : base(id, @operator)
    {
        Level = level;
    }

    internal static AlignmentLevelCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var level))
        {
            return new(id, @operator, level);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.AlignmentLevel.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Level);
    }
}
