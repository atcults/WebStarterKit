using System.Linq;

namespace Core.ViewOnly.Base
{
    internal class ArrayKey<T>
    {
        private readonly int _hashCode;
        private readonly T[] _keys;

        public ArrayKey(T[] keys)
        {
            // Store the keys
            _keys = keys;

            // Calculate the hashcode
            _hashCode = 17;
            foreach (var k in keys)
            {
                _hashCode = _hashCode*23 + (k == null ? 0 : k.GetHashCode());
            }
        }

        private bool Equals(ArrayKey<T> other)
        {
            if (other == null)
                return false;

            if (other._hashCode != _hashCode)
                return false;

            if (other._keys.Length != _keys.Length)
                return false;

            return !_keys.Where((t, i) => !Equals(t, other._keys[i])).Any();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ArrayKey<T>);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
}