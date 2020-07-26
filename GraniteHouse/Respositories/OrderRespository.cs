using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.interfaces;
using GraniteHouse.Models;
using GraniteHouse.Data;

namespace GraniteHouse.Respositories
{
    public class OrderRespository:IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ShoppingCart _shoppingCart;

        public OrderRespository(ApplicationDbContext db, ShoppingCart shoppingCart)
        {
            _db = db;
            _shoppingCart = shoppingCart;
        }
        public void CreateOrder(Appointments appointments)
        {

            var ShoppingCartItems = _shoppingCart.ShoppingCartItems;
            
            foreach (var items in ShoppingCartItems)
            {
                decimal newPrice = (items.Products.Price - ((items.Products.Price * items.Products.Discount) / 100));
                decimal Total = (items.Products.Price * items.Amount);
                decimal newTotal = (newPrice * items.Amount);
                if(items.Products.Price>newPrice)
                {
                    var orderDetails = new ProductsSelectedForAppointment()
                    {
                        quantity = items.Amount,
                        ProductId = items.Products.Id,
                        AppointmentId = appointments.Id,
                        productName = items.Products.Name,
                        price = newPrice,
                        TotalQuantity = newTotal
                    };
                    _db.ProductsSelectedForAppointment.Add(orderDetails);
                }
                else
                {
                    var orderDetails = new ProductsSelectedForAppointment()
                    {
                        quantity = items.Amount,
                        ProductId = items.Products.Id,
                        AppointmentId = appointments.Id,
                        productName = items.Products.Name,
                        price = items.Products.Price,
                        TotalQuantity = Total
                    };
                    _db.ProductsSelectedForAppointment.Add(orderDetails);
                }
               
            }
            _db.SaveChanges();
        }
    }
}
