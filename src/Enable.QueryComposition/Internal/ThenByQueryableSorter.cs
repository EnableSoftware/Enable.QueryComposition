using System;
using System.Linq;
using System.Linq.Expressions;

namespace Enable.QueryComposition.Internal
{
    internal class ThenByQueryableSorter<TSource, TKey> : IQueryableSorter<TSource>
    {
        private readonly IQueryableSorter<TSource> _previousSorter;
        private readonly Expression<Func<TSource, TKey>> _keySelector;
        private readonly SortDirection _direction;

        public ThenByQueryableSorter(
            IQueryableSorter<TSource> previousSorter,
            Expression<Func<TSource, TKey>> keySelector,
            SortDirection direction)
        {
            _previousSorter = previousSorter;
            _keySelector = keySelector;
            _direction = direction;
        }

        public IOrderedQueryable<TSource> Sort(IQueryable<TSource> query)
        {
            var orderedQuery = _previousSorter.Sort(query);

            if (_direction == SortDirection.Ascending)
            {
                return Queryable.ThenBy(orderedQuery, _keySelector);
            }

            return Queryable.ThenByDescending(orderedQuery, _keySelector);
        }
    }
}
