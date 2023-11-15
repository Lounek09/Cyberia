using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class StateData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("p")]
        public int P { get; init; }

        public StateData()
        {
            Name = string.Empty;
        }

        public string GetImagePath()
        {
            return $"{DofusApi.Config.CdnUrl}/images/states/{Id}.png";
        }
    }

    public sealed class StatesData
    {
        private const string FILE_NAME = "states.json";

        [JsonPropertyName("ST")]
        public List<StateData> States { get; init; }

        public StatesData()
        {
            States = [];
        }

        internal static StatesData Build()
        {
            return Json.LoadFromFile<StatesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public StateData? GetStateDataById(int id)
        {
            return States.Find(x => x.Id == id);
        }

        public string GetStateNameById(int id)
        {
            StateData? stateData = GetStateDataById(id);

            return stateData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : stateData.Name;
        }
    }
}
