using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SalesOrders.Data;
using SalesOrders.Domains.Models;
using SalesOrders.Services.IServices;
using SalesOrders.Services.Services;
using Xunit;

namespace SalesManagerApiTest
{
    public class SalesManagerApiTests
    {
        private readonly ProductService _productService;
        private readonly SalesOrderService _salesService;
        private readonly AuthService _userService;
        private readonly Mock<ILogger<ProductService>> _productLoggerMock;
        private readonly Mock<ILogger<SalesOrderService>> _salesLoggerMock;
        private readonly Mock<ILogger<AuthService>> _userLoggerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ISignalRService> _signalrMock;
        private readonly AppDbContext _context;

        public SalesManagerApiTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new AppDbContext(options);

            _productLoggerMock = new Mock<ILogger<ProductService>>();
            _salesLoggerMock = new Mock<ILogger<SalesOrderService>>();
            _userLoggerMock = new Mock<ILogger<AuthService>>();
            _configMock = new Mock<IConfiguration>();

            _signalrMock = new Mock<ISignalRService>();
            _productService = new ProductService(_context, _productLoggerMock.Object);
            _salesService = new SalesOrderService(_context, _salesLoggerMock.Object, _signalrMock.Object);
            _userService = new AuthService(_context, _configMock.Object, _userLoggerMock.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Coke", Price = 10.5m },
                new Product { Id = 2, Name = "Fanta", Price = 20.0m },
                new Product { Id = 3, Name = "Pepsi", Price = 15.5m}
            };
            var users = new List<User>
            {
                new User { Id = 1, Username = "ade", PasswordHash = "849e1dd3640a7c68b66a3d487bd2572ce420d8913f74c73490572aa5ece61638" },
                new User { Id = 2, Username = "ayo", PasswordHash = "ayoPassword" }
            };
            var salesOrders = new List<SalesOrder>
            {
                new SalesOrder { Id = 1, ProductId = 1, CustomerId = 1, Quantity = 10 },
                new SalesOrder { Id = 2, ProductId = 2, CustomerId = 2, Quantity = 20 }
            };

            if (!_context.SalesOrders.Any())
            {
                _context.SalesOrders.AddRange(salesOrders);
            }
            if (!_context.Products.Any())
            {
                _context.Products.AddRange(products);
            }
            if (!_context.Users.Any())
            {
                _context.Users.AddRange(users);
            }

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetProductsWithHighestPriceAsync_ReturnsCorrectProducts()
        {
            var products = await _productService.GetHighestPriceProducts(3);

            Assert.Equal(3, products.Data?.Count());
            Assert.Equal(2, products.Data?.First().Id);
        }

        [Fact]
        public async Task GetProductByNameAsync_ReturnsCorrectProduct()
        {
            var product = await _productService.GetProductByName("Coke");

            Assert.NotNull(product);
            Assert.Equal(1, product.Data?.Id);
        }

        [Fact]
        public async Task GetProductByNameAsync_ReturnsNullForNonexistentProduct()
        {
            var product = await _productService.GetProductByName("NonexistentProduct");

            Assert.Null(product.Data);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsTrueForSuccessfulRegistration()
        {
            var result = await _userService.Register("randomName", "newpassword");

            Assert.True(!result.Error);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFalseForExistingUsername()
        {
            var result = await _userService.Register("ade", "newpassword");

            Assert.True(result.Error);
        }

        [Fact]
        public async Task LoginUserAsync_ReturnsNullForInvalidCredentials()
        {
            var user = await _userService.Login("ade", "wrongpassword");

            Assert.Null(user.Data);
        }

        [Fact]
        public async Task GetAllSalesOrdersAsync_ReturnsAllSalesOrders()
        {
            var salesOrders = await _salesService.GetSalesOrders();

            Assert.NotEqual(0, salesOrders.Data?.Count());
        }

        [Fact]
        public async Task GetSalesOrderByIdAsync_ReturnsCorrectSalesOrder()
        {
            var salesOrder = await _salesService.GetSalesOrderByIdAsync(1);

            Assert.NotNull(salesOrder.Data);
            Assert.Equal(1, salesOrder.Data?.Id);
        }

        [Fact]
        public async Task GetSalesOrderByIdAsync_ReturnsNullForNonexistentSalesOrder()
        {
            var salesOrder = await _salesService.GetSalesOrderByIdAsync(99);

            Assert.Null(salesOrder.Data);
        }

        [Fact]
        public async Task CreateSalesOrderAsync_AddsSalesOrderToDatabase()
        {
            var salesOrder = new SalesOrder { Id = 30, ProductId = 3, CustomerId = 3, Quantity = 30 };
            await _salesService.CreateSalesOrder(salesOrder.ProductId, salesOrder.CustomerId, salesOrder.Quantity);

            var addedOrder = await _context.SalesOrders.Where(x => x.ProductId == 3 && x.CustomerId == 3).FirstOrDefaultAsync();
            Assert.NotNull(addedOrder);
        }

        [Fact]
        public async Task DeleteSalesOrderAsync_RemovesSalesOrderFromDatabase()
        {
            await _salesService.DeleteSalesOrder(2);
            var deletedOrder = await _context.SalesOrders.Where(x => x.Id == 2).FirstOrDefaultAsync();

            Assert.Null(deletedOrder);
        }
    }
}