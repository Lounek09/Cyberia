using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGainJobXpEffect : Effect
{
    public int JobId { get; init; }
    public int XpAmount { get; init; }

    private CharacterGainJobXpEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int jobId, int xpAmount)
        : base(id, duration, probability, criteria, effectArea)
    {
        JobId = jobId;
        XpAmount = xpAmount;
    }

    internal static CharacterGainJobXpEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param2, (int)parameters.Param3);
    }

    public JobData? GetJobData()
    {
        return DofusApi.Datacenter.JobsRepository.GetJobDataById(JobId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var jobName = DofusApi.Datacenter.JobsRepository.GetJobNameById(JobId, culture);

        return GetDescription(culture, XpAmount, jobName);
    }
}
