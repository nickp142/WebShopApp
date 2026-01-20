using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopApp.Infrastructure.Data.Entities
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsActive { get; set; } = true;
    }
}