using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Aligments;

internal sealed class AlignmentAttackData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public IReadOnlyList<bool> Values { get; init; }

    [JsonConstructor]
    internal AlignmentAttackData()
    {
        Values = [];
    }

    public bool CanAttack(int targetAlignmentId)
    {
        return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
    }
}
