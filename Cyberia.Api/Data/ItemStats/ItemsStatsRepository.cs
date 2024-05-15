using Cyberia.Api.Data.ItemStats.Custom;
using Cyberia.Api.Factories.Effects;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats;

public sealed class ItemsStatsRepository : IDofusRepository
{
    private const string c_fileName = "itemstats.json";

    [JsonPropertyName("ISTA")]
    [JsonInclude]
    internal List<ItemStatsData> ItemsStatsCore { get; init; }

    [JsonIgnore]
    public FrozenDictionary<int, ItemStatsData> ItemsStats { get; internal set; }

    [JsonConstructor]
    internal ItemsStatsRepository()
    {
        ItemsStatsCore = [];
        ItemsStats = FrozenDictionary<int, ItemStatsData>.Empty;
    }

    internal static ItemsStatsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);
        var customFilePath = Path.Combine(DofusApi.CustomPath, c_fileName);

        var data = Datacenter.LoadRepository<ItemsStatsRepository>(filePath);
        var customData = Datacenter.LoadRepository<ItemsStatsCustomRepository>(customFilePath);

        foreach (var itemStatsCustomData in customData.ItemsStatsCustom)
        {
            var itemStatsData = data.ItemsStatsCore.Find(x => x.Id == itemStatsCustomData.Id);
            if (itemStatsData is null)
            {
                data.ItemsStatsCore.Add(new ItemStatsData()
                {
                    Id = itemStatsCustomData.Id,
                    Effects = itemStatsCustomData.Effects
                });

                continue;
            }

            ((List<IEffect>)itemStatsData.Effects).AddRange(itemStatsCustomData.Effects);
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
