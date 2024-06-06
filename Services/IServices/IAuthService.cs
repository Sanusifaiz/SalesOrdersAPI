using SalesOrders.Domains.DTOs;
using SalesOrders.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseMessage<UserRegResponseDTO>> Register(string username, string password);
        Task<ResponseMessage<UserRegResponseDTO>> Login(string username, string password);
    }
}
