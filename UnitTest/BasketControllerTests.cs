using System.Net.Http.Headers;
using WebCart.Controllers;
using WebCart.Schemas;
using WebCart.Services;

namespace UnitTest
{
    public class BasketControllerTests
    {
        private readonly ImpactAPIService _impactAPIService;
        private readonly IBasketService _basketService;
        private readonly BasketController _controller;
        private readonly HttpClient _httpClient = new();

        public BasketControllerTests()
        {
            _httpClient.BaseAddress = new Uri("https://azfun-impact-code-challenge-api.azurewebsites.net");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _impactAPIService = new ImpactAPIService(_httpClient);
            _basketService = new BasketService(_impactAPIService);
            _controller = new BasketController(_basketService);
        }

        private async Task<LoginResponse?> GetTokenAsync()
        {
            var loginResponse = await _controller.LoginAsync(new LoginRequest { Email = "androslopes@gmail.com" });
            return loginResponse;
        }

        [Fact]
        public async void LoginAsync_ReturnsToken()
        {
            // Act
            var loginResponse = await _controller.LoginAsync(new LoginRequest { Email = "androslopes@gmail.com" });

            // Assert
            Assert.NotNull(loginResponse);
            Assert.True(loginResponse is LoginResponse);
            Assert.NotNull(loginResponse.Token);
        }

        [Fact]
        public async void GetTopRankedProductsAsync_ReturnElements()
        {
            // Arrange
            var loginResponse = await GetTokenAsync();
            var products = await _impactAPIService.GetAllProductsAsync(loginResponse?.Token);

            // Act
            var topProductResponses = await _controller.GetTopRankedProductsAsync(loginResponse?.Token);

            // Assert
            Assert.NotNull(topProductResponses);
            Assert.True(topProductResponses is IEnumerable<ProductResponse>);
            Assert.True(topProductResponses.Count() <= 100);

            Assert.NotNull(products);
            var expectedProducts = products.OrderByDescending(x => x.Stars).Take(100).Select(x => x.Id);
            var actualProducts = topProductResponses.Select(x => x.Id);
            Assert.Equal(expectedProducts, actualProducts);
        }

        [Fact]
        public async void GetPaginatedProductsAsync_ReturnElements()
        {
            // Arrange
            var loginResponse = await GetTokenAsync();
            var products = await _impactAPIService.GetAllProductsAsync(loginResponse?.Token);

            // Act
            var paginatedProductResponses = await _controller.GetPaginatedProductsAsync(page: 1, pageSize: 1001, loginResponse?.Token);

            // Assert
            Assert.NotNull(paginatedProductResponses);
            Assert.True(paginatedProductResponses is IEnumerable<ProductResponse>);
            Assert.True(paginatedProductResponses.Count() <= 1000);

            Assert.NotNull(products);
            var expectedProducts = products.OrderBy(x => x.Price).Skip((1 - 1) * 1000).Take(1000).Select(x => x.Id);
            var actualProducts = paginatedProductResponses.Select(x => x.Id);
            Assert.Equal(expectedProducts, actualProducts);
        }

        [Fact]
        public async void GetCheapestProductsAsync_ReturnElements()
        {
            // Arrange
            var loginResponse = await GetTokenAsync();
            var products = await _impactAPIService.GetAllProductsAsync(loginResponse?.Token);

            // Act
            var cheapestProductResponses = await _controller.GetCheapestProductsAsync(loginResponse?.Token);

            // Assert
            Assert.NotNull(cheapestProductResponses);
            Assert.True(cheapestProductResponses is IEnumerable<ProductResponse>);
            Assert.True(cheapestProductResponses.Count() <= 10);

            Assert.NotNull(products);
            var expectedProducts = products.OrderBy(x => x.Price).Take(10).Select(x => x.Id);
            var actualProducts = cheapestProductResponses.Select(x => x.Id);
            Assert.Equal(expectedProducts, actualProducts);
        }

        [Fact]
        public async void AddProduct_Success()
        {
            // Arrange
            var loginResponse = await GetTokenAsync();
            var products = await _impactAPIService.GetAllProductsAsync(loginResponse?.Token);
            Assert.NotNull(products);
            var orderLine = new OrderLine
            {
                ProductId = products.First().Id,
                ProductName = products.First().Name,
                ProductUnitPrice = products.First().Price,
                ProductSize = products.First().Size.ToString(),
                Quantity = 1,
                TotalPrice = products.First().Price,
            };

            // Act
            var newOrderLine = _controller.AddProduct(orderLine);

            // Assert
            Assert.NotNull(newOrderLine);
            Assert.True(newOrderLine is OrderLine);

            Assert.Equal(orderLine.ProductId, newOrderLine.ProductId);
            Assert.Equal(orderLine.ProductName, newOrderLine.ProductName);
            Assert.Equal(orderLine.ProductUnitPrice, newOrderLine.ProductUnitPrice);
            Assert.Equal(orderLine.ProductSize, newOrderLine.ProductSize);
            Assert.Equal(orderLine.Quantity, newOrderLine.Quantity);
            Assert.Equal(orderLine.TotalPrice, newOrderLine.TotalPrice);
        }

