using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SalesOrders.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Services.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<SignalRHub> _hubContext;

        public SignalRService(IHubContext<SignalRHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendMessage(string user, string message)
        {
            await _hubContext.Clients.All.SendAsync("Message", user, message);
        }

        public async Task NotifyNewSalesOrder(long salesOrderId)
        {
            await _hubContext.Clients.All.SendAsync("NewSalesOrder", salesOrderId);
        }

        public async Task NotifyDeletedSalesOrder(long salesOrderId)
        {
            await _hubContext.Clients.All.SendAsync("DeletedSalesOrder", salesOrderId);
        }
    }

    public class SignalRHub : Hub
    {

    }
}
