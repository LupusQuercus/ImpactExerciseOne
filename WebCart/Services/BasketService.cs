using WebCart.Schemas;
using WebCart.Utils;

namespace WebCart.Services
{
    public class BasketService : IBasketService
    {
        private readonly ImpactAPIService _impactAPIService;
        private readonly HashSet<OrderLine> _orderLines;

        public BasketService(ImpactAPIService impactAPIService)
        {
            _impactAPIService = impactAPIService;
            _orderLines = new HashSet<OrderLine>(new OrderLineComparer());
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest login)
        {
            return await _impactAPIService.LoginAsync(login);
        }

        public async Task<IEnumerable<ProductResponse>?> GetTopRankedProductsAsync(string? token, int top = 100)
        {
            var products = await _impactAPIService.GetAllProductsAsync(token);
            return products?.OrderByDescending(x => x.Stars).Take(top);
        }

        public async Task<IEnumerable<ProductResponse>?> GetPaginatedProductsAsync(string? token, int page, int pageSize)
        {
            if (pageSize > 1000)
            {
                pageSize = 1000;
            }
            var products = await _impactAPIService.GetAllProductsAsync(token);
            return products?.OrderBy(x => x.Price).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<ProductResponse>?> GetCheapestProductsAsync(string? token, int cheapest = 10)
        {
            var products = await _impactAPIService.GetAllProductsAsync(token);
            return products?.OrderBy(x => x.Price).Take(cheapest);
        }

        public OrderLine AddProduct(OrderLine orderLine)
        {
            _orderLines.Add(orderLine);
            return orderLine;
        }

        public bool RemoveProduct(int productId)
        {
            var orderLine = _orderLines.FirstOrDefault(x => x.ProductId == productId);
            return orderLine != null && _orderLines.Remove(orderLine);
        }

        public bool ChangeProductQuantity(int productId, int quantity)
        {
            var orderLine = _orderLines.FirstOrDefault(x => x.ProductId == productId);
            if (orderLine != null)
            {
                orderLine.Quantity = quantity;
                orderLine.TotalPrice = orderLine.ProductUnitPrice * orderLine.Quantity;
                return true;
            }
            return false;
        }

        public async Task<OrderResponse?> CreateOrderAsync(CreateOrder order)
        {
            //Prepare Request
            var orderLines = new List<OrderLine>();
            orderLines.AddRange(_orderLines);

            var request = new CreateOrderRequest
            {
                UserEmail = order.UserEmail,
                TotalAmount = order.TotalAmount,
                OrderLines = orderLines
            };

            //Request
            var orderResponse = await _impactAPIService.CreateOrderAsync(order.Token, request);
            if (orderResponse != null && !string.IsNullOrWhiteSpace(orderResponse.OrderId))
            {
                //clear for next order
                _orderLines.Clear();
            }

            return orderResponse;
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(string? token, string? orderId)
        {
            return await _impactAPIService.GetOrderByIdAsync(token, orderId);
        }
    }
}
