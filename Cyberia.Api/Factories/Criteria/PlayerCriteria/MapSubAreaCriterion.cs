namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class MapSubAreaCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int mapSubAreaId))
            {
                string mapSubAreaName = DofusApi.Instance.Datacenter.MapsData.GetMapSubAreaNameById(mapSubAreaId);

                return $"Sous-zone {@operator} {mapSubAreaName.Bold()}";
            }

            return null;
        }
    }
}
