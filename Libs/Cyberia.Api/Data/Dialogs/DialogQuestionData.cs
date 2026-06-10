using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Dialogs;

public sealed class DialogQuestionData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public LocalizedString Message { get; init; }

    [JsonConstructor]
    internal DialogQuestionData()
    {
        Message = LocalizedString.Empty;
    }
}
