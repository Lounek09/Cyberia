﻿using System.Collections;
using System.Collections.ObjectModel;

namespace Cyberia.Api.Factories.Criteria;

public sealed record CriteriaCollection
    : ICriteriaElement, IReadOnlyList<ICriteriaElement>
{
    public ReadOnlyCollection<ICriteriaElement> Items => _items.AsReadOnly();
    public int Count => _items.Count;
    public ICriteriaElement this[int index] => _items[index];

    private readonly List<ICriteriaElement> _items;

    public CriteriaCollection()
    {
        _items = [];
    }

    public CriteriaCollection(List<ICriteriaElement> items)
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