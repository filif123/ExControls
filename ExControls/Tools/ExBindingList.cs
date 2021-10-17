using System;
using System.Collections.Generic;
using System.ComponentModel;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace ExControls
{
    /// <summary>
    ///     Provides a generic collection that supports data binding and sorting.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExBindingList<T> : BindingList<T>
    {
        private bool _isSorted;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor _sortProperty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExBindingList{T}" /> class.
        /// </summary>
        public ExBindingList() : this(new List<T>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExBindingList{T}" /> class.
        /// </summary>
        /// <param name="list">
        ///     An <see cref="T:System.Collections.Generic.IList`1" /> of items to be contained in the
        ///     <see cref="T:System.ComponentModel.BindingList`1" />.
        /// </param>
        public ExBindingList(IList<T> list) : base(list)
        {
        }

        /// <summary>
        ///     Gets a value indicating whether the list supports sorting.
        /// </summary>
        protected override bool SupportsSortingCore => Sortable;

        /// <summary>
        ///     Gets a value indicating whether the list is sorted.
        /// </summary>
        protected override bool IsSortedCore => _isSorted;

        /// <summary>
        ///     Gets the direction the list is sorted.
        /// </summary>
        protected override ListSortDirection SortDirectionCore => _sortDirection;

        /// <summary>
        ///     Gets or sets whether collection is sortable.
        /// </summary>
        public bool Sortable { get; set; } = true;

        /// <summary>
        ///     Gets or sets whether to trigger an event when sorting items in a collection.
        /// </summary>
        public bool FireEventOnSort { get; set; } = true;

        /// <summary>
        ///     Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class;
        ///     otherwise, returns <see langword="null" />.
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore => _sortProperty;

        /// <summary>
        ///     Removes any sort applied with ApplySortCore if sorting is implemented
        /// </summary>
        protected override void RemoveSortCore()
        {
            _sortDirection = ListSortDirection.Ascending;
            _sortProperty = null;
            _isSorted = false;
        }

        /// <summary>
        ///     Sorts the items.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            if (!Sortable)
                return;

            _sortProperty = prop;
            _sortDirection = direction;

            if(Items is List<T> list)
                list.Sort(Compare);

            _isSorted = true;

            if (FireEventOnSort)
                //fire an event that the list has been changed.
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        private int Compare(T lhs, T rhs)
        {
            var result = OnComparisonCore(lhs, rhs);
            //invert if descending
            if (_sortDirection == ListSortDirection.Descending)
                result = -result;
            return result;
        }

        private int OnComparisonCore(T lhs, T rhs)
        {
            var lhsValue = lhs == null ? null : _sortProperty.GetValue(lhs);
            var rhsValue = rhs == null ? null : _sortProperty.GetValue(rhs);

            if (lhsValue == null) return rhsValue == null ? 0 : -1; //nulls are equal
            if (rhsValue == null) return 1; //first has value, second doesn't

            return OnComparison(lhsValue, rhsValue);
        }

        /// <summary>
        ///     Compares two property values
        /// </summary>
        /// <param name="left">left item</param>
        /// <param name="right">right item</param>
        /// <returns></returns>
        protected int OnComparison(object left, object right)
        {
            if (long.TryParse(left.ToString(), out var ln) && long.TryParse(right.ToString(), out var rn)) return ln.CompareTo(rn);

            if (left is IComparable comparable) return comparable.CompareTo(right);
            if (left.Equals(right)) return 0; //both are the same

            //not comparable, compare ToString
            return string.Compare(left.ToString(), right.ToString(), StringComparison.CurrentCulture);
        }

        /// <summary>
        ///     Adds the elements of the specified collection to the end of the <see cref="ExBindingList{T}" />.
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<T> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            var raise = RaiseListChangedEvents;
            var newIndex = Items.Count;
            RaiseListChangedEvents = false;
            var newItemsCount = 0;

            if (items is ICollection<T> collection && Items is List<T> list)
            {
                var requiredCapacity = Count + collection.Count;
                if (requiredCapacity > list.Capacity)
                    list.Capacity = requiredCapacity;
            }

            foreach (var item in items)
            {
                Add(item);
                newItemsCount++;
            }

            RaiseListChangedEvents = raise;
            if (raise && newItemsCount != 0) OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, newIndex));
        }
    }
}