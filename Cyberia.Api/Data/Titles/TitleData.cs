using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Titles;

public sealed class TitleData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("t")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("c")]
    public int Color { get; init; }

    [JsonPropertyName("pt")]
    public int ParametersType { get; init; }

    [JsonConstructor]
    internal TitleData()
    {
        Name = LocalizedString.Empty;
    }
}
