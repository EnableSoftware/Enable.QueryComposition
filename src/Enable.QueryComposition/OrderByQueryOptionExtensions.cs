using System.Linq;
using Enable.QueryComposition.Internal;

namespace Enable.QueryComposition
{
    public static class OrderByQueryOptionExtensions
    {
        public static IOrderedQueryable<T> ApplyTo<T>(this OrderByQueryOption options, IQueryable<T> query)
        {
            var queryableSorterBuilder = new QueryableSorterBuilder<T>();

            foreach (var node in options.OrderbyNodes)
            {
                if (node.Direction == OrderByDirection.Ascending)
                {
                    queryableSorterBuilder.OrderByAscending(node.PropertyPath);
                }
                else
                {
                    queryableSorterBuilder.OrderByDescending(node.PropertyPath);
                }
            }

            var sorter = queryableSorterBuilder.Build();

            // TODO The next line will fail if we don't have any order by nodes.
            var orderedQueryable = sorter.Sort(query);

            return orderedQueryable;
        }
    }
}
