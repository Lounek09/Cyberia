using Cyberia.Api.Data.Items;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Crafts;

public sealed class CraftData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, int>))]
    public IReadOnlyDictionary<int, int> Ingredients { get; init; }

    [JsonConstructor]
    internal CraftData()
    {
        Ingredients = new Dictionary<int, int>();
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(Id);
    }

    public bool HasSubCraft()
    {
        return Ingredients.Any(x => DofusApi.Datacenter.CraftsRepository.GetCraftDataById(x.Key) is not null);
    }

    public IReadOnlyDictionary<ItemData, int> GetIngredients(int quantity)
    {
        Dictionary<ItemData, int> ingredients = [];

        foreach (var ingredient in Ingredients)
        {
            var itemData = DofusApi.Datacenter.ItemsRepository.GetItemDataById(ingredient.Key);
            if (itemData is not null)
            {
                ingredients.Add(itemData, ingredient.Value * quantity);
            }
        }

        return ingredients;
    }

    public IReadOnlyDictionary<ItemData, int> GetIngredientsWithSubCraft(int quantity)
    {
        Dictionary<ItemData, int> ingredients = [];

        foreach (var ingredient in GetIngredients(quantity))
        {
            var subCraft = DofusApi.Datacenter.CraftsRepository.GetCraftDataById(ingredient.Key.Id);
            if (subCraft is not null)
            {
                foreach (var subIngredient in subCraft.GetIngredientsWithSubCraft(ingredient.Value))
                {
                    if (!ingredients.TryAdd(subIngredient.Key, subIngredient.Value))
                    {
                        ingredients[subIngredient.Key] += subIngredient.Value;
                    }
                }

                continue;
            }

            if (!ingredients.TryAdd(ingredient.Key, ingredient.Value))
            {
                ingredients[ingredient.Key] += ingredient.Value;
            }
        }

        return ingredients;
    }

    public int GetWeight()
    {
        var pods = 0;

        foreach (var ingredient in GetIngredients(1))
        {
            pods += ingredient.Key.Weight * ingredient.Value;
        }

        return pods;
    }

    public int GetWeightWithSubCraft()
    {
        var pods = 0;

        foreach (var ingredient in GetIngredientsWithSubCraft(1))
        {
            pods += ingredient.Key.Weight * ingredient.Value;
        }

        return pods;
    }

    public TimeSpan GetTimePerCraft(int quantity)
    {
        return Formulas.GetTimePerCraft(quantity, Ingredients.Count);
    }

    public TimeSpan GetTimePerCraftWithSubCraft(int quantity)
    {
        var totalTime = TimeSpan.Zero;

        foreach (var ingredient in Ingredients)
        {
            var subCraftData = DofusApi.Datacenter.CraftsRepository.GetCraftDataById(ingredient.Key);
            if (subCraftData is not null)
            {
                totalTime += subCraftData.GetTimePerCraftWithSubCraft(quantity * ingredient.Value);
            }
        }

        return totalTime + GetTimePerCraft(quantity);
    }
}
