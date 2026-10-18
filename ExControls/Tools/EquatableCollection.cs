using System.Collections.ObjectModel;

namespace ExControls;

/// <summary>
/// Provides the base class for a generic collection that supports equality (two collections are equal when they contain
/// equal items in the same order).
/// </summary>
/// <typeparam name="T">type of the items</typeparam>
public class EquatableCollection<T> : Collection<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EquatableCollection{T}" /> class with a copy of the given items.
    /// </summary>
    /// <param name="list">items of the new collection</param>
    public EquatableCollection(IEnumerable<T> list) : base(list.ToArray())
    {
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is IEnumerable<T> otherEnumerable && otherEnumerable.SequenceEqual(this);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = 43;
        unchecked
        {
            hash = this.Aggregate(hash, (current, item) => 19 * current + (item?.GetHashCode() ?? 0));
        }

        return hash;
    }
}