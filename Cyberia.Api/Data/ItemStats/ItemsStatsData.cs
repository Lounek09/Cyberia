using Cyberia.Api.Data.ItemStats.Custom;
using Cyberia.Api.Factories.Effects;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats;

public sealed class ItemsStatsData : IDofusData
{
    private const string c_fileName = "itemstats.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);
    private static readonly string s_customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

    [JsonPropertyName("ISTA")]
    [JsonInclude]
    internal List<ItemStatsData> ItemsStatsCore { get; init; }

    [JsonIgnore]
    public FrozenDictionary<int, ItemStatsData> ItemsStats { get; internal set; }

    [JsonConstructor]
    internal ItemsStatsData()
    {
        ItemsStatsCore = [];
        ItemsStats = FrozenDictionary<int, ItemStatsData>.Empty;
    }

    internal static async Task<ItemsStatsData> LoadAsync()
    {
        var data = await Datacenter.LoadDataAsync<ItemsStatsData>(s_filePath);
        var customData = await Datacenter.LoadDataAsync<ItemsStatsCustomData>(s_customFilePath);

        foreach (var itemStatsCustomData in customData.ItemsStatsCustom)
        {
            var itemStatsData = data.ItemsStatsCore.Find(x => x.Id == itemStatsCustomData.Id);
            if (itemStatsData is not null)
            {
                ((List<IEffect>)itemStatsData.Effects).AddRange(itemStatsCustomData.Effects);
                continue;
            }

            data.ItemsStatsCore.Add(itemStatsCustomData.ToItemStatsData());
        }

        data.ItemsStats = data.ItemsStatsCore.GroupBy(x => x.Id).ToFrozenDictionary(x => x.Key, x => x.ElementAt(0));
        return data;
    }

    public ItemStatsData? GetItemStatDataById(int id)
    {
        ItemsStats.TryGetValue(id, out var itemStatsData);
        return itemStatsData;
    }
}
