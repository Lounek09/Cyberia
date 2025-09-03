using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterAlignmentSideSetEffect : Effect
{
    public int AlignmentId { get; init; }

    public int AlignmentSpecializationId { get; init; }

    public int Level { get; init; }

    private CharacterAlignmentSideSetEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int alignmentId, int alignmentSpecializationId, int level)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        AlignmentId = alignmentId;
        AlignmentSpecializationId = alignmentSpecializationId;
        Level = level;
    }

    internal static CharacterAlignmentSideSetEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public AlignmentData? GetAlignmentData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentDataById(AlignmentId);
    }

    public AlignmentSpecializationData? GetAlignmentSpecializationData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentSpecializationDataById(AlignmentSpecializationId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var alignmentName = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentNameById(AlignmentId, culture);
        var alignmentSpecializationName = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentSpecializationNameById(AlignmentSpecializationId, culture);

        return GetDescription(culture, alignmentName, alignmentSpecializationName, Level);
    }
}
