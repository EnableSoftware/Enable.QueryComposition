using System.Collections.Generic;
using System.Linq;
using Enable.QueryComposition;
using Xunit;

namespace Enable.QueryCompositionTests
{
    public class OrderByQueryOptionExtensionsTests
    {
        [Fact]
        public void CanOrderByAscending()
        {
            // Arrange
            var query = new List<Product>
            {
                new Product { Id = 2 },
                new Product { Id = 1 },
                new Product { Id = 4 },
                new Product { Id = 3 }
            }
            .AsQueryable();

            var orderByNodes = new List<OrderByNode>
            {
                new OrderByNode(nameof(Product.Id))
            };

            var options = new OrderByQueryOption(orderByNodes);

            // Act
            var results = options.ApplyTo(query).ToList();

            // Assert
            Assert.Collection(
                results,
                o => Assert.Equal(1, o.Id),
                o => Assert.Equal(2, o.Id),
                o => Assert.Equal(3, o.Id),
                o => Assert.Equal(4, o.Id));
        }

        [Fact]
        public void CanOrderByDescending()
        {
            // Arrange
            var query = new List<Product>
            {
                new Product { Id = 2 },
                new Product { Id = 1 },
                new Product { Id = 4 },
                new Product { Id = 3 }
            }
            .AsQueryable();

            var orderByNodes = new List<OrderByNode>
            {
                new OrderByNode(nameof(Product.Id), OrderByDirection.Descending)
            };

            var options = new OrderByQueryOption(orderByNodes);

            // Act
            var results = options.ApplyTo(query).ToList();

            // Assert
            Assert.Collection(
                results,
                o => Assert.Equal(4, o.Id),
                o => Assert.Equal(3, o.Id),
                o => Assert.Equal(2, o.Id),
                o => Assert.Equal(1, o.Id));
        }

        [Fact]
        public void CanOrderByChildPropertyAscending()
        {
            // Arrange
            var query = new List<Product>
            {
                new Product { Category = new Category { Description = "b" } },
                new Product { Category = new Category { Description = "a" } },
                new Product { Category = new Category { Description = "d" } },
                new Product { Category = new Category { Description = "c" } }
            }
            .AsQueryable();

            var sortPropertyPath = string.Concat(
                nameof(Product.Category),
                ".",
                nameof(Category.Description));

            var orderByNodes = new List<OrderByNode>
            {
                new OrderByNode(sortPropertyPath)
            };

            var options = new OrderByQueryOption(orderByNodes);

            // Act
            var results = options.ApplyTo(query).ToList();

            // Assert
            Assert.Collection(
                results,
                o => Assert.Equal("a", o.Category.Description),
                o => Assert.Equal("b", o.Category.Description),
                o => Assert.Equal("c", o.Category.Description),
                o => Assert.Equal("d", o.Category.Description));
        }

        [Fact]
        public void CanOrderByChildPropertyDescending()
        {
            // Arrange
            var query = new List<Product>
            {
                new Product { Category = new Category { Description = "b" } },
                new Product { Category = new Category { Description = "a" } },
                new Product { Category = new Category { Description = "d" } },
                new Product { Category = new Category { Description = "c" } }
            }
            .AsQueryable();

            var sortPropertyPath = string.Concat(
                nameof(Product.Category),
                ".",
                nameof(Category.Description));

            var orderByNodes = new List<OrderByNode>
            {
                new OrderByNode(sortPropertyPath, OrderByDirection.Descending)
            };

            var options = new OrderByQueryOption(orderByNodes);

            // Act
            var results = options.ApplyTo(query).ToList();

            // Assert
            Assert.Collection(
                results,
                o => Assert.Equal("d", o.Category.Description),
                o => Assert.Equal("c", o.Category.Description),
                o => Assert.Equal("b", o.Category.Description),
                o => Assert.Equal("a", o.Category.Description));
        }

        [Fact]
        public void CanOrderByMultipleProperties()
        {
            // Arrange
            var query = new List<Product>
            {
                new Product { Id = 2, Price = 1.23m },
                new Product { Id = 4, Price = 5.23m },
                new Product { Id = 1, Price = 1.23m },
                new Product { Id = 3, Price = 0.23m }
            }
            .AsQueryable();

            var orderByNodes = new List<OrderByNode>
            {
                new OrderByNode(nameof(Product.Price), OrderByDirection.Descending),
                new OrderByNode(nameof(Product.Id))
            };

            var options = new OrderByQueryOption(orderByNodes);

            // Act
            var results = options.ApplyTo(query).ToList();

            // Assert
            Assert.Collection(
                results,
                o => Assert.Equal(4, o.Id),
                o => Assert.Equal(1, o.Id),
                o => Assert.Equal(2, o.Id),
                o => Assert.Equal(3, o.Id));
        }

        [Fact]
        public void OrderByIsCaseInsensitive()
        {
            // Arrange
            var query = new List<Product>
            {
                new Product { Id = 2 },
                new Product { Id = 1 },
                new Product { Id = 4 },
                new Product { Id = 3 }
            }
            .AsQueryable();

            var orderByNodes = new List<OrderByNode>
            {
                new OrderByNode("iD")
            };

            var options = new OrderByQueryOption(orderByNodes);

            // Act
            var results = options.ApplyTo(query).ToList();

            // Assert
            Assert.Collection(
                results,
                o => Assert.Equal(1, o.Id),
                o => Assert.Equal(2, o.Id),
                o => Assert.Equal(3, o.Id),
                o => Assert.Equal(4, o.Id));
        }

        private class Product
        {
            public int Id { get; set; }

            public decimal Price { get; set; }

            public Category Category { get; set; }
        }

        private class Category
        {
            public string Description { get; set; }
        }
    }
}
