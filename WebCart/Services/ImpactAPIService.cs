using System.Net.Http.Headers;
using WebCart.Schemas;

namespace WebCart.Services
{
    public class ImpactAPIService
    {
        private readonly HttpClient _httpClient;

        public ImpactAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest login)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Login", login);
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse?>();
            return loginResponse;
        }

        public async Task<IEnumerable<ProductResponse>?> GetAllProductsAsync(string? token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var productResponse = await _httpClient.GetFromJsonAsync<IEnumerable<ProductResponse>?>("/api/GetAllProducts");
            return productResponse;
        }

        public async Task<OrderResponse?> CreateOrderAsync(string? token, CreateOrderRequest request)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("/api/CreateOrder", request);
            var orderResponse = await response.Content.ReadFromJsonAsync<OrderResponse?>();
            return orderResponse;
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(string? token, string? orderId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var orderResponse = await _httpClient.GetFromJsonAsync<OrderResponse?>($"/api/GetOrder/{orderId}");
            return orderResponse;
        }
    }
}
