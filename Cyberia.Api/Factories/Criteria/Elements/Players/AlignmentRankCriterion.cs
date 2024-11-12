using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record AlignmentRankCriterion : Criterion
{
    public int Rank { get; init; }

    private AlignmentRankCriterion(string id, char @operator, int rank)
        : base(id, @operator)
    {
        Rank = rank;
    }

    internal static AlignmentRankCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var rank))
        {
            return new(id, @operator, rank);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.AlignmentRank.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Rank);
    }
}
