﻿using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Jobs;

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