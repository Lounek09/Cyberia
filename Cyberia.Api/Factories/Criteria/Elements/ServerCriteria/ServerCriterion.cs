using Cyberia.Api.Data.Servers;

namespace Cyberia.Api.Factories.Criteria;

public sealed record ServerCriterion
    : Criterion, ICriterion
{
    public int ServerId { get; init; }

    private ServerCriterion(string id, char @operator, int serverId)
        : base(id, @operator)
    {
        ServerId = serverId;
    }

    internal static ServerCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var serverId))
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
        var serverName = DofusApi.Datacenter.ServersData.GetServerNameById(ServerId);

        return GetDescription(serverName);
    }
}
