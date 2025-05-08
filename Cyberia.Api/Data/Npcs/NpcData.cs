using Cyberia.Api.Utils;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs;

public sealed class NpcData : IDofusData<int>
{
    public const double RetailRatio = 0.1;

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("a")]
    public IReadOnlyList<int> NpcActionsId { get; init; }

    [JsonConstructor]
    internal NpcData()
    {
        Name = LocalizedString.Empty;
        NpcActionsId = [];
    }

    public async Task<string> GetBigImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/big", Id, size);
    }

    public async Task<string> GetFaceImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/faces", Id, size);
    }

    public async Task<string> GetMiniImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/mini", Id, size);
    }

    public IEnumerable<NpcActionData> GetNpcActionsData()
    {
        foreach (var npcActionId in NpcActionsId)
        {
            var npcActionData = DofusApi.Datacenter.NpcsRepository.GetNpcActionDataById(npcActionId);
            if (npcActionData is not null)
            {
                yield return npcActionData;
            }
        }
    }
}
