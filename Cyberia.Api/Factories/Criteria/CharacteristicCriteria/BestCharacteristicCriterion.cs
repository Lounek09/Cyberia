namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class BastCharacteristicCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
            {
                string element = values[0] switch
                {
                    "1" => "Force",
                    "2" => "Inteligence",
                    "3" => "Chance",
                    "4" => "Agilité",
                    _ => "Inconnu",
                };

                return $"Meilleur élément {@operator} {element.Bold()}";
            }


            return null;
        }
    }
}
