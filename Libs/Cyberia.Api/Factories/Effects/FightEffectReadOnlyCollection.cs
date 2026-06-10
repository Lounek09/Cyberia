using System.Collections;
using System.Collections.ObjectModel;

namespace Cyberia.Api.Factories.Effects;

/// <summary>
/// Represents a read-only collection of <see cref="FightEffect"/>.
/// </summary>
//[JsonConverter(typeof(FightEffectReadOnlyCollectionConverter))]
public sealed class FightEffectReadOnlyCollection : IReadOnlyList<FightEffect>
{
    /// <summary>
    /// Gets an empty <see cref="EffectReadOnlyCollection"/>.
    /// </summary>
    public static readonly FightEffectReadOnlyCollection Empty = new(Array.Empty<FightEffect>());

    public int Count => _items.Count;

    public FightEffect this[int index] => _items[index];

    /// <summary>
    /// The internal list of items in the collection.
    /// </summary>
    private readonly ReadOnlyCollection<FightEffect> _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="EffectReadOnlyCollection"/> class.
    /// </summary>
    /// <param name="items">The items to add to the collection.</param>
    public FightEffectReadOnlyCollection(IList<FightEffect> items)
    {
        _items = items.AsReadOnly();
    }

    public IEnumerator<FightEffect> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
