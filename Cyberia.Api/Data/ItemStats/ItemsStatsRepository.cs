using Cyberia.Api.Data.ItemStats.Custom;
using Cyberia.Api.Factories.Effects;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats;

public sealed class ItemsStatsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "itemstats.json";

    [JsonPropertyName("ISTA")]
    [JsonInclude]
    private List<ItemStatsData> ItemsStatsCore { get; init; }

    [JsonIgnore]
    public FrozenDictionary<int, ItemStatsData> ItemsStats { get; internal set; }

    [JsonConstructor]
    internal ItemsStatsRepository()
    {
        ItemsStatsCore = [];
        ItemsStats = FrozenDictionary<int, ItemStatsData>.Empty;
    }

    public ItemStatsData? GetItemStatDataById(int id)
    {
        ItemsStats.TryGetValue(id, out var itemStatsData);
        return itemStatsData;
    }

    protected override void LoadCustomData()
    {
        var customRepository = DofusCustomRepository.Load<ItemsStatsCustomRepository>();

        foreach (var itemStatsCustomData in customRepository.ItemsStatsCustom)
        {
            var itemStatsData = ItemsStatsCore.Find(x => x.Id == itemStatsCustomData.Id);
            if (itemStatsData is null)
            {
                ItemsStatsCore.Add(new ItemStatsData()
                {
                    Id = itemStatsCustomData.Id,
                    Effects = itemStatsCustomData.Effects
                });

                continue;
            }

            ((List<IEffect>)itemStatsData.Effects).AddRange(itemStatsCustomData.Effects);
        }

        ItemsStats = ItemsStatsCore.GroupBy(x => x.Id).ToFrozenDictionary(x => x.Key, x => x.First());
    }
}
