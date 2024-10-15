using Cyberia.Api.Data.Alignments;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

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

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var alignmentName = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentNameById(AlignmentId, culture);

        return GetDescription(culture, alignmentName);
    }
}
