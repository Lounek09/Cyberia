using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class ScriptDialogData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Message { get; init; }

        public ScriptDialogData()
        {
            Message = string.Empty;
        }
    }

    public sealed class ScriptsData
    {
        private const string FILE_NAME = "scripts.json";

        [JsonPropertyName("SCR")]
        public List<ScriptDialogData> ScriptDialogs { get; init; }

        public ScriptsData()
        {
            ScriptDialogs = new();
        }

        internal static ScriptsData Build()
        {
            return Json.LoadFromFile<ScriptsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }
    }
}
