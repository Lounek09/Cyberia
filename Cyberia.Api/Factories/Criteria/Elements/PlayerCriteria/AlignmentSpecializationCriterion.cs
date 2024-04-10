using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.Criteria;

public sealed record AlignmentSpecializationCriterion : Criterion, ICriterion
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
        return DofusApi.Datacenter.AlignmentsData.GetAlignmentSpecializationDataById(AlignmentSpecializationId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.AlignmentSpecialization.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var alignmentSpecializationName = DofusApi.Datacenter.AlignmentsData.GetAlignmentSpecializationNameById(AlignmentSpecializationId);

        return GetDescription(alignmentSpecializationName);
    }
}
