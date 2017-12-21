using System;
using System.Linq;
using Enable.QueryComposition;
using Xunit;

namespace Enable.QueryCompositionTests
{
    public class FromQueryOptionExtensionsTests
    {
        [Fact]
        public void CanSkipResults()
        {
            // Arrange
            var skipCount = CreateRandomNumber();

            var expectedItemCount = CreateRandomNumber();

            var totalItemCount = skipCount + expectedItemCount;

            var query = Enumerable.Range(0, totalItemCount)
                .Select(o => new TestModel())
                .AsQueryable();

            var sut = new FromQueryOption(skipCount);

            // Act
            var results = sut.ApplyTo(query).ToList();

            // Assert
            Assert.Equal(expectedItemCount, results.Count());
        }

        private static int CreateRandomNumber()
        {
            var rng = new Random();

            return rng.Next(byte.MaxValue);
        }

        private class TestModel
        {
        }
    }
}
