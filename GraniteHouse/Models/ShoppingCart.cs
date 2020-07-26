using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace GraniteHouse.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCart(ApplicationDbContext db)
        {
            _db = db;
        }
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public static ShoppingCart GetCart(IServiceProvider services)
        {            
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }
        public void AddToCart(Products products,int amount)
        {
            var shoppingCartItem = _db.ShoppingCartItems.SingleOrDefault(
                s => s.Products.Id == products.Id && s.ShoppingCartId == ShoppingCartId);
            if(shoppingCartItem==null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Products = products,
                    Amount = 1
                };
                _db.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _db.SaveChanges();
        }
        public void RemoveFromCart(Products products)
        {
            var shoppingCartItem = _db.ShoppingCartItems.SingleOrDefault(
               s => s.Products.Id == products.Id && s.ShoppingCartId == ShoppingCartId);
            if(shoppingCartItem!=null)
            {
                _db.ShoppingCartItems.Remove(shoppingCartItem);              
            }
            _db.SaveChanges();

        }
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                (ShoppingCartItems = _db.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Include(s => s.Products).ToList());
        }
        public void ClearCart()
        {
            var cartItems = _db.ShoppingCartItems.Where(cart => cart.ShoppingCartId == ShoppingCartId);
            _db.ShoppingCartItems.RemoveRange(cartItems);
            _db.SaveChanges();
        }
        /*
        public decimal GetshoppingCartTotal()
        {
            var total = _db.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Products.Price * c.Amount).Sum();
            return total;
        }
        */
    }
}
