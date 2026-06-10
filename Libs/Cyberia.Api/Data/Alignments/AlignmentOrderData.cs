using Cyberia.Api.Utils;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentOrderData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("a")]
    public int AlignmentId { get; init; }

    [JsonConstructor]
    internal AlignmentOrderData()
    {
        Name = LocalizedString.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("alignments/orders", Id, size);
    }

    public AlignmentData? GetAlignementData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentDataById(AlignmentId);
    }
}
