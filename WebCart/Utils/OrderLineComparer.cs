using System.Diagnostics.CodeAnalysis;
using WebCart.Schemas;

namespace WebCart.Utils
{
    public class OrderLineComparer : IEqualityComparer<OrderLine>
    {
        public bool Equals(OrderLine? x, OrderLine? y)
        {
            return x?.ProductId == y?.ProductId;
        }

        public int GetHashCode([DisallowNull] OrderLine obj)
        {
            return obj.ProductId.GetHashCode();
        }
    }
}
