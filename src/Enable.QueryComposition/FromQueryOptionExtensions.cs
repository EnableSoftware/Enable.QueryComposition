using System.Linq;

namespace Enable.QueryComposition
{
    public static class FromQueryOptionExtensions
    {
        public static IQueryable<T> ApplyTo<T>(this FromQueryOption options, IQueryable<T> query)
        {
            if (options.HasValue)
            {
                query = query.Skip(options.Value);
            }

            return query;
        }
    }
}
