namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class ItemCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int itemId))
            {
                string itemName = DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(itemId).SanitizeMarkDown();

                switch (@operator)
                {
                    case '≠':
                        return $"Ne pas posséder l'objet {itemName.Bold()}";
                    case '=':
                        return $"Posséder l'objet {itemName.Bold()}";
                    case 'X':
                        return $"Ne pas avoir équipé l'objet {itemName.Bold()}";
                    case 'E':
                        return $"Avoir équipé l'objet {itemName.Bold()}";
                }
            }

            return null;
        }
    }
}
