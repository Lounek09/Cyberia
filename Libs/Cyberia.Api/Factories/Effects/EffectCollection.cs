using System.Collections;

namespace Cyberia.Api.Factories.Effects;

/// <summary>
/// Represents a collection of <see cref="Effect"/>.
/// </summary>
public sealed class EffectCollection : IList<Effect>
{
    public int Count => _items.Count;

    public bool IsReadOnly => throw new NotImplementedException();

    Effect IList<Effect>.this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    /// <summary>
    /// The internal list of items in the collection.
    /// </summary>
    private readonly IList<Effect> _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="EffectCollection"/> class.
    /// </summary>
    public EffectCollection()
    {
        _items = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EffectCollection"/> class.
    /// </summary>
    /// <param name="capacity">The number of elements that the new list can initially store.</param>
    public EffectCollection(int capacity)
    {
        _items = new List<Effect>(capacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EffectCollection"/> class.
    /// </summary>
    /// <param name="items">The items to add to the collection.</param>
    public EffectCollection(IList<Effect> items)
    {
        _items = items;
    }

    /// <summary>
    /// Returns a <see cref="EffectReadOnlyCollection"/> wrapper for this instance.
    /// </summary>
    /// <returns>an object that acts as a read-only wrapper around the current instance.</returns>
    public EffectReadOnlyCollection AsReadOnly()
    {
        return new EffectReadOnlyCollection(this);
    }

    public int IndexOf(Effect item)
    {
        return _items.IndexOf(item);
    }

    public void Insert(int index, Effect item)
    {
        _items.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _items.RemoveAt(index);
    }

    public void Add(Effect item)
    {
        _items.Add(item);
    }

    public void Clear()
    {
        _items?.Clear();
    }

    public bool Contains(Effect item)
    {
        return _items.Contains(item);
    }

    public void CopyTo(Effect[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    public bool Remove(Effect item)
    {
        return _items.Remove(item);
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
