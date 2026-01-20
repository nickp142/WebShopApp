using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopApp.Infrastructure.Data.Entities
{
    public class Favorite
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int ProductId { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual Product? Product { get; set; }
    }
}