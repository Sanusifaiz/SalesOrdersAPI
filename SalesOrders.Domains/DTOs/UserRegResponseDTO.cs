using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Domains.DTOs
{
    public record UserRegResponseDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
