using SalesOrders.Domains.DTOs;
using SalesOrders.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Services.IServices
{
    public interface ISalesOrderService
    {
        Task<ResponseMessage<IEnumerable<SalesOrder>>> GetSalesOrders();
        Task<ResponseMessage<SalesOrder>> GetSalesOrderByIdAsync(long id);
        Task<ResponseMessage<SalesOrder>> CreateSalesOrder(long productId, long customerId, int quantity);
        Task DeleteSalesOrder(long id);
        Task<ResponseMessage<IEnumerable<object>>> GetTopProductsByQuantity();
    }
}
