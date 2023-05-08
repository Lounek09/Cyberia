namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class JobCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int jobId))
            {
                string jobName = DofusApi.Instance.Datacenter.JobsData.GetJobNameById(jobId);

                if (values.Length > 1)
                    return $"Niveau du métier {jobName.Bold()} {@operator} {values[1].Bold()}";

                return $"Métier {@operator} {jobName.Bold()}";
            }

            return null;
        }
    }
}
