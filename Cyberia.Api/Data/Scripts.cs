using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class ScriptDialogData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Message { get; init; }

        [JsonConstructor]
        internal ScriptDialogData()
        {
            Message = string.Empty;
        }
    }

    public sealed class ScriptsData
    {
        private const string FILE_NAME = "scripts.json";

        [JsonPropertyName("SCR")]
        public List<ScriptDialogData> ScriptDialogs { get; init; }

        [JsonConstructor]
        public ScriptsData()
        {
            ScriptDialogs = [];
        }

        internal static ScriptsData Load()
        {
            return Json.LoadFromFile<ScriptsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }
    }
}
