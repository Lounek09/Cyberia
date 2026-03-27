using Cyberia.Api.JsonConverters;

using System.Collections;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Factories.Criteria;

/// <summary>
/// Represents a read-only collection of <see cref="ICriteriaElement"/>.
/// </summary>
[JsonConverter(typeof(CriterionReadOnlyCollectionConverter))]
public sealed class CriterionReadOnlyCollection : ICriteriaElement, IReadOnlyList<ICriteriaElement>
{
    /// <summary>
    /// Gets an empty <see cref="CriterionReadOnlyCollection"/>.
    /// </summary>
    public static readonly CriterionReadOnlyCollection Empty = new(Array.Empty<ICriteriaElement>());

    public int Count => _items.Count;

    public ICriteriaElement this[int index] => _items[index];

    /// <summary>
    /// The internal list of items in the collection.
    /// </summary>
    private readonly ReadOnlyCollection<ICriteriaElement> _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="CriterionReadOnlyCollection"/> class.
    /// </summary>
    /// <param name="items">The items to add to the collection.</param>
    public CriterionReadOnlyCollection(IList<ICriteriaElement> items)
    {
        _items = items.AsReadOnly();
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
