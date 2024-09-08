using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Dialogs.Localized;

internal sealed class DialogAnswerLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Message { get; init; }

    [JsonConstructor]
    internal DialogAnswerLocalizedData()
    {
        Message = string.Empty;
    }
}
