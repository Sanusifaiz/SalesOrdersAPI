using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Domains.DTOs
{
    public record CreateProductDTO
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }
}
