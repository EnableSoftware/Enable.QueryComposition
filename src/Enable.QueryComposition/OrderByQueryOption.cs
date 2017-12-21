using System.Collections.Generic;

namespace Enable.QueryComposition
{
    public class OrderByQueryOption
    {
        private readonly IList<OrderByNode> _orderByNodes;

        public OrderByQueryOption(IList<OrderByNode> orderByNodes)
        {
            // TODO Make this collection readonly. Add null checks.
            _orderByNodes = orderByNodes;
        }

        public IList<OrderByNode> OrderbyNodes
        {
            get
            {
                return _orderByNodes;
            }
        }
    }
}
