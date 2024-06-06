using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Domains.DTOs
{
    public record UserRegRequestDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
