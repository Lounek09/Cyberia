using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Scripts;

public sealed class ScriptDialogData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Message { get; init; }

    [JsonConstructor]
    internal ScriptDialogData()
    {
        Message = string.Empty;
    }
}
