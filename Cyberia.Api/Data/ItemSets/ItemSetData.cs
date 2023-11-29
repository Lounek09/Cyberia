using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets;

public sealed class ItemSetData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("i")]
    public IReadOnlyList<int> ItemsId { get; init; }

    [JsonIgnore]
    public IReadOnlyList<IReadOnlyList<IEffect>> Effects { get; internal set; }

    [JsonConstructor]
    internal ItemSetData()
    {
        Name = string.Empty;
        ItemsId = [];
        Effects = [];
    }

    public IEnumerable<ItemData> GetItemsData()
    {
        foreach (var id in ItemsId)
        {
            var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(id);
            if (itemData is not null)
            {
                yield return itemData;
            }
        }
    }

    public int GetLevel()
    {
        var itemsData = GetItemsData();
        if (itemsData.Any())
        {
            return itemsData.Max(x => x.Level);
        }

        return 1;
    }

    public IEnumerable<IEffect> GetEffects(int nbItem)
    {
        var index = nbItem - 2;

        return Effects.Count > index && index >= 0 ? Effects[index] : [];
    }

    public BreedData? GetBreedData()
    {
        return DofusApi.Datacenter.BreedsData.Breeds.Values.FirstOrDefault(x => x.ItemSetId == Id);
    }
}
