using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopApp.Core.Contracts;
using WebShopApp.Infrastructure.Data;
using WebShopApp.Infrastructure.Data.Entities;

namespace WebShopApp.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public CartService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }


            if (cart.Items.Count == 0 && cart.AppliedPromoDiscountPercent > 0)
            {
                cart.AppliedPromoDiscountPercent = 0m;
                await _context.SaveChangesAsync();
            }


            return cart;
        }

        public async Task AddItemAsync(string userId, int productId, int quantity = 1)
        {
            var cart = await GetCartByUserIdAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            var product = _productService.GetProductById(productId);
            if (product == null) return;

            var priceWithDiscount = product.Price * (1 - product.Discount / 100m);

            if (item == null)
            {
                item = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = priceWithDiscount
                };
                cart.Items.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
        }


        public async Task RemoveItemAsync(string userId, int productId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);


                if (cart.Items.Count == 0)
                {
                    cart.AppliedPromoDiscountPercent = 0m;
                }


                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateQuantityAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartByUserIdAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                if (item.Quantity <= 0)
                {
                    cart.Items.Remove(item);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetTotalAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            var subtotal = cart.Items.Sum(i => i.Price * i.Quantity);
            return subtotal;
        }



        public decimal CalculateTotalWithPromo(Cart cart)
        {
            var subtotal = cart.Items.Sum(i => i.Price * i.Quantity);


            if (cart.Items.Count == 0 || cart.AppliedPromoDiscountPercent == 0)
            {
                return subtotal;
            }

            if (cart.AppliedPromoDiscountPercent > 0)
            {
                subtotal -= subtotal * cart.AppliedPromoDiscountPercent / 100m;
            }

            return subtotal;
        }

        public async Task ResetCartAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);


            cart.Items.Clear();


            cart.AppliedPromoDiscountPercent = 0m;

            await _context.SaveChangesAsync();
        }







    }
}