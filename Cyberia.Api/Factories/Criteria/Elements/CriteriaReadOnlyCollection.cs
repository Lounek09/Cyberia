using Cyberia.Api.JsonConverters;

using System.Collections;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Factories.Criteria;

[JsonConverter(typeof(CriteriaReadOnlyCollectionConverter))]
public sealed record CriteriaReadOnlyCollection : ICriteriaElement, IReadOnlyList<ICriteriaElement>
{
    public ReadOnlyCollection<ICriteriaElement> Items => _items.AsReadOnly();
    public int Count => _items.Count;
    public ICriteriaElement this[int index] => _items[index];

    private readonly IList<ICriteriaElement> _items;

    public CriteriaReadOnlyCollection()
    {
        _items = [];
    }

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
