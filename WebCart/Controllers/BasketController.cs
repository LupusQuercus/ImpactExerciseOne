using Microsoft.AspNetCore.Mvc;
using WebCart.Schemas;
using WebCart.Services;

namespace WebCart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _service;

        public BasketController(IBasketService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<LoginResponse?> LoginAsync([FromBody] LoginRequest login)
        {
            return await _service.LoginAsync(login);
        }

        [HttpGet]
        [Route("GetTopRankedProducts")]
        public async Task<IEnumerable<ProductResponse>?> GetTopRankedProductsAsync([FromQuery] string? token)
        {
            return await _service.GetTopRankedProductsAsync(token);
        }

        [HttpGet]
        [Route("GetPaginatedProducts")]
        public async Task<IEnumerable<ProductResponse>?> GetPaginatedProductsAsync(
            [FromQuery] int page
            , [FromQuery] int pageSize
            , [FromQuery] string? token)
        {
            return await _service.GetPaginatedProductsAsync(token, page: page, pageSize: pageSize);
        }

        [HttpGet]
        [Route("GetCheapestProducts")]
        public async Task<IEnumerable<ProductResponse>?> GetCheapestProductsAsync([FromQuery] string? token)
        {
            return await _service.GetCheapestProductsAsync(token);
        }

        [HttpPost]
        [Route("AddProduct")]
        public OrderLine AddProduct([FromBody] OrderLine newItem)
        {
            return _service.AddProduct(newItem);
        }

        [HttpDelete]
        [Route("RemoveProduct/{productId}")]
        public bool RemoveProduct([FromRoute] int productId)
        {
            return _service.RemoveProduct(productId);
        }

        [HttpPut]
        [Route("ChangeProductQuantity/{productId}")]
        public bool ChangeProductQuantity([FromRoute] int productId, [FromQuery] int quantity)
        {
            return _service.ChangeProductQuantity(productId, quantity);
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<OrderResponse?> CreateOrderAsync([FromBody] CreateOrder order)
        {
            return await _service.CreateOrderAsync(order);
        }

        [HttpGet]
        [Route("GetOrder/{orderId}")]
        public async Task<OrderResponse?> GetOrderByIdAsync([FromRoute] string? orderId, [FromQuery] string? token)
        {
            return await _service.GetOrderByIdAsync(token, orderId);
        }
    }
}
