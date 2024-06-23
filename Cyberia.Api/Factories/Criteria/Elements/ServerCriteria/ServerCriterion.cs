using Cyberia.Api.Data.Servers;

namespace Cyberia.Api.Factories.Criteria;

public sealed record ServerCriterion : Criterion
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
        return DofusApi.Datacenter.ServersRepository.GetServerDataById(ServerId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Server.{GetOperatorDescriptionKey()}";
    }

    public override Description GetDescription()
    {
        var serverName = DofusApi.Datacenter.ServersRepository.GetServerNameById(ServerId);

        return GetDescription(serverName);
    }
}
