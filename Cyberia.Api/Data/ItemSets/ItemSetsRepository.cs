using Cyberia.Api.Data.ItemSets.Custom;
using Cyberia.Api.Data.ItemSets.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets;

public sealed class ItemSetsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "itemsets.json";

    [JsonPropertyName("IS")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSetData>))]
    public FrozenDictionary<int, ItemSetData> ItemSets { get; init; }

    [JsonConstructor]
    internal ItemSetsRepository()
    {
        ItemSets = FrozenDictionary<int, ItemSetData>.Empty;
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
                return ExtendString.NormalizeToAscii(x.Name).Contains(y, StringComparison.OrdinalIgnoreCase);
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

    protected override void LoadCustomData()
    {
        var customRepository = DofusCustomRepository.Load<ItemSetsCustomRepository>();

        foreach (var itemSetCustomData in customRepository.ItemSetsCustom)
        {
            var itemSetData = GetItemSetDataById(itemSetCustomData.Id);
            if (itemSetData is not null)
            {
                itemSetData.Effects = itemSetCustomData.Effects;
            }
        }
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToCultureInfo().TwoLetterISOLanguageName;
        var localizedRepository = DofusLocalizedRepository.Load<ItemSetsLocalizedRepository>(type, language);

        foreach (var itemSetLocalizedData in localizedRepository.ItemSets)
        {
            var itemSetData = GetItemSetDataById(itemSetLocalizedData.Id);
            itemSetData?.Name.Add(twoLetterISOLanguageName, itemSetLocalizedData.Name);
        }
    }
}
