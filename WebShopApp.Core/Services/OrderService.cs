using Microsoft.AspNetCore.Mvc.Filters;
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
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public OrderService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public bool Create(int productId, string userId, int quantity)
        {
            var product = this._context.Products.SingleOrDefault(x => x.Id == productId);

            if (product == null) return false;


            decimal priceWithDiscount = product.Price;
            if (product.Discount > 0)
            {
                priceWithDiscount = product.Price * (1 - (product.Discount / 100m));
            }

            Order item = new Order
            {
                OrderDate = DateTime.Now,
                ProductId = productId,
                UserId = userId,
                Quantity = quantity,
                Price = product.Price,
                Discount = product.Discount,
                TotalPrice = priceWithDiscount * quantity
            };

            product.Quantity -= quantity;

            this._context.Products.Update(product);
            this._context.Orders.Add(item);

            return _context.SaveChanges() != 0;
        }

        public Order GetOrderById(int orderId)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrders()
        {
            return _context.Orders.OrderByDescending(x => x.OrderDate).ToList();
        }

        public List<Order> GetOrdersByUser(string userId)
        {
            return _context.Orders.Where(x => x.UserId == userId)
                 .OrderByDescending(x => x.OrderDate)
                 .ToList();
        }

        public bool RemoveById(int orderId)
        {
            throw new NotImplementedException();
        }

        public bool Update(int orderId, int productId, string userId, int quantity)
        {
            throw new NotImplementedException();
        }

        public bool CreateFromCartItem(CartItem cartItem, string userId, decimal promoDiscountPercent)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);
            if (product == null) return false;

            //discount zadaden ot create
            decimal productDiscount = product.Discount;


            decimal productPriceAfterDiscount = product.Price * (1 - (productDiscount / 100m));


            decimal finalUnitPrice = productPriceAfterDiscount * (1 - (promoDiscountPercent / 100m));


            decimal totalDiscountToRecord = productDiscount + promoDiscountPercent;

            var order = new Order
            {
                OrderDate = DateTime.Now,
                ProductId = product.Id,
                UserId = userId,
                Quantity = cartItem.Quantity,
                Price = product.Price,

                //discount + promocode %
                Discount = totalDiscountToRecord,


                TotalPrice = finalUnitPrice * cartItem.Quantity
            };

            product.Quantity -= cartItem.Quantity;

            _context.Orders.Add(order);
            _context.Products.Update(product);

            return _context.SaveChanges() > 0;
        }





    }
}