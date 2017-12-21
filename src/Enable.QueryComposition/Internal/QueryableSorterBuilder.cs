using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Enable.QueryComposition.Internal
{
    internal class QueryableSorterBuilder<T>
    {
        private IQueryableSorter<T> _currentSorter;

        public QueryableSorterBuilder<T> OrderByAscending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            if (_currentSorter != null)
            {
                _currentSorter = new ThenByQueryableSorter<T, TKey>(_currentSorter, keySelector, SortDirection.Ascending);
            }
            else
            {
                _currentSorter = new OrderByQueryableSorter<T, TKey>(keySelector, SortDirection.Ascending);
            }

            return this;
        }

        public QueryableSorterBuilder<T> OrderByAscending(string propertyPath)
        {
            _currentSorter = BuildQueryableSorter(_currentSorter, propertyPath, SortDirection.Ascending);
            return this;
        }

        public QueryableSorterBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            if (_currentSorter != null)
            {
                _currentSorter = new ThenByQueryableSorter<T, TKey>(_currentSorter, keySelector, SortDirection.Descending);
            }
            else
            {
                _currentSorter = new OrderByQueryableSorter<T, TKey>(keySelector, SortDirection.Descending);
            }

            return this;
        }

        public QueryableSorterBuilder<T> OrderByDescending(string propertyPath)
        {
            _currentSorter = BuildQueryableSorter(_currentSorter, propertyPath, SortDirection.Descending);
            return this;
        }

        public IQueryableSorter<T> Build()
        {
            // TODO What should we do if `OrderByAscending` or `OrderByDescending` hasn't been called yet?
            // See the related TODO in `OrderByQueryOptionExtensions`.
            // Ideally we'd return a `NullQueryableSorter`, which would perform a no-op on an `IQueryable`.
            // However, a no-op can't take an `IQueryable` and return a `IOrderedQueryable`.
            return _currentSorter;
        }

        private static IQueryableSorter<T> BuildQueryableSorter(IQueryableSorter<T> previousSorter, string propertyPath, SortDirection direction)
        {
            var (keySelector, keyType) = GetPropertyAccessorLambda(propertyPath);

            if (previousSorter == null)
            {
                var sorterType = typeof(OrderByQueryableSorter<,>).MakeGenericType(typeof(T), keyType);

                return (IQueryableSorter<T>)Activator.CreateInstance(sorterType, keySelector, direction);
            }
            else
            {
                var sorterType = typeof(ThenByQueryableSorter<,>).MakeGenericType(typeof(T), keyType);

                return (IQueryableSorter<T>)Activator.CreateInstance(sorterType, previousSorter, keySelector, direction);
            }
        }

        private static (LambdaExpression, Type) GetPropertyAccessorLambda(string propertyPath)
        {
            var declaringType = typeof(T);

            var parameter = Expression.Parameter(declaringType, "λ");

            Expression body = parameter;

            foreach (var propertyName in propertyPath.Split('.'))
            {
                // Ensure that property the property exists.
                var accessor = GetPropertyAccessor(declaringType, propertyName);
                declaringType = accessor.ReturnType;

                body = Expression.Property(body, propertyName);
            }

            return (Expression.Lambda(body, parameter), declaringType);
        }

        private static MethodInfo GetPropertyAccessor(Type declaringType, string propertyName)
        {
            var prop = GetPropertyByName(declaringType, propertyName);

            return GetPropertyGetter(prop);
        }

        private static PropertyInfo GetPropertyByName(Type declaringType, string propertyName)
        {
            // TODO Review the use of `BindingFlags.IgnoreCase`.
            // How does this behave when ignoring case introduces an ambiguity between multiple properties?
            var flags = BindingFlags.IgnoreCase |
                BindingFlags.Instance |
                BindingFlags.Public;

            var property = declaringType.GetProperty(propertyName, flags);

            if (property == null)
            {
                // TODO Rather than using exceptions, can we use `TryGet...` methods.
                var message = $"{declaringType} does not contain a property named '{propertyName}'.";

                throw new InvalidOperationException(message);
            }

            return property;
        }

        private static MethodInfo GetPropertyGetter(PropertyInfo property)
        {
            var propertyAccessor = property.GetGetMethod();

            if (propertyAccessor == null)
            {
                var message = $"The property '{property.Name}' does not contain a getter.";

                throw new InvalidOperationException(message);
            }

            return propertyAccessor;
        }
    }
}
