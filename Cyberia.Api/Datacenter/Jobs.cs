using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class JobData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("s")]
        public int SpecializationOfJobId { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        public JobData()
        {
            Name = string.Empty;
        }

        public JobData? GetJobDataSpecialization()
        {
            return DofusApi.Instance.Datacenter.JobsData.GetJobDataById(SpecializationOfJobId);
        }
    }

    public sealed class JobsData
    {
        private const string FILE_NAME = "jobs.json";

        [JsonPropertyName("J")]
        public List<JobData> Jobs { get; init; }

        public JobsData()
        {
            Jobs = new();
        }

        internal static JobsData Build()
        {
            return Json.LoadFromFile<JobsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public JobData? GetJobDataById(int id)
        {
            return Jobs.Find(x => x.Id == id);
        }

        public string GetJobNameById(int id)
        {
            JobData? jobData = GetJobDataById(id);

            return jobData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : jobData.Name;
        }
    }
}
