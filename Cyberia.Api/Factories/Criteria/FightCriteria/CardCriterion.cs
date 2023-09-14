namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class CardInHandCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
            {
                string hand = values[0] switch
                {
                    "1" => "Une pair",
                    "2" => "Double pair",
                    "3" => "Brelan",
                    "4" => "Carré",
                    _ => "Inconnu",
                };

                return $"Main {@operator} {hand.Bold()}";
            }


            return null;
        }
    }
}
