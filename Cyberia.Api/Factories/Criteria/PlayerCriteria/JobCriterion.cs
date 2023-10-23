using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record JobCriterion : Criterion, ICriterion<JobCriterion>
    {
        public int JobId { get; init; }
        public int? Level { get; init; }

        private JobCriterion(string id, char @operator, int jobId, int? level) :
            base(id, @operator)
        {
            JobId = jobId;
            Level = level;
        }

        public static JobCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 1 && int.TryParse(parameters[0], out int jobId) && int.TryParse(parameters[1], out int level))
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
            return DofusApi.Instance.Datacenter.JobsData.GetJobDataById(JobId);
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
            string jobName = DofusApi.Instance.Datacenter.JobsData.GetJobNameById(JobId);

            if (Level.HasValue)
            {
                return GetDescription(jobName, Level.Value);
            }

            return GetDescription(jobName);
        }
    }
}
