using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class JobData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("s")]
    public int JobSpecializationId { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonConstructor]
    internal JobData()
    {
        Name = string.Empty;
    }

    public JobData? GetJobDataSpecialization()
    {
        return DofusApi.Datacenter.JobsData.GetJobDataById(JobSpecializationId);
    }
}

public sealed class JobsData : IDofusData
{
    private const string FILE_NAME = "jobs.json";

    [JsonPropertyName("J")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, JobData>))]
    public FrozenDictionary<int, JobData> Jobs { get; init; }

    [JsonConstructor]
    internal JobsData()
    {
        Jobs = FrozenDictionary<int, JobData>.Empty;
    }

    internal static JobsData Load()
    {
        return Datacenter.LoadDataFromFile<JobsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public JobData? GetJobDataById(int id)
    {
        Jobs.TryGetValue(id, out var jobData);
        return jobData;
    }

    public string GetJobNameById(int id)
    {
        var jobData = GetJobDataById(id);

        return jobData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : jobData.Name;
    }
}
