using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterUnlearnJobEffect : Effect, IEffect<CharacterUnlearnJobEffect>
{
    public int JobId { get; init; }

    private CharacterUnlearnJobEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int jobId)
        : base(id, duration, probability, criteria, effectArea)
    {
        JobId = jobId;
    }

    public static CharacterUnlearnJobEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public JobData? GetJobData()
    {
        return DofusApi.Datacenter.JobsData.GetJobDataById(JobId);
    }

    public Description GetDescription()
    {
        var jobName = DofusApi.Datacenter.JobsData.GetJobNameById(JobId);

        return GetDescription(null, null, jobName);
    }
}
