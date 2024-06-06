using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalesOrders.Data;
using SalesOrders.Domains.DTOs;
using SalesOrders.Domains.Models;
using SalesOrders.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseMessage<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                return ResponseMessage<IEnumerable<Product>>.Ok("Successful", await _context.Products.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products");
                throw;
            }
        }

        public async Task<ResponseMessage<Product>> CreateProduct(string name, decimal price)
        {
            try
            {
                var product = new Product { Name = name, Price = price };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product created successfully: {ProductName}", name);
                return ResponseMessage<Product>.Ok("successful", product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product: {ProductName}", name);
                throw;
            }
        }

        public async Task<ResponseMessage<IEnumerable<Product>>> GetHighestPriceProducts(int number)
        {
            try
            {
                return ResponseMessage<IEnumerable<Product>>.Ok("Successful", _context.Products.AsNoTracking()
                    .AsEnumerable()
                    .OrderByDescending(p => p.Price).Take(number).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching highest price products");
                throw;
            }
        }

        public async Task<ResponseMessage<Product>> GetProductByName(string name) 
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
                if (product == null)
                {
                    _logger.LogWarning("Product with name {ProductName} not found", name);
                    return ResponseMessage<Product>.Fail("Product not found");
                }

                return ResponseMessage<Product>.Ok("Successful", product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching product with name: {ProductName}", name);
                throw;
            }
        }
    }
}
