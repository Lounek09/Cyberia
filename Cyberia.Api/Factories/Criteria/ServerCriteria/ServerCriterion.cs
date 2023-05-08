namespace Cyberia.Api.Factories.Criteria.ServerCriteria
{
    public static class ServerCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int serverId))
            {
                string serverName = DofusApi.Instance.Datacenter.ServersData.GetServerNameById(serverId);

                return $"Serveur {@operator} {serverName.Bold()}";
            }

            return null;
        }
    }
}