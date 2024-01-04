using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook;

public sealed class KnowledgeBookTriggerData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("t")]
    public int Type { get; init; }

    [JsonPropertyName("v")]
    [JsonInclude]
    internal JsonElement Values { get; init; } //string or int[]

    [JsonPropertyName("d")]
    public int KnowledgeBookTipId { get; init; }

    [JsonConstructor]
    internal KnowledgeBookTriggerData()
    {
        Values = new();
    }

    public KnowledgeBookTipData? GetKnowledgeBookTipData()
    {
        return DofusApi.Datacenter.KnowledgeBookData.GetKnowledgeBookTipDataById(KnowledgeBookTipId);
    }
}
