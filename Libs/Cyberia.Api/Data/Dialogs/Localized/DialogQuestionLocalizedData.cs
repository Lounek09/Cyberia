using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Dialogs.Localized;

internal sealed class DialogQuestionLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Message { get; init; }

    [JsonConstructor]
    internal DialogQuestionLocalizedData()
    {
        Message = string.Empty;
    }
}
