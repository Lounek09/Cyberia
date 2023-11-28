using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Scripts;

public sealed class ScriptsData : IDofusData
{
    private const string FILE_NAME = "scripts.json";

    [JsonPropertyName("SCR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ScriptDialogData>))]
    public FrozenDictionary<int, ScriptDialogData> ScriptDialogs { get; init; }

    [JsonConstructor]
    internal ScriptsData()
    {
        ScriptDialogs = FrozenDictionary<int, ScriptDialogData>.Empty;
    }

    internal static ScriptsData Load()
    {
        return Datacenter.LoadDataFromFile<ScriptsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public ScriptDialogData? GetScriptDialog(int id)
    {
        ScriptDialogs.TryGetValue(id, out var scriptDialog);
        return scriptDialog;
    }
}
