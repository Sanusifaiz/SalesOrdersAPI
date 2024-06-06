

namespace SalesOrders.Domains.DTOs
{
    public record CreateSalesOrderDTO
    {
        public required long ProductId { get; set; }
        public required long CustomerId { get; set; }
        public required int Quantity { get; set; }
    }
}
