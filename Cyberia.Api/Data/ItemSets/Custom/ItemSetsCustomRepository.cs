using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets.Custom;

internal sealed class ItemSetsCustomRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => ItemSetsRepository.FileName;

    [JsonPropertyName("CIS")]
    public IReadOnlyList<ItemSetCustomData> ItemSetsCustom { get; init; }

    [JsonConstructor]
    internal ItemSetsCustomRepository()
    {
        ItemSetsCustom = [];
    }
}