        [Fact]
        public async void RemoveProduct_Success()
        {
            // Arrange
            var loginResponse = await GetTokenAsync();
            var products = await _impactAPIService.GetAllProductsAsync(loginResponse?.Token);
            Assert.NotNull(products);
            var orderLineOne = new OrderLine
            {
                ProductId = products.First().Id,
                ProductName = products.First().Name,
                ProductUnitPrice = products.First().Price,
                ProductSize = products.First().Size.ToString(),
                Quantity = 1,
                TotalPrice = products.First().Price,
            };
            _controller.AddProduct(orderLineOne);

            // Act
            var orderLineOneIsRemoved = _controller.RemoveProduct(orderLineOne.ProductId);

            // Assert
            Assert.True(orderLineOneIsRemoved);
        }

        [Fact]
        public async void ChangeProductQuantity_Success()
        {
            // Arrange
            var loginResponse = await GetTokenAsync();
            var products = await _impactAPIService.GetAllProductsAsync(loginResponse?.Token);
            Assert.NotNull(products);
            var orderLine = new OrderLine
            {
                ProductId = products.First().Id,
                ProductName = products.First().Name,
                ProductUnitPrice = products.First().Price,
                ProductSize = products.First().Size.ToString(),
                Quantity = 1,
                TotalPrice = products.First().Price,
            };
            _controller.AddProduct(orderLine);

            // Act
            var orderLineQuanIsChanged = _controller.ChangeProductQuantity(orderLine.ProductId, quantity: 2);

            // Assert
            Assert.True(orderLineQuanIsChanged);
        }

        [Fact]
        public async void CreateOrderAsync_Success()
        {
            // Arrange
            var loginResponse = await GetTokenAsync();
            var products = await _impactAPIService.GetAllProductsAsync(loginResponse?.Token);
            Assert.NotNull(products);
            var orderLineOne = new OrderLine
            {
                ProductId = products.First().Id,
                ProductName = products.First().Name,
                ProductUnitPrice = products.First().Price,
                ProductSize = products.First().Size.ToString(),
                Quantity = 1,
                TotalPrice = products.First().Price,
            };
            var orderLineTwo = new OrderLine
            {
                ProductId = products.Last().Id,
                ProductName = products.Last().Name,
                ProductUnitPrice = products.Last().Price,
                ProductSize = products.Last().Size.ToString(),
                Quantity = 1,
                TotalPrice = products.Last().Price,
            };
            _controller.AddProduct(orderLineOne);
            _controller.AddProduct(orderLineTwo);

            var createOrder = new CreateOrder
            {
                Token = loginResponse?.Token,
                UserEmail = "androslopes@gmail.com",
                TotalAmount = orderLineOne.TotalPrice + orderLineTwo.TotalPrice
            };

            // Act
            var newOrder = await _controller.CreateOrderAsync(createOrder);

            // Assert
            Assert.NotNull(newOrder);
            Assert.True(newOrder is OrderResponse);
            Assert.False(string.IsNullOrWhiteSpace(newOrder.OrderId));
            Assert.Equal(createOrder.UserEmail, newOrder.UserEmail);
            Assert.Equal(createOrder.TotalAmount, newOrder.TotalAmount);

            Assert.Contains(orderLineOne.ProductId, newOrder.OrderLines?.Select(x => x.ProductId) ?? Enumerable.Empty<int>());
            Assert.Contains(orderLineTwo.ProductId, newOrder.OrderLines?.Select(x => x.ProductId) ?? Enumerable.Empty<int>());
        }

        [Fact]
        public async void GetOrderByIdAsync_Success()
        {
            // Arrange
            var loginResponse = await GetTokenAsync();
            var products = await _impactAPIService.GetAllProductsAsync(loginResponse?.Token);
            Assert.NotNull(products);
            var orderLineOne = new OrderLine
            {
                ProductId = products.First().Id,
                ProductName = products.First().Name,
                ProductUnitPrice = products.First().Price,
                ProductSize = products.First().Size.ToString(),
                Quantity = 1,
                TotalPrice = products.First().Price,
            };
            var orderLineTwo = new OrderLine
            {
                ProductId = products.Last().Id,
                ProductName = products.Last().Name,
                ProductUnitPrice = products.Last().Price,
                ProductSize = products.Last().Size.ToString(),
                Quantity = 1,
                TotalPrice = products.Last().Price,
            };
            _controller.AddProduct(orderLineOne);
            _controller.AddProduct(orderLineTwo);

            var createOrder = new CreateOrder
            {
                Token = loginResponse?.Token,
                UserEmail = "androslopes@gmail.com",
                TotalAmount = orderLineOne.TotalPrice + orderLineTwo.TotalPrice
            };

            var newOrder = await _controller.CreateOrderAsync(createOrder);

            // Act
            var order = await _controller.GetOrderByIdAsync(newOrder?.OrderId, loginResponse?.Token);

            // Assert
            Assert.NotNull(order);
            Assert.True(order is OrderResponse);
            Assert.False(string.IsNullOrWhiteSpace(order.OrderId));
            Assert.Equal(createOrder.UserEmail, order.UserEmail);
            Assert.Equal(createOrder.TotalAmount, order.TotalAmount);

            Assert.Contains(orderLineOne.ProductId, order.OrderLines?.Select(x => x.ProductId) ?? Enumerable.Empty<int>());
            Assert.Contains(orderLineTwo.ProductId, order.OrderLines?.Select(x => x.ProductId) ?? Enumerable.Empty<int>());
        }
    }
}