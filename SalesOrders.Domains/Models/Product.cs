using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Domains.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }
}
