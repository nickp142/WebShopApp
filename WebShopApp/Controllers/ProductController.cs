using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using WebShopApp.Core.Contracts;
using WebShopApp.Infrastructure.Data;
using WebShopApp.Infrastructure.Data.Entities;
using WebShopApp.Models.Brand;
using WebShopApp.Models.Product;

namespace WebShopApp.Controllers

{
    [Authorize(Roles = "Administrator")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ApplicationDbContext _context;


        public ProductController(IProductService productService, ICategoryService categoryService,
            IBrandService brandService, ApplicationDbContext context)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._brandService = brandService;
            this._context = context;
        }


        // GET: ProductController
        [AllowAnonymous]
        public ActionResult Index(string searchStringCategoryName, string searchStringBrandName, string sort)
        {
            List<ProductIndexVM> products = _productService.GetProducts(searchStringCategoryName, searchStringBrandName)
                 .Select(product => new ProductIndexVM
                 {
                     Id = product.Id,
                     ProductName = product.ProductName,
                     BrandId = product.BrandId,
                     BrandName = product.Brand.BrandName,
                     CategoryId = product.CategoryId,
                     CategoryName = product.Category.CategoryName,
                     Picture = product.Picture,
                     Quantity = product.Quantity,
                     Price = product.Price,
                     Discount = product.Discount


                 }).ToList();

            switch (sort)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.ProductName).ToList();
                    break;

                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName).ToList();
                    break;

                case "price_asc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;

                case "price_desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
            }


            return this.View(products);
        }

        // GET: ProductController/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            Product item = _productService.GetProductById(id);
            if (item == null)
            {
                return NotFound();
            }
            ProductDetailsVM product = new ProductDetailsVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                BrandId = item.BrandId,
                BrandName = item.Brand.BrandName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };
            return this.View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var product = new ProductCreateVM();
            product.Brands = _brandService.GetBrands()
                .Select(x => new Models.Brand.BrandPairVM()
                {
                    Id = x.Id,
                    Name = x.BrandName
                }).ToList();
            product.Categories = _categoryService.GetCategories()
                .Select(x => new Models.Category.CategoryPairVM()
                {
                    Id = x.Id,
                    Name = x.CategoryName
                }).ToList();
            return View(product);


        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] ProductCreateVM product)
        {
            if (ModelState.IsValid)
            {
                var createdId = _productService.Create(product.ProductName, product.BrandId,
                    product.CategoryId, product.Picture, product.Quantity, product.Price, product.Discount);
                if (createdId)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            ProductEditVM updateProduct = new ProductEditVM()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                BrandId = product.BrandId,
                //BrandName = product.Brand.BrandName,
                CategoryId = product.CategoryId,
                //CategoryName = product.Category.CategoryName,
                Picture = product.Picture,
                Quantity = product.Quantity,
                Price = product.Price,
                Discount = product.Discount
            };
            updateProduct.Brands = _brandService.GetBrands()
                .Select(b => new BrandPairVM()
                {
                    Id = b.Id,
                    Name = b.BrandName

                }).ToList();
            updateProduct.Categories = _categoryService.GetCategories()
                .Select(c => new Models.Category.CategoryPairVM()
                {
                    Id = c.Id,
                    Name = c.CategoryName
                }).ToList();
            return View(updateProduct);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEditVM product)
        {
            if (ModelState.IsValid)
            {
                var updated = _productService.Update(id, product.ProductName,
                    product.BrandId, product.CategoryId, product.Picture,
                    product.Quantity, product.Price, product.Discount);
                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }
            return View(product);
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            Product item = _productService.GetProductById(id);
            if (item == null)
            {
                return NotFound();
            }
            ProductDeleteVM product = new ProductDeleteVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                BrandId = item.BrandId,
                BrandName = item.Brand.BrandName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount


            };
            return View(product);



        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _productService.RemoveById(id);
            if (deleted)
            {
                return this.RedirectToAction("Success");
            }
            else
            {
                return View();
            }

        }

        public IActionResult Success()
        {
            return View();
        }
    }
}