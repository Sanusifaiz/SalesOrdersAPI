using Microsoft.AspNetCore.Mvc;
using SalesOrders.Domains.DTOs;
using SalesOrders.Services.IServices;

namespace SalesOrdersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<DashboardController> _logger;
        private readonly ISalesOrderService _salesOrder;

        public DashboardController(IProductService productService, ILogger<DashboardController> logger, ISalesOrderService salesOrderService)
        {
            _productService = productService;
            _logger = logger;
            _salesOrder = salesOrderService;
        }


        /// <summary>
        /// Get Products With Highest Quantity Sold Endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet("products-highest-quantity-sold")]
        public async Task<IActionResult> GetProductsWithHighestQuantitySold()
        {
            try
            {
                var products = await _salesOrder.GetTopProductsByQuantity();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting products with the highest quantity sold.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get Products With Highest Price Endpoint
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpGet("products-highest-price")]
        public async Task<IActionResult> GetProductsWithHighestPrice(int number)
        {
            try
            {
                var products = await _productService.GetHighestPriceProducts(number);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting products with the highest price.");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
