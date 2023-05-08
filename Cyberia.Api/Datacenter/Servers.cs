using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Server
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("l")]
        public string Language { get; init; }

        [JsonPropertyName("p")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int ServerPopulationId { get; init; }

        [JsonPropertyName("t")]
        public int Type { get; init; }

        [JsonPropertyName("c")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int ServerCommunityId { get; init; }

        [JsonPropertyName("date")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long Date { get; init; }

        [JsonPropertyName("rlng")]
        public List<string> RealLanguages { get; init; }

        public Server()
        {
            Name = string.Empty;
            Description = string.Empty;
            Language = string.Empty;
            RealLanguages = new();
        }

        public ServerPopulation? GetServerPopulation()
        {
            return DofusApi.Instance.Datacenter.ServersData.GetServerPopulationById(ServerPopulationId);
        }

        public ServerPopulationWeight? GetServerPopulationWeight()
        {
            return DofusApi.Instance.Datacenter.ServersData.GetServerPopulationWeightById(ServerPopulationId);
        }

        public ServerCommunity? GetServerCommunity()
        {
            return DofusApi.Instance.Datacenter.ServersData.GetServerCommunityById(ServerCommunityId);
        }
    }

    public sealed class ServerPopulation
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public ServerPopulation()
        {
            Name = string.Empty;
        }
    }

    public sealed class ServerPopulationWeight
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public int Weight { get; init; }

        public ServerPopulationWeight()
        {

        }
    }

    public sealed class ServerCommunity
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public bool Visible { get; init; }

        [JsonPropertyName("i")]
        public int Id2 { get; init; }

        [JsonPropertyName("c")]
        public List<string> Countries { get; init; }

        public ServerCommunity()
        {
            Name = string.Empty;
            Countries = new();
        }
    }

    public sealed class DefaultServerSpecificText
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("l")]
        public string Label { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        public DefaultServerSpecificText()
        {
            Label = string.Empty;
            Description = string.Empty;
        }
    }

    public sealed class ServerSpecificText
    {
        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("v")]
        public string Description { get; init; }

        public ServerSpecificText()
        {
            Id = string.Empty;
            Description = string.Empty;
        }
    }

    public sealed class ServersData
    {
        private const string FILE_NAME = "servers.json";

        [JsonPropertyName("SR")]
        public List<Server> Servers { get; init; }

        [JsonPropertyName("SRP")]
        public List<ServerPopulation> ServerPopulations { get; init; }

        [JsonPropertyName("SRPW")]
        public List<ServerPopulationWeight> ServerPopulationsWeight { get; init; }

        [JsonPropertyName("SRC")]
        public List<ServerCommunity> ServerCommunities { get; init; }

        [JsonPropertyName("SRVT")]
        public List<DefaultServerSpecificText> DefaultServerSpecificTexts { get; init; }

        [JsonPropertyName("SRVC")]
        public List<ServerSpecificText> ServerSpecificTexts { get; init; }

        public ServersData()
        {
            Servers = new();
            ServerPopulations = new();
            ServerPopulationsWeight = new();
            ServerCommunities = new();
            DefaultServerSpecificTexts = new();
            ServerSpecificTexts = new();
        }

        internal static ServersData Build()
        {
            return Json.LoadFromFile<ServersData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Server? GetServerById(int id)
        {
            return Servers.Find(x => x.Id == id);
        }

        public string GetServerNameById(int id)
        {
            Server? server = GetServerById(id);

            return server is null ? $"Inconnu ({id})" : server.Name;
        }

        public ServerPopulation? GetServerPopulationById(int id)
        {
            return ServerPopulations.Find(x => x.Id == id);
        }

        public ServerPopulationWeight? GetServerPopulationWeightById(int id)
        {
            return ServerPopulationsWeight.Find(x => x.Id == id);
        }

        public ServerCommunity? GetServerCommunityById(int id)
        {
            return ServerCommunities.Find(x => x.Id == id);
        }

    }
}
