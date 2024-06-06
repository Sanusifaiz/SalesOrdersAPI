namespace SalesOrders.Services.IServices
{
    public interface ISignalRService
    {
        Task SendMessage(string user, string message);
        Task NotifyNewSalesOrder(long salesOrderId);
        Task NotifyDeletedSalesOrder(long salesOrderId);
    }
}