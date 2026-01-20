using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopApp.Infrastructure.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public virtual List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal AppliedPromoDiscountPercent { get; set; } = 0m;

    }
}