using Cyberia.Api.JsonConverters;

using System.Drawing;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Titles;

public sealed class TitleData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("t")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("c")]
    [JsonConverter(typeof(JsonConverters.ColorConverter))]
    public Color? Color { get; init; }

    [JsonPropertyName("pt")]
    public int ParametersType { get; init; }

    [JsonConstructor]
    internal TitleData()
    {
        Name = LocalizedString.Empty;
    }
}
