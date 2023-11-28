using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Aligments;

internal sealed class AlignmentAttackData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<bool>))]
    public ReadOnlyCollection<bool> Values { get; init; }

    [JsonConstructor]
    internal AlignmentAttackData()
    {
        Values = ReadOnlyCollection<bool>.Empty;
    }

    public bool CanAttack(int targetAlignmentId)
    {
        return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
    }
}
