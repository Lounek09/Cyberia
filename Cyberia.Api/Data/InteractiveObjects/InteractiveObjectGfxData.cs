using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.InteractiveObjects;

internal sealed class InteractiveObjectGfxData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int GfxId { get; init; }

    [JsonPropertyName("v")]
    public int Id { get; init; }

    [JsonConstructor]
    internal InteractiveObjectGfxData() { }
}
