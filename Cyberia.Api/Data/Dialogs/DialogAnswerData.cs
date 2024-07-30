using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Dialogs;

public sealed class DialogAnswerData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public LocalizedString Answer { get; init; }

    [JsonConstructor]
    internal DialogAnswerData()
    {
        Answer = LocalizedString.Empty;
    }
}
