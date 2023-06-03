namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class BastCharacteristicCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
            {
                string element = "";
                switch (values[0])
                {
                    case "1":
                        element = "Force";
                        break;
                    case "2":
                        element = "Inteligence";
                        break;
                    case "3":
                        element = "Chance";
                        break;
                    case "4":
                        element = "Agilité";
                        break;
                    default:
                        element = "Inconnu";
                        break;
                }

                return $"Meilleur élément {@operator} {element.Bold()}";
            }
                

            return null;
        }
    }
}
