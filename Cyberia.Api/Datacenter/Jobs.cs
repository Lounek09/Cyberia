using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Job
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("s")]
        public int SpecializationOfJobId { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        public Job()
        {
            Name = string.Empty;
        }

        public Job? GetSpecializationOf()
        {
            return DofusApi.Instance.Datacenter.JobsData.GetJobById(SpecializationOfJobId);
        }
    }

    public sealed class JobsData
    {
        private const string FILE_NAME = "jobs.json";

        [JsonPropertyName("J")]
        public List<Job> Jobs { get; init; }

        public JobsData()
        {
            Jobs = new();
        }

        internal static JobsData Build()
        {
            return Json.LoadFromFile<JobsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Job? GetJobById(int id)
        {
            return Jobs.Find(x => x.Id == id);
        }

        public string GetJobNameById(int id)
        {
            Job? job = GetJobById(id);

            return job is null ? $"Inconnu ({id})" : job.Name;
        }
    }
}
