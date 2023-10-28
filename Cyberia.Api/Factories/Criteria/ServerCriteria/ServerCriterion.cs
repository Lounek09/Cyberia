using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.Criteria.ServerCriteria
{
    public sealed record ServerCriterion : Criterion, ICriterion<ServerCriterion>
    {
        public int ServerId { get; init; }

        private ServerCriterion(string id, char @operator, int serverId) :
            base(id, @operator)
        {
            ServerId = serverId;
        }

        public static ServerCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int serverId))
            {
                return new(id, @operator, serverId);
            }

            return null;
        }

        public ServerData? GetServerData()
        {
            return DofusApi.Datacenter.ServersData.GetServerDataById(ServerId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Server.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string serverName = DofusApi.Datacenter.ServersData.GetServerNameById(ServerId);

            return GetDescription(serverName);
        }
    }
}
