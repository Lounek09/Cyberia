using Cyberia.Api.Utils;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Jobs;

public sealed class JobData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("s")]
    public int JobSpecializationId { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonConstructor]
    internal JobData()
    {
        Name = LocalizedString.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("jobs", GfxId, size);
    }

    public JobData? GetJobDataSpecialization()
    {
        return DofusApi.Datacenter.JobsRepository.GetJobDataById(JobSpecializationId);
    }
}
