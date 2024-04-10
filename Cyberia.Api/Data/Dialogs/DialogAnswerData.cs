using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Dialogs;

public sealed class DialogAnswerData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Answer { get; init; }

    [JsonConstructor]
    internal DialogAnswerData()
    {
        Answer = string.Empty;
    }
}
