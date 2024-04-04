using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Jobs;

public sealed class JobsData
    : IDofusData
{
    private const string FILE_NAME = "jobs.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("J")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, JobData>))]
    public FrozenDictionary<int, JobData> Jobs { get; init; }

    [JsonConstructor]
    internal JobsData()
    {
        Jobs = FrozenDictionary<int, JobData>.Empty;
    }

    internal static async Task<JobsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<JobsData>(FILE_PATH);
    }

    public JobData? GetJobDataById(int id)
    {
        Jobs.TryGetValue(id, out var jobData);
        return jobData;
    }

    public string GetJobNameById(int id)
    {
        var jobData = GetJobDataById(id);

        return jobData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : jobData.Name;
    }
}
