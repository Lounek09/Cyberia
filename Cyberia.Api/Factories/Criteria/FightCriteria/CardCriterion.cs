namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class CardInHandCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
            {
                string hand = "";
                switch (values[0])
                {
                    case "1":
                        hand = "Une pair";
                        break;
                    case "2":
                        hand = "Double pair";
                        break;
                    case "3":
                        hand = "Brelan";
                        break;
                    case "4":
                        hand = "Carré";
                        break;
                    default:
                        hand = "Inconnu";
                        break;
                }

                return $"Main {@operator} {hand.Bold()}";
            }
                

            return null;
        }
    }
}
