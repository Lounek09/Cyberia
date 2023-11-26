using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class CraftData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, int>))]
        public ReadOnlyDictionary<int, int> Ingredients { get; init; }

        [JsonConstructor]
        internal CraftData()
        {
            Ingredients = ReadOnlyDictionary<int, int>.Empty;
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Datacenter.ItemsData.GetItemDataById(Id);
        }

        public bool HasSubCraft()
        {
            return Ingredients.Any(x => DofusApi.Datacenter.CraftsData.GetCraftDataById(x.Key) is not null);
        }

        public ReadOnlyDictionary<ItemData, int> GetIngredients(int qte)
        {
            Dictionary<ItemData, int> ingredients = [];

            foreach (KeyValuePair<int, int> ingredient in Ingredients)
            {
                ItemData? itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(ingredient.Key);
                if (itemData is not null)
                {
                    ingredients.Add(itemData, ingredient.Value * qte);
                }
            }

            return ingredients.AsReadOnly();
        }

        public ReadOnlyDictionary<ItemData, int> GetIngredientsWithSubCraft(int qte)
        {
            Dictionary<ItemData, int> ingredients = [];

            foreach (KeyValuePair<ItemData, int> ingredient in GetIngredients(qte))
            {
                CraftData? subCraft = DofusApi.Datacenter.CraftsData.GetCraftDataById(ingredient.Key.Id);
                if (subCraft is not null)
                {
                    foreach (KeyValuePair<ItemData, int> subIngredient in subCraft.GetIngredientsWithSubCraft(ingredient.Value))
                    {
                        if (ingredients.ContainsKey(subIngredient.Key))
                        {
                            ingredients[subIngredient.Key] += subIngredient.Value;
                            continue;
                        }

                        ingredients.Add(subIngredient.Key, subIngredient.Value);
                    }

                    continue;
                }

                if (ingredients.ContainsKey(ingredient.Key))
                {
                    ingredients[ingredient.Key] += ingredient.Value;
                    continue;
                }

                ingredients.Add(ingredient.Key, ingredient.Value);
            }

            return ingredients.AsReadOnly();
        }

        public int GetWeight()
        {
            int pods = 0;

            foreach (KeyValuePair<ItemData, int> ingredient in GetIngredients(1))
            {
                pods += ingredient.Key.Weight * ingredient.Value;
            }

            return pods;
        }

        public int GetWeightWithSubCraft()
        {
            int pods = 0;

            foreach (KeyValuePair<ItemData, int> ingredient in GetIngredientsWithSubCraft(1))
            {
                pods += ingredient.Key.Weight * ingredient.Value;
            }

            return pods;
        }

        public TimeSpan GetTimePerCraft(int qte)
        {
            return Formulas.GetTimePerCraft(qte, Ingredients.Count);
        }

        public TimeSpan GetTimePerCraftWithSubCraft(int qte)
        {
            TimeSpan totalTime = TimeSpan.Zero;

            foreach (KeyValuePair<int, int> ingredient in Ingredients)
            {
                CraftData? subCraftData = DofusApi.Datacenter.CraftsData.GetCraftDataById(ingredient.Key);
                if (subCraftData is not null)
                {
                    totalTime += subCraftData.GetTimePerCraftWithSubCraft(qte * ingredient.Value);
                }
            }

            return totalTime + GetTimePerCraft(qte);
        }
    }

    public sealed class CraftsData : IDofusData
    {
        private const string FILE_NAME = "crafts.json";

        [JsonPropertyName("CR")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, CraftData>))]
        public FrozenDictionary<int, CraftData> Crafts { get; init; }

        [JsonConstructor]
        internal CraftsData()
        {
            Crafts = FrozenDictionary<int, CraftData>.Empty;
        }

        internal static CraftsData Load()
        {
            return Datacenter.LoadDataFromFile<CraftsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public CraftData? GetCraftDataById(int id)
        {
            Crafts.TryGetValue(id, out CraftData? craftData);
            return craftData;
        }

        public IEnumerable<CraftData> GetCraftsDataByItemName(string itemName)
        {
            string[] itemNames = itemName.NormalizeCustom().Split(' ');
            foreach (CraftData craftData in Crafts.Values)
            {
                ItemData? itemData = craftData.GetItemData();
                if (itemData is not null && itemNames.All(itemData.NormalizedName.Contains))
                {
                    yield return craftData;
                }
            }
        }
    }
}
