using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Domains.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
