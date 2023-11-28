using Cyberia.Api.Data.Items;
using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Crafts;

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

        foreach (var ingredient in Ingredients)
        {
            var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(ingredient.Key);
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

        foreach (var ingredient in GetIngredients(qte))
        {
            var subCraft = DofusApi.Datacenter.CraftsData.GetCraftDataById(ingredient.Key.Id);
            if (subCraft is not null)
            {
                foreach (var subIngredient in subCraft.GetIngredientsWithSubCraft(ingredient.Value))
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

    public TimeSpan GetTimePerCraft(int qte)
    {
        return Formulas.GetTimePerCraft(qte, Ingredients.Count);
    }

    public TimeSpan GetTimePerCraftWithSubCraft(int qte)
    {
        var totalTime = TimeSpan.Zero;

        foreach (var ingredient in Ingredients)
        {
            var subCraftData = DofusApi.Datacenter.CraftsData.GetCraftDataById(ingredient.Key);
            if (subCraftData is not null)
            {
                totalTime += subCraftData.GetTimePerCraftWithSubCraft(qte * ingredient.Value);
            }
        }

        return totalTime + GetTimePerCraft(qte);
    }
}
