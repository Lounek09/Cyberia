using Cyberia.Api.Data.Jobs;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record JobCriterion : Criterion
{
    public int JobId { get; init; }
    public int Level { get; init; }

    private JobCriterion(string id, char @operator, int jobId, int level)
        : base(id, @operator)
    {
        JobId = jobId;
        Level = level;
    }

    internal static JobCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 1 && int.TryParse(parameters[0], out var jobId) && int.TryParse(parameters[1], out var level))
        {
            return new(id, @operator, jobId, level);
        }

        if (parameters.Length > 0 && int.TryParse(parameters[0], out jobId))
        {
            return new(id, @operator, jobId, 0);
        }

        return null;
    }

    public JobData? GetJobData()
    {
        return DofusApi.Datacenter.JobsRepository.GetJobDataById(JobId);
    }

    protected override string GetDescriptionKey()
    {
        if (Level > 0)
        {
            return $"Criterion.Job.{GetOperatorDescriptionKey()}.Level";
        }

        return $"Criterion.Job.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var jobName = DofusApi.Datacenter.JobsRepository.GetJobNameById(JobId, culture);

        return GetDescription(culture, jobName, Level);
    }
}
