using Cyberia.Api.Data.Jobs;

namespace Cyberia.Api.Factories.Criteria;

public sealed record JobCriterion
    : Criterion, ICriterion
{
    public int JobId { get; init; }
    public int? Level { get; init; }

    private JobCriterion(string id, char @operator, int jobId, int? level)
        : base(id, @operator)
    {
        JobId = jobId;
        Level = level;
    }

    internal static JobCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 1 && int.TryParse(parameters[0], out var jobId) && int.TryParse(parameters[1], out var level))
        {
            return new(id, @operator, jobId, level);
        }

        if (parameters.Length > 0 && int.TryParse(parameters[0], out jobId))
        {
            return new(id, @operator, jobId, null);
        }

        return null;
    }

    public JobData? GetJobData()
    {
        return DofusApi.Datacenter.JobsData.GetJobDataById(JobId);
    }

    protected override string GetDescriptionName()
    {
        if (Level.HasValue)
        {
            return $"Criterion.Job.{GetOperatorDescriptionName()}.Level";
        }

        return $"Criterion.Job.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var jobName = DofusApi.Datacenter.JobsData.GetJobNameById(JobId);

        if (Level.HasValue)
        {
            return GetDescription(jobName, Level.Value);
        }

        return GetDescription(jobName);
    }
}
