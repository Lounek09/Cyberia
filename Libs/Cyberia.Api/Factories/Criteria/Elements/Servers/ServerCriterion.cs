using Cyberia.Api.Data.Servers;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Servers;

public sealed record ServerCriterion : Criterion
{
    public int ServerId { get; init; }

    private ServerCriterion(string id, char @operator, int serverId)
        : base(id, @operator)
    {
        ServerId = serverId;
    }

    internal static ServerCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
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

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var serverName = DofusApi.Datacenter.ServersRepository.GetServerNameById(ServerId, culture);

        return GetDescription(culture, serverName);
    }
}
