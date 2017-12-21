using System.Linq;

namespace Enable.QueryComposition.Internal
{
    internal interface IQueryableSorter<T>
    {
        IOrderedQueryable<T> Sort(IQueryable<T> query);
    }
}
