using Cyberia.Api.Data.Common;
using Cyberia.Api.Data.Npcs;

using System.Drawing;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.DailyQuest;

public sealed class DailyQuestNpcData : IDofusData
{
    [JsonPropertyName("i")]
    public int NpcId { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonPropertyName("c1")]
    [JsonConverter(typeof(JsonConverters.ColorConverter))]
    public Color? Color1 { get; init; }

    [JsonPropertyName("c2")]
    [JsonConverter(typeof(JsonConverters.ColorConverter))]
    public Color? Color2 { get; init; }

    [JsonPropertyName("c3")]
    [JsonConverter(typeof(JsonConverters.ColorConverter))]
    public Color? Color3 { get; init; }

    [JsonPropertyName("a")]
    public AccessoriesData Accessories { get; init; }

    [JsonConstructor]
    internal DailyQuestNpcData()
    {
        Accessories = new();
    }

    public NpcData? GetNpcData()
    {
        return DofusApi.Datacenter.NpcsRepository.GetNpcDataById(NpcId);
    }
}
