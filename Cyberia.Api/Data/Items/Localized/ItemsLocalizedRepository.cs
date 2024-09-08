using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items.Localized;

internal sealed class ItemsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => ItemsRepository.FileName;

    [JsonPropertyName("I.us")]
    public IReadOnlyList<ItemUnicStringLocalizedData> ItemUnicStrings { get; init; }

    [JsonPropertyName("I.t")]
    public IReadOnlyList<ItemTypeLocalizedData> ItemTypes { get; init; }

    [JsonPropertyName("I.u")]
    public IReadOnlyList<ItemLocalizedData> Items { get; init; }

    [JsonConstructor]
    internal ItemsLocalizedRepository()
    {
        ItemUnicStrings = [];
        ItemTypes = [];
        Items = [];
    }
}
