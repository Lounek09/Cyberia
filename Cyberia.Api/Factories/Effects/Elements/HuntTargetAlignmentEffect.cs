using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record HuntTargetAlignmentEffect : Effect
{
    public int AlignmentId { get; init; }

    private HuntTargetAlignmentEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int alignmentId)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        AlignmentId = alignmentId;
    }

    internal static HuntTargetAlignmentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
    }

    public AlignmentData? GetAlignmentData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentDataById(AlignmentId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var alignmentName = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentNameById(AlignmentId, culture);

        return GetDescription(culture, string.Empty, string.Empty, alignmentName);
    }
}
