using System.Linq;

namespace Enable.QueryComposition
{
    public static class TakeQueryOptionExtensions
    {
        public static IQueryable<T> ApplyTo<T>(this TakeQueryOption options, IQueryable<T> query)
        {
            if (options.HasValue)
            {
                query = query.Take(options.Value);
            }

            return query;
        }
    }
}
