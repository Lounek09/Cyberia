using System.Collections;
using System.Collections.ObjectModel;

namespace Cyberia.Api.Factories.Criteria
{
    public sealed record CriteriaCollection : ICriteriaElement, ICollection<ICriteriaElement>
    {
        private readonly Collection<ICriteriaElement> _items;

        public int Count => _items.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public CriteriaCollection()
        {
            _items = [];
        }

        public void Add(ICriteriaElement item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(ICriteriaElement item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(ICriteriaElement[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(ICriteriaElement item)
        {
            return _items.Remove(item);
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
}
