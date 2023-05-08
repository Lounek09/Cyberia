using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Managers
{
    public static class CraftManager
    {
        public static List<string> GetCraftToString(this Craft craft, int qte, bool recursive, bool highlightItemWithCraft)
        {
            List<string> result = new();
            Dictionary<int, int> ingredients = recursive ? craft.GetRecursiveIngredients(qte) : craft.GetIngredients(qte);

            foreach (KeyValuePair<int, int> ingredient in ingredients)
            {
                Item? item = DofusApi.Instance.Datacenter.ItemsData.GetItemById(ingredient.Key);
                string itemName = DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ingredient.Key).SanitizeMarkDown();
                if (highlightItemWithCraft && item is not null && item.GetCraft() is not null)
                    itemName = itemName.Bold();

                result.Add($"{ingredient.Value.ToStringThousandSeparator().Bold()}x {itemName}");
            }

            return result;
        }

        public static TimeSpan GetTimePerCraft(int qte, int nbSlot)
        {
            try
            {
                return TimeSpan.FromSeconds((1 + (nbSlot - 1) * 0.15) * qte);
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }
    }
}
