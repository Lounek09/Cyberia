using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.Criteria;

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

    internal static AlignmentFeatCriterion? Create(string id, char @operator, params string[] parameters)
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

    public override Description GetDescription()
    {
        var alignmentName = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentFeatNameById(AlignmentFeatId);

        if (Level.HasValue)
        {
            return GetDescription(alignmentName, Level.Value);
        }

        return GetDescription(alignmentName);
    }
}
