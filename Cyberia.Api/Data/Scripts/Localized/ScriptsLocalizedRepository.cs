using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Scripts.Localized;

internal sealed class ScriptsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => ScriptsRepository.FileName;

    [JsonPropertyName("SCR")]
    public IReadOnlyList<ScriptDialogLocalizedData> ScriptDialogs { get; init; }

    [JsonConstructor]
    internal ScriptsLocalizedRepository()
    {
        ScriptDialogs = [];
    }
}
