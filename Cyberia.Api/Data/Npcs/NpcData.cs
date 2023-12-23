using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs;

public sealed class NpcData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("a")]
    public IReadOnlyList<int> NpcActionsId { get; init; }

    [JsonConstructor]
    internal NpcData()
    {
        Name = string.Empty;
        NpcActionsId = [];
    }

    public IEnumerable<NpcActionData> GetNpcActionsData()
    {
        foreach (var npcActionId in NpcActionsId)
        {
            var npcActionData = DofusApi.Datacenter.NpcsData.GetNpcActionDataById(npcActionId);
            if (npcActionData is not null)
            {
                yield return npcActionData;
            }
        }
    }
}
