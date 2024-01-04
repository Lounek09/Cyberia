using Cyberia.Api.Values;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG;

public sealed class TTGEntityData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("i")]
    public int Index { get; init; }

    [JsonPropertyName("a")]
    public TTGCardRarity Rarity { get; init; }

    [JsonPropertyName("f")]
    public int TTGFamilyId { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal TTGEntityData()
    {
        Name = string.Empty;
    }

    public TTGFamilyData? GetTTGFamilyData()
    {
        return DofusApi.Datacenter.TTGData.GetTTGFamilyDataById(TTGFamilyId);
    }
}
