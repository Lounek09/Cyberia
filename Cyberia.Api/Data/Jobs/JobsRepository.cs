﻿using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Jobs;

public sealed class JobsRepository : IDofusRepository
{
    private const string c_fileName = "jobs.json";

    [JsonPropertyName("J")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, JobData>))]
    public FrozenDictionary<int, JobData> Jobs { get; init; }

    [JsonConstructor]
    internal JobsRepository()
    {
        Jobs = FrozenDictionary<int, JobData>.Empty;
    }

    internal static JobsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<JobsRepository>(filePath);
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