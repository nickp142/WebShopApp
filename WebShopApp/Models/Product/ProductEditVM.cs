using System.ComponentModel.DataAnnotations;
using WebShopApp.Models.Brand;
using WebShopApp.Models.Category;

namespace WebShopApp.Models.Product
{
    public class ProductEditVM
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Product Name")]
        [MaxLength(30)]
        public string? ProductName { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public virtual List<BrandPairVM> Brands { get; set; } = new List<BrandPairVM>();

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual List<CategoryPairVM> Categories { get; set; } = new List<CategoryPairVM>();


        [Display(Name = "Picture")]
        public string? Picture { get; set; }

        [Range(0, 5000)]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        [Display(Name = "Discounted Price")]
        public decimal DiscountedPrice => Price * (1 - Discount / 100m);

    }
}