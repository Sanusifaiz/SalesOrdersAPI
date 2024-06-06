using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesOrders.Domains.DTOs;
using SalesOrders.Domains.Models;
using SalesOrders.Services.IServices;
using SalesOrdersAPI.Validations;

namespace SalesOrdersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
           
            var products = await _productService.GetProducts();
            if (products.Error)
            {
                return NotFound(products);
            }
            return Ok(products);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDTO product)
        {
            //validate request
            var validator = new CreateProductRequestValidator().Validate(product);
            if (!validator.IsValid)
            {
                return BadRequest(ResponseMessage<string>.Fail(validator.Errors.First().ErrorMessage));
            }

            var createdProduct = await _productService.CreateProduct(product.Name, product.Price);
            if (createdProduct.Error) 
            {
                return BadRequest(createdProduct);
            }
            return Ok(createdProduct);
        }


        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetProductByName(string name) 
        {
            var product = await _productService.GetProductByName(name);
            if (product.Error)
            {
                return NotFound(product);
            }

            return Ok(product);
        }
    }
}
