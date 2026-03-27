using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterUnlearnJobEffect : Effect, IJobEffect
{
    public int JobId { get; init; }

    private CharacterUnlearnJobEffect(int id, int jobId)
        : base(id)
    {
        JobId = jobId;
    }

    internal static CharacterUnlearnJobEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public JobData? GetJobData()
    {
        return DofusApi.Datacenter.JobsRepository.GetJobDataById(JobId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var jobName = DofusApi.Datacenter.JobsRepository.GetJobNameById(JobId, culture);

        return GetDescription(culture, string.Empty, string.Empty, jobName);
    }
}
