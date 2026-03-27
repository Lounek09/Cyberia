using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGainJobXpEffect : Effect, IJobEffect
{
    public int JobId { get; init; }
    public int XpAmount { get; init; }

    private CharacterGainJobXpEffect(int id, int jobId, int xpAmount)
        : base(id)
    {
        JobId = jobId;
        XpAmount = xpAmount;
    }

    internal static CharacterGainJobXpEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param2, (int)parameters.Param3);
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
