using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalesOrders.Data;
using SalesOrders.Domains.DTOs;
using SalesOrders.Domains.Models;
using SalesOrders.Services.IServices;

namespace SalesOrders.Services.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SalesOrderService> _logger;
        private readonly ISignalRService _notificationSender;

        public SalesOrderService(AppDbContext context, ILogger<SalesOrderService> logger, ISignalRService signalRService)
        {
            _context = context;
            _logger = logger;
            _notificationSender = signalRService;
        }

        public async Task<ResponseMessage<IEnumerable<SalesOrder>>> GetSalesOrders()
        {
            try
            {
                return ResponseMessage<IEnumerable<SalesOrder>>.Ok("Successful", await _context.SalesOrders.Include(so => so.Product).ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching sales orders");
                throw;
            }
        }

        public async Task<ResponseMessage<SalesOrder>> GetSalesOrderByIdAsync(long id)
        {
            try
            {
                var salesOrder = await _context.SalesOrders.Include(so => so.Product).FirstOrDefaultAsync(so => so.Id == id);
                if (salesOrder == null)
                {
                    _logger.LogWarning("Sales order with ID {SalesOrderId} not found", id);
                    return ResponseMessage<SalesOrder>.Fail("Invalid sales order id");
                }

                return ResponseMessage<SalesOrder>.Ok("Successful", salesOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching sales order with ID: {SalesOrderId}", id);
                throw;
            }
        }

        public async Task<ResponseMessage<SalesOrder>> CreateSalesOrder(long productId, long customerId, int quantity)
        {
            try
            {
                var salesOrder = new SalesOrder
                {
                    CustomerId = customerId,
                    ProductId = productId,
                    Quantity = quantity
                };

                //check product available
                bool isProductValid = await _context.Products.AnyAsync(x => x.Id == productId);
                if (!isProductValid)
                {
                    return ResponseMessage<SalesOrder>.Fail("Product does not exist");
                }
                _context.SalesOrders.Add(salesOrder);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Sales order created successfully with ID: {SalesOrderId}", salesOrder.Id);

                await _notificationSender.NotifyNewSalesOrder(salesOrder.Id);
                return ResponseMessage<SalesOrder>.Ok("Sales order created successfully", salesOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a sales order");
                throw;
            }
        }

        public async Task DeleteSalesOrder(long id)
        {
            try
            {
                var salesOrder = await _context.SalesOrders.FindAsync(id);
                if (salesOrder == null)
                {
                    _logger.LogWarning("Sales order with ID {SalesOrderId} not found for deletion", id);
                    return;
                }

                _context.SalesOrders.Remove(salesOrder);
                await _context.SaveChangesAsync();
                await _notificationSender.NotifyDeletedSalesOrder(salesOrder.Id);

                _logger.LogInformation("Sales order with ID {SalesOrderId} deleted successfully", id);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a sales order with ID: {SalesOrderId}", id);
                throw;
            }
        }

        public async Task<ResponseMessage<IEnumerable<object>>> GetTopProductsByQuantity()
        {
            try
            {
                var topProducts = await _context.SalesOrders
                    .GroupBy(so => new { so.ProductId, so.Product.Name })
                    .Select(g => new
                    {
                        g.Key.ProductId,
                        ProductName = g.Key.Name,
                        TotalQuantity = g.Sum(so => so.Quantity)
                    })
                    .OrderByDescending(g => g.TotalQuantity)
                    .ToListAsync();

                return ResponseMessage<IEnumerable<object>>.Ok("Successful", topProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching top products by quantity");
                throw;
            }
        }
    }
}
