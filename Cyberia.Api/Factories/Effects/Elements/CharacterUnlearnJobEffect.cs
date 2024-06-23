using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterUnlearnJobEffect : Effect
{
    public int JobId { get; init; }

    private CharacterUnlearnJobEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int jobId)
        : base(id, duration, probability, criteria, effectArea)
    {
        JobId = jobId;
    }

    internal static CharacterUnlearnJobEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public JobData? GetJobData()
    {
        return DofusApi.Datacenter.JobsRepository.GetJobDataById(JobId);
    }

    public override Description GetDescription()
    {
        var jobName = DofusApi.Datacenter.JobsRepository.GetJobNameById(JobId);

        return GetDescription(string.Empty, string.Empty, jobName);
    }
}
