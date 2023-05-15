using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class State
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("p")]
        public int P { get; init; }

        public State()
        {
            Name = string.Empty;
        }

        public string GetImagePath()
        {
            return $"{DofusApi.Instance.CdnUrl}/images/states/{Id}.png";
        }
    }

    public sealed class StatesData
    {
        private const string FILE_NAME = "states.json";

        [JsonPropertyName("ST")]
        public List<State> States { get; init; }

        public StatesData()
        {
            States = new();
        }

        internal static StatesData Build()
        {
            return Json.LoadFromFile<StatesData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public State? GetStateById(int id)
        {
            return States.Find(x => x.Id == id);
        }

        public string GetStateNameById(int id)
        {
            State? state = GetStateById(id);

            return state is null ? $"Inconnu ({id})" : state.Name;
        }
    }
}
