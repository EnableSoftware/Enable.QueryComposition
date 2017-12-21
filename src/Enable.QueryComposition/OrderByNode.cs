using System;

namespace Enable.QueryComposition
{
    public class OrderByNode
    {
        public OrderByNode(string propertyPath)
            : this(propertyPath, OrderByDirection.Ascending)
        {
        }

        public OrderByNode(string propertyPath, OrderByDirection direction)
        {
            if (string.IsNullOrWhiteSpace(propertyPath))
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }

            Direction = direction;
            PropertyPath = propertyPath;
        }

        public OrderByDirection Direction { get; internal set; }

        public string PropertyPath { get; internal set; }
    }
}
