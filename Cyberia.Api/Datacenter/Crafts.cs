using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class CraftData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(DictionaryJsonConverter<int, int>))]
        public Dictionary<int, int> Ingredients { get; init; }

        public CraftData()
        {
            Ingredients = new();
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(Id);
        }

        public bool HasSubCraft()
        {
            foreach (KeyValuePair<int, int> ingredient in Ingredients)
            {
                CraftData? subCraftData = DofusApi.Instance.Datacenter.CraftsData.GetCraftDataById(ingredient.Key);
                if (subCraftData is not null)
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<int, int> GetIngredients(int qte)
        {
            Dictionary<int, int> ingredients = new();

            foreach (KeyValuePair<int, int> ingredient in Ingredients)
            {
                if (ingredients.TryGetValue(ingredient.Key, out _))
                {
                    ingredients[ingredient.Key] += qte * ingredient.Value;
                    continue;
                }

                ingredients.Add(ingredient.Key, qte * ingredient.Value);
            }

            return ingredients;
        }

        public Dictionary<int, int> GetRecursiveIngredients(int qte, Dictionary<int, int>? ingredients = null, CraftData? craftData = null)
        {
            ingredients ??= new();
            craftData ??= this;

            foreach (KeyValuePair<int, int> ingredient in craftData.GetIngredients(qte))
            {
                CraftData? subCraft = DofusApi.Instance.Datacenter.CraftsData.GetCraftDataById(ingredient.Key);

                if (subCraft is not null)
                {
                    ingredients = GetRecursiveIngredients(ingredient.Value, ingredients, subCraft);
                    continue;
                }

                if (ingredients.TryGetValue(ingredient.Key, out _))
                {
                    ingredients[ingredient.Key] += ingredient.Value;
                    continue;
                }

                ingredients.Add(ingredient.Key, ingredient.Value);
            }

            return ingredients;
        }

        public int GetWeight()
        {
            int pods = 0;

            foreach (KeyValuePair<int, int> ingredient in GetIngredients(1))
            {
                ItemData? itemData = DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(ingredient.Key);
                if (itemData is not null)
                {
                    pods += itemData.Weight * ingredient.Value;
                }
            }

            return pods;
        }

        public int GetRecursiveWeight()
        {
            int pods = 0;

            foreach (KeyValuePair<int, int> ingredient in GetRecursiveIngredients(1))
            {
                ItemData? itemData = DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(ingredient.Key);
                if (itemData is not null)
                {
                    pods += itemData.Weight * ingredient.Value;
                }
            }

            return pods;
        }

        public TimeSpan GetTimeForMultipleCraft(int qte)
        {
            return Formulas.GetTimePerCraft(qte, Ingredients.Count);
        }

        public TimeSpan GetRecursiveTimeForMultipleCraft(int qte, TimeSpan? totalTime = null, CraftData? craftData = null)
        {
            totalTime ??= TimeSpan.Zero;
            craftData ??= this;

            foreach (KeyValuePair<int, int> ingredient in craftData.Ingredients)
            {
                CraftData? subCraftData = DofusApi.Instance.Datacenter.CraftsData.GetCraftDataById(ingredient.Key);
                if (subCraftData is not null)
                {
                    totalTime = GetRecursiveTimeForMultipleCraft(qte * ingredient.Value, totalTime, subCraftData);
                }
            }

            return totalTime.Value + craftData.GetTimeForMultipleCraft(qte);
        }
    }

    public sealed class CraftsData
    {
        private const string FILE_NAME = "crafts.json";

        [JsonPropertyName("CR")]
        public List<CraftData> Crafts { get; init; }

        public CraftsData()
        {
            Crafts = new();
        }

        internal static CraftsData Build()
        {
            return Json.LoadFromFile<CraftsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public CraftData? GetCraftDataById(int id)
        {
            return Crafts.Find(x => x.Id == id);
        }

        public List<CraftData> GetCraftsDataByItemName(string itemName)
        {
            string[] itemNames = ExtendString.Normalize(itemName).Split(' ');
            List<CraftData> craftsData = new();
            foreach (CraftData craftData in Crafts)
            {
                ItemData? itemData = craftData.GetItemData();
                if (itemData is not null && itemNames.All(itemData.NormalizedName.Contains))
                {
                    craftsData.Add(craftData);
                }
            }

            return craftsData;
        }
    }
}
