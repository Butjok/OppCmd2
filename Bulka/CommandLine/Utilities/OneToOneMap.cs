using System.Collections;
using System.Collections.Generic;

namespace Bulka.CommandLine.Utilities
{
    public class OneToOneMap<TKey, TValue> : IReadOnlyOneToOneMap<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _forward;
        private readonly Dictionary<TValue, TKey> _backward;

        public OneToOneMap(IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
        {
            _forward  = new Dictionary<TKey, TValue>(keyComparer);
            _backward = new Dictionary<TValue, TKey>(valueComparer);
        }

        public IEnumerable<TValue> Values => _forward.Values;
        public IEnumerable<TKey> Keys => _forward.Keys;

        public int Count => _forward.Count;

        public void Add(TKey left, TValue right)
        {
            (!_backward.ContainsKey(right)).Assert(right.ToString);
            _forward.Add(left, right);
            _backward.Add(right, left);
        }

        public bool ContainsKey(TKey left)
        {
            return _forward.ContainsKey(left);
        }
        public bool ContainsValue(TValue right)
        {
            return _backward.ContainsKey(right);
        }

        public bool TryGetValue(TKey left, out TValue right)
        {
            return _forward.TryGetValue(left, out right);
        }
        public bool TryGetKey(TValue right, out TKey left)
        {
            return _backward.TryGetValue(right, out left);
        }

        public void RemoveByKey(TKey left)
        {
            var right = _forward[left];
            _backward.Remove(right);
            _forward.Remove(left);
        }
        public void RemoveByValue(TValue right)
        {
            var left = _backward[right];
            _forward.Remove(left);
            _backward.Remove(right);
        }

        public TValue GetValue(TKey left)
        {
            return _forward[left];
        }
        public TKey GetKey(TValue right)
        {
            return _backward[right];
        }

        public TValue this[TKey key]
        {
            set => Add(key, value);
        }

        public IEnumerator<(TKey, TValue)> GetEnumerator()
        {
            foreach (var pair in _forward)
                yield return (pair.Key, pair.Value);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}