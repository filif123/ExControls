using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExControls
{
    /// <summary>
    ///     Provides the base class for a generic collection that support equality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EquatableCollection<T> : Collection<T>
    {
        /// <summary>
        /// </summary>
        /// <param name="list"></param>
        public EquatableCollection(IEnumerable<T> list) : base(list.ToArray())
        {
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return other is IEnumerable<T> otherEnumerable && otherEnumerable.SequenceEqual(this);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = 43;
            unchecked
            {
                foreach (var item in this) hash = 19 * hash + item.GetHashCode();
            }

            return hash;
        }
    }
}