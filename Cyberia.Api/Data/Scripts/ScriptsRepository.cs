using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Scripts;

public sealed class ScriptsRepository : IDofusRepository
{
    private const string c_fileName = "scripts.json";

    [JsonPropertyName("SCR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ScriptDialogData>))]
    public FrozenDictionary<int, ScriptDialogData> ScriptDialogs { get; init; }

    [JsonConstructor]
    internal ScriptsRepository()
    {
        ScriptDialogs = FrozenDictionary<int, ScriptDialogData>.Empty;
    }

    internal static ScriptsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<ScriptsRepository>(filePath);
    }

    public ScriptDialogData? GetScriptDialog(int id)
    {
        ScriptDialogs.TryGetValue(id, out var scriptDialog);
        return scriptDialog;
    }
}
