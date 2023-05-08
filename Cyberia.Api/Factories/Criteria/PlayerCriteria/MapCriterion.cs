using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class MapCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int mapId))
            {
                Map? map = DofusApi.Instance.Datacenter.MapsData.GetMapById(mapId);
                string areaSubAreaName = map is null ? "Inconnu" : map.GetMapAreaName();
                string coordinate = map is null ? "[x, x]" : map.GetCoordinate();

                switch (@operator)
                {
                    case '≠':
                        return $"Ne pas être sur la map {coordinate.Bold()} ({areaSubAreaName.Bold()})";
                    case '=':
                        return $"Être sur la map {coordinate.Bold()} ({areaSubAreaName.Bold()})";
                }
            }

            return null;
        }
    }
}
