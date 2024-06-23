using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.Criteria;

public sealed record AlignmentSpecializationCriterion : Criterion
{
    public int AlignmentSpecializationId { get; init; }

    private AlignmentSpecializationCriterion(string id, char @operator, int alignmentSpecializationId)
        : base(id, @operator)
    {
        AlignmentSpecializationId = alignmentSpecializationId;
    }

    internal static AlignmentSpecializationCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var alignmentSpecializationId))
        {
            return new(id, @operator, alignmentSpecializationId);
        }

        return null;
    }

    public AlignmentSpecializationData? GetAlignmentSpecializationData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentSpecializationDataById(AlignmentSpecializationId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.AlignmentSpecialization.{GetOperatorDescriptionKey()}";
    }

    public override Description GetDescription()
    {
        var alignmentSpecializationName = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentSpecializationNameById(AlignmentSpecializationId);

        return GetDescription(alignmentSpecializationName);
    }
}
