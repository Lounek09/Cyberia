using System.Collections;
using System.Collections.ObjectModel;

namespace Cyberia.Api.Factories.Criteria;

public sealed record CriteriaReadOnlyCollection
    : ICriteriaElement, IReadOnlyList<ICriteriaElement>
{
    public ReadOnlyCollection<ICriteriaElement> Items => _items.AsReadOnly();
    public int Count => _items.Count;
    public ICriteriaElement this[int index] => _items[index];

    private readonly List<ICriteriaElement> _items;

    public CriteriaReadOnlyCollection()
    {
        _items = [];
    }

    public CriteriaReadOnlyCollection(List<ICriteriaElement> items)
    {
        _items = items;
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
