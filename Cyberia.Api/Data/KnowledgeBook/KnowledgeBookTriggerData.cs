using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook;

public sealed class KnowledgeBookTriggerData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("t")]
    public int Type { get; init; }

    [JsonPropertyName("v")]
    [JsonInclude]
    //TODO: JsonConverter for Values in KnowledgeBookTrigger
    internal object Values { get; init; }

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
