using System;
using System.Linq;
using System.Linq.Expressions;

namespace Enable.QueryComposition.Internal
{
    internal class OrderByQueryableSorter<TSource, TKey> : IQueryableSorter<TSource>
    {
        private readonly Expression<Func<TSource, TKey>> _keySelector;
        private readonly SortDirection _direction;

        public OrderByQueryableSorter(
            Expression<Func<TSource, TKey>> keySelector,
            SortDirection direction)
        {
            _keySelector = keySelector;
            _direction = direction;
        }

        public IOrderedQueryable<TSource> Sort(IQueryable<TSource> query)
        {
            if (_direction == SortDirection.Ascending)
            {
                return Queryable.OrderBy(query, _keySelector);
            }

            return Queryable.OrderByDescending(query, _keySelector);
        }
    }
}
