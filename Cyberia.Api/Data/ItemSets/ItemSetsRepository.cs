using Cyberia.Api.Data.ItemSets.Custom;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets;

public sealed class ItemSetsRepository : IDofusRepository
{
    private const string c_fileName = "itemsets.json";

    [JsonPropertyName("IS")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSetData>))]
    public FrozenDictionary<int, ItemSetData> ItemSets { get; init; }

    [JsonConstructor]
    internal ItemSetsRepository()
    {
        ItemSets = FrozenDictionary<int, ItemSetData>.Empty;
    }

    internal static ItemSetsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);
        var customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

        var data = Datacenter.LoadRepository<ItemSetsRepository>(filePath);
        var customData = Datacenter.LoadRepository<ItemSetsCustomRepository>(customFilePath);

        foreach (var itemSetCustomData in customData.ItemSetsCustom)
        {
            var itemSetData = data.GetItemSetDataById(itemSetCustomData.Id);
            if (itemSetData is not null)
            {
                itemSetData.Effects = itemSetCustomData.Effects;
            }
        }

        return data;
    }

    public ItemSetData? GetItemSetDataById(int id)
    {
        ItemSets.TryGetValue(id, out var itemSetData);
        return itemSetData;
    }

    public IEnumerable<ItemSetData> GetItemSetsDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return ItemSets.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetItemSetNameById(int id)
    {
        var itemSetData = GetItemSetDataById(id);

        return itemSetData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : itemSetData.Name;
    }
}
