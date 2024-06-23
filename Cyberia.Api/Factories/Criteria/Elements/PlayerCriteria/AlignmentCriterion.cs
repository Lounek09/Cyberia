using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.Criteria;

public sealed record AlignmentCriterion : Criterion
{
    public int AlignmentId { get; init; }

    private AlignmentCriterion(string id, char @operator, int alignmentId)
        : base(id, @operator)
    {
        AlignmentId = alignmentId;
    }

    internal static AlignmentCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var alignmentId))
        {
            return new(id, @operator, alignmentId);
        }

        return null;
    }

    public AlignmentData? GetAlignmentData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentDataById(AlignmentId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Alignment.{GetOperatorDescriptionKey()}";
    }

    public override Description GetDescription()
    {
        var alignmentName = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentNameById(AlignmentId);

        return GetDescription(alignmentName);
    }
}
