using Cyberia.Api.Managers;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Craft
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(DictionaryJsonConverter<int, int>))]
        public Dictionary<int, int> Ingredients { get; init; }

        public Craft()
        {
            Ingredients = new();
        }

        public Item? GetItem()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemById(Id);
        }

        public bool HasSubCraft()
        {
            foreach (KeyValuePair<int, int> ingredient in Ingredients)
            {
                Craft? subCraft = DofusApi.Instance.Datacenter.CraftsData.GetCraftById(ingredient.Key);
                if (subCraft is not null)
                    return true;
            }

            return false;
        }

        public Dictionary<int, int> GetIngredients(int qte)
        {
            Dictionary<int, int> ingredients = new();

            foreach (KeyValuePair<int, int> ingredient in Ingredients)
            {
                if (ingredients.TryGetValue(ingredient.Key, out _))
                    ingredients[ingredient.Key] += qte * ingredient.Value;
                else
                    ingredients.Add(ingredient.Key, qte * ingredient.Value);
            }

            return ingredients;
        }

        public Dictionary<int, int> GetRecursiveIngredients(int qte, Dictionary<int, int>? ingredients = null, Craft? craft = null)
        {
            ingredients ??= new();
            craft ??= this;

            foreach (KeyValuePair<int, int> ingredient in craft.GetIngredients(qte))
            {
                Craft? subCraft = DofusApi.Instance.Datacenter.CraftsData.GetCraftById(ingredient.Key);

                if (subCraft is null)
                {
                    if (ingredients.TryGetValue(ingredient.Key, out _))
                        ingredients[ingredient.Key] += ingredient.Value;
                    else
                        ingredients.Add(ingredient.Key, ingredient.Value);
                }
                else
                    ingredients = GetRecursiveIngredients(ingredient.Value, ingredients, subCraft);
            }

            return ingredients;
        }

        public int GetWeight()
        {
            int pods = 0;

            foreach (KeyValuePair<int, int> ingredient in GetIngredients(1))
            {
                Item? item = DofusApi.Instance.Datacenter.ItemsData.GetItemById(ingredient.Key);
                if (item is not null)
                    pods += item.Weight * ingredient.Value;
            }

            return pods;
        }

        public int GetRecursiveWeight()
        {
            int pods = 0;

            foreach (KeyValuePair<int, int> ingredient in GetRecursiveIngredients(1))
            {
                Item? item = DofusApi.Instance.Datacenter.ItemsData.GetItemById(ingredient.Key);

                if (item is not null)
                    pods += item.Weight * ingredient.Value;
            }

            return pods;
        }

        public TimeSpan GetTimeForMultipleCraft(int qte)
        {
            return CraftManager.GetTimePerCraft(qte, Ingredients.Count);
        }

        public TimeSpan GetRecursiveTimeForMultipleCraft(int qte, TimeSpan? totalTime = null, Craft? craft = null)
        {
            totalTime ??= TimeSpan.Zero;
            craft ??= this;

            foreach (KeyValuePair<int, int> ingredient in craft.Ingredients)
            {
                Craft? subCraft = DofusApi.Instance.Datacenter.CraftsData.GetCraftById(ingredient.Key);
                if (subCraft is not null)
                    totalTime = GetRecursiveTimeForMultipleCraft(qte * ingredient.Value, totalTime, subCraft);
            }

            return totalTime.Value + craft.GetTimeForMultipleCraft(qte);
        }
    }

    public sealed class CraftsData
    {
        private const string FILE_NAME = "crafts.json";

        [JsonPropertyName("CR")]
        public List<Craft> Crafts { get; init; }

        public CraftsData()
        {
            Crafts = new();
        }

        internal static CraftsData Build()
        {
            return Json.LoadFromFile<CraftsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Craft? GetCraftById(int id)
        {
            return Crafts.Find(x => x.Id == id);
        }

        public List<Craft> GetCraftsByItemName(string itemName)
        {
            string[] itemNames = itemName.RemoveDiacritics().Split(' ');
            List<Craft> crafts = new();
            foreach (Craft craft in Crafts)
            {
                Item? item = craft.GetItem();
                if (item is not null && itemNames.All(item.Name.RemoveDiacritics().Contains))
                    crafts.Add(craft);
            }

            return crafts;
        }
    }
}
