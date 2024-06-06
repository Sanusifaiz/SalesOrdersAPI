using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Domains.Models
{
    public class SalesOrder
    {
        [Key]
        public long Id { get; set; }
        public required long ProductId { get; set; }
        public required long CustomerId { get; set; }
        public required int Quantity { get; set; }
        public virtual Product Product { get; set; }
    }
}
