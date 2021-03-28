using System.Collections.Generic;

namespace Bulka.CommandLine.Utilities
{
    public interface IReadOnlyOneToOneMap<TKey, TValue> : IEnumerable<(TKey, TValue)>
    {
        IEnumerable<TValue> Values { get; }
        IEnumerable<TKey> Keys { get; }
        int Count { get; }
        bool ContainsKey(TKey left);
        bool ContainsValue(TValue right);
        bool TryGetValue(TKey left, out TValue right);
        bool TryGetKey(TValue right, out TKey left);
        TValue GetValue(TKey left);
        TKey GetKey(TValue right);
    }
}