namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class BreedCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int classId))
            {
                string className = DofusApi.Instance.Datacenter.BreedsData.GetBreedNameById(classId);

                return $"Classe {@operator} {className.Bold()}";
            }

            return null;
        }
    }
}
