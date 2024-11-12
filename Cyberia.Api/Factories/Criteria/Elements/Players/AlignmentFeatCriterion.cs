using Cyberia.Api.Data.Alignments;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record AlignmentFeatCriterion : Criterion
{
    public int AlignmentFeatId { get; init; }
    public int? Level { get; init; }

    private AlignmentFeatCriterion(string id, char @operator, int alignmentFeatId, int? level)
        : base(id, @operator)
    {
        AlignmentFeatId = alignmentFeatId;
        Level = level;
    }

    internal static AlignmentFeatCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 1 && int.TryParse(parameters[0], out var alignmentFeatId) && int.TryParse(parameters[1], out var level))
        {
            return new(id, @operator, alignmentFeatId, level);
        }

        if (parameters.Length > 0 && int.TryParse(parameters[0], out alignmentFeatId))
        {
            return new(id, @operator, alignmentFeatId, null);
        }

        return null;
    }

    public AlignmentFeatData? GetAlignmentFeatData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentFeatDataById(AlignmentFeatId);
    }

    protected override string GetDescriptionKey()
    {
        if (Level.HasValue)
        {
            return $"Criterion.AlignmentFeat.{GetOperatorDescriptionKey()}.Level";
        }

        return $"Criterion.AlignmentFeat.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var alignmentName = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentFeatNameById(AlignmentFeatId, culture);

        if (Level.HasValue)
        {
            return GetDescription(culture, alignmentName, Level.Value);
        }

        return GetDescription(culture, alignmentName);
    }
}
