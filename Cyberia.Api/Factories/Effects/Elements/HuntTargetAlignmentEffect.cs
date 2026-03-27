using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record HuntTargetAlignmentEffect : Effect, IAlignmentEffect
{
    public int AlignmentId { get; init; }

    private HuntTargetAlignmentEffect(int id, int alignmentId)
        : base(id)
    {
        AlignmentId = alignmentId;
    }

    internal static HuntTargetAlignmentEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
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
