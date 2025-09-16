using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs.Localized;

internal sealed class NpcsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => NpcsRepository.FileName;

    [JsonPropertyName("N.a")]
    public IReadOnlyList<NpcActionLocalizedData> NpcActions { get; init; }

    [JsonPropertyName("N.d")]
    public IReadOnlyList<NpcLocalizedData> Npcs { get; init; }

    [JsonConstructor]
    internal NpcsLocalizedRepository()
    {
        NpcActions = ReadOnlyCollection<NpcActionLocalizedData>.Empty;
        Npcs = ReadOnlyCollection<NpcLocalizedData>.Empty;
    }
}
