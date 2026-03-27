using Cyberia.Api.JsonConverters;

using System.Collections;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Factories.Effects;

/// <summary>
/// Represents a read-only collection of <see cref="Effect"/>.
/// </summary>
[JsonConverter(typeof(EffectReadOnlyCollectionConverter))]
public sealed class EffectReadOnlyCollection : IReadOnlyList<Effect>
{
    /// <summary>
    /// Gets an empty <see cref="EffectReadOnlyCollection"/>.
    /// </summary>
    public static readonly EffectReadOnlyCollection Empty = new(Array.Empty<Effect>());

    public int Count => _items.Count;

    public Effect this[int index] => _items[index];

    /// <summary>
    /// The internal list of items in the collection.
    /// </summary>
    private readonly ReadOnlyCollection<Effect> _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="EffectReadOnlyCollection"/> class.
    /// </summary>
    /// <param name="items">The items to add to the collection.</param>
    public EffectReadOnlyCollection(IList<Effect> items)
    {
        _items = items.AsReadOnly();
    }

    public IEnumerator<Effect> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
