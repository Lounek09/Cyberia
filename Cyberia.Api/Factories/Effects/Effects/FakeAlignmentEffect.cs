using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record FakeAlignmentEffect : Effect, IEffect<FakeAlignmentEffect>
{
    public int AlignmentId { get; init; }

    private FakeAlignmentEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int alignmentId)
        : base(id, duration, probability, criteria, effectArea)
    {
        AlignmentId = alignmentId;
    }

    public static FakeAlignmentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public AlignmentData? GetAlignmentData()
    {
        return DofusApi.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
    }

    public Description GetDescription()
    {
        var alignmentName = DofusApi.Datacenter.AlignmentsData.GetAlignmentNameById(AlignmentId);

        return GetDescription(null, null, alignmentName);
    }
}
