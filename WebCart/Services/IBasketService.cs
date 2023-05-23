using WebCart.Schemas;

namespace WebCart.Services
{
    public interface IBasketService
    {
        //Login
        Task<LoginResponse?> LoginAsync(LoginRequest login);

        //Product choice actions
        Task<IEnumerable<ProductResponse>?> GetTopRankedProductsAsync(string? token, int top = 100);
        Task<IEnumerable<ProductResponse>?> GetPaginatedProductsAsync(string? token, int page, int pageSize);
        Task<IEnumerable<ProductResponse>?> GetCheapestProductsAsync(string? token, int cheapest = 10);

        //Basket choice actions
        OrderLine AddProduct(OrderLine orderLine);
        bool RemoveProduct(int productId);
        bool ChangeProductQuantity(int productId, int quantity);

        //Basket actions
        Task<OrderResponse?> CreateOrderAsync(CreateOrder order);
        Task<OrderResponse?> GetOrderByIdAsync(string? token, string? orderId);
    }
}
