using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class ServerData
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

        public ServerData()
        {
            Name = string.Empty;
            Description = string.Empty;
            Language = string.Empty;
            RealLanguages = new();
        }

        public ServerPopulationData? GetServerPopulationData()
        {
            return DofusApi.Instance.Datacenter.ServersData.GetServerPopulationDataById(ServerPopulationId);
        }

        public ServerPopulationWeightData? GetServerPopulationWeightData()
        {
            return DofusApi.Instance.Datacenter.ServersData.GetServerPopulationWeightDataById(ServerPopulationId);
        }

        public ServerCommunityData? GetServerCommunityData()
        {
            return DofusApi.Instance.Datacenter.ServersData.GetServerCommunityDataById(ServerCommunityId);
        }
    }

    public sealed class ServerPopulationData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public ServerPopulationData()
        {
            Name = string.Empty;
        }
    }

    public sealed class ServerPopulationWeightData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public int Weight { get; init; }

        public ServerPopulationWeightData()
        {

        }
    }

    public sealed class ServerCommunityData
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

        public ServerCommunityData()
        {
            Name = string.Empty;
            Countries = new();
        }
    }

    public sealed class DefaultServerSpecificTextData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("l")]
        public string Label { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        public DefaultServerSpecificTextData()
        {
            Label = string.Empty;
            Description = string.Empty;
        }
    }

    public sealed class ServerSpecificTextData
    {
        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("v")]
        public string Description { get; init; }

        public ServerSpecificTextData()
        {
            Id = string.Empty;
            Description = string.Empty;
        }
    }

    public sealed class ServersData
    {
        private const string FILE_NAME = "servers.json";

        [JsonPropertyName("SR")]
        public List<ServerData> Servers { get; init; }

        [JsonPropertyName("SRP")]
        public List<ServerPopulationData> ServerPopulations { get; init; }

        [JsonPropertyName("SRPW")]
        public List<ServerPopulationWeightData> ServerPopulationsWeight { get; init; }

        [JsonPropertyName("SRC")]
        public List<ServerCommunityData> ServerCommunities { get; init; }

        [JsonPropertyName("SRVT")]
        public List<DefaultServerSpecificTextData> DefaultServerSpecificTexts { get; init; }

        [JsonPropertyName("SRVC")]
        public List<ServerSpecificTextData> ServerSpecificTexts { get; init; }

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
            return Json.LoadFromFile<ServersData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public ServerData? GetServerDataById(int id)
        {
            return Servers.Find(x => x.Id == id);
        }

        public string GetServerNameById(int id)
        {
            ServerData? serverData = GetServerDataById(id);

            return serverData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : serverData.Name;
        }

        public ServerPopulationData? GetServerPopulationDataById(int id)
        {
            return ServerPopulations.Find(x => x.Id == id);
        }

        public ServerPopulationWeightData? GetServerPopulationWeightDataById(int id)
        {
            return ServerPopulationsWeight.Find(x => x.Id == id);
        }

        public ServerCommunityData? GetServerCommunityDataById(int id)
        {
            return ServerCommunities.Find(x => x.Id == id);
        }

    }
}
