using Cyberia.Api.JsonConverters;

using System.Collections;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Factories.Criteria.Elements;

/// <summary>
/// Represents a read-only collection of criteria.
/// </summary>
[JsonConverter(typeof(CriteriaReadOnlyCollectionConverter))]
public sealed record CriteriaReadOnlyCollection : ICriteriaElement, IReadOnlyList<ICriteriaElement>
{
    /// <summary>
    /// Gets the items in the collection.
    /// </summary>
    public ReadOnlyCollection<ICriteriaElement> Items => _items.AsReadOnly();

    public int Count => _items.Count;

    public ICriteriaElement this[int index] => _items[index];

    /// <summary>
    /// The internal list of items in the collection.
    /// </summary>
    private readonly IList<ICriteriaElement> _items;

    /// <summary>
    /// Initializes a new empty instance of the <see cref="CriteriaReadOnlyCollection"/> record.
    /// </summary>
    public CriteriaReadOnlyCollection()
    {
        _items = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CriteriaReadOnlyCollection"/> record with the specified items.
    /// </summary>
    /// <param name="items">The items to add to the collection.</param>
    public CriteriaReadOnlyCollection(IEnumerable<ICriteriaElement> items)
    {
        _items = items as IList<ICriteriaElement> ?? new List<ICriteriaElement>(items);
    }

    public IEnumerator<ICriteriaElement> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
