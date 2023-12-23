using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterGainJobXpEffect : Effect, IEffect<CharacterGainJobXpEffect>
{
    public int JobId { get; init; }
    public int XpAmount { get; init; }

    private CharacterGainJobXpEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int jobId, int xpAmount)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        JobId = jobId;
        XpAmount = xpAmount;
    }

    public static CharacterGainJobXpEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param2, parameters.Param3);
    }

    public JobData? GetJobData()
    {
        return DofusApi.Datacenter.JobsData.GetJobDataById(JobId);
    }

    public Description GetDescription()
    {
        var jobName = DofusApi.Datacenter.JobsData.GetJobNameById(JobId);

        return GetDescription(XpAmount, jobName);
    }
}
