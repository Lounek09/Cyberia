namespace Cyberia.Api.Factories.Criteria.ServerCriteria;

public static class ServerContentCriterion
{
    public static string? GetValue(char @operator, string[] values)
    {
        if (values.Length > 0)
        {
            string value = $"Contenu n°{values[0].Bold()}";

            switch (@operator)
            {
                case '≠':
                    return $"{value} désactivé";
                case '=':
                    return $"{value} activé";
            }
        }

        return null;
    }
}
