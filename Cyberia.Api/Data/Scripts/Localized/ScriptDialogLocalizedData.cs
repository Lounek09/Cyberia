using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Scripts.Localized;

internal sealed class ScriptDialogLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Message { get; init; }

    [JsonConstructor]
    internal ScriptDialogLocalizedData()
    {
        Message = string.Empty;
    }
}
