using SalesOrders.Domains.DTOs;
using SalesOrders.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseMessage<IEnumerable<Product>>> GetProducts();
        Task<ResponseMessage<Product>> CreateProduct(string name, decimal price);
        Task<ResponseMessage<IEnumerable<Product>>> GetHighestPriceProducts(int number);
        Task<ResponseMessage<Product>> GetProductByName(string name);

    }
}
