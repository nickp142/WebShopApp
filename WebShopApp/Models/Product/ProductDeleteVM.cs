using System.ComponentModel.DataAnnotations;
using WebShopApp.Models.Brand;
using WebShopApp.Models.Category;

namespace WebShopApp.Models.Product
{
    public class ProductDeleteVM
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }


        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public string? BrandName { get; set; }



        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }


        [Display(Name = "Picture")]
        public string? Picture { get; set; }


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