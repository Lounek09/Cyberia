using Cyberia.Api.Factories.Effects;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets.Custom;

internal sealed class ItemSetCustomData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("e")]
    public IReadOnlyList<EffectReadOnlyCollection> Effects { get; init; }

    [JsonConstructor]
    internal ItemSetCustomData()
    {
        Effects = ReadOnlyCollection<EffectReadOnlyCollection>.Empty;
    }
}
