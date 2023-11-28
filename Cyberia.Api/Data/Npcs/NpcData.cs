using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs;

public sealed class NpcData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("a")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> NpcActionsId { get; init; }

    [JsonConstructor]
    internal NpcData()
    {
        Name = string.Empty;
        NpcActionsId = ReadOnlyCollection<int>.Empty;
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
