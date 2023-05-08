using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class ScriptDialog
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Message { get; init; }

        public ScriptDialog()
        {
            Message = string.Empty;
        }
    }

    public sealed class ScriptsData
    {
        private const string FILE_NAME = "scripts.json";

        [JsonPropertyName("SCR")]
        public List<ScriptDialog> ScriptDialogs { get; init; }

        public ScriptsData()
        {
            ScriptDialogs = new();
        }

        internal static ScriptsData Build()
        {
            return Json.LoadFromFile<ScriptsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }
    }
}
