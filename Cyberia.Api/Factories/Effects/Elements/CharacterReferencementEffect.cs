using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterReferencementEffect
    : Effect, IEffect
{
    public int JobId { get; init; }

    private CharacterReferencementEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int jobId)
        : base(id, duration, probability, criteria, effectArea)
    {
        JobId = jobId;
    }

    internal static CharacterReferencementEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1);
    }

    public JobData? GetJobData()
    {
        return DofusApi.Datacenter.JobsData.GetJobDataById(JobId);
    }

    public Description GetDescription()
    {
        var jobName = DofusApi.Datacenter.JobsData.GetJobNameById(JobId);

        return GetDescription(jobName);
    }
}
