using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraniteHouse.interfaces;
using GraniteHouse.Respositories;
using GraniteHouse.Models;
using GraniteHouse.Data;
using GraniteHouse.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AppointmentsController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly ApplicationDbContext _db;

        public AppointmentsController(IOrderRepository orderRepository,ShoppingCart shoppingCart, ApplicationDbContext db)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _db = db;
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(Appointments appointments)
        {
            var cart = _shoppingCart.GetShoppingCartItems();
            
            if(cart==null||cart.Count==0)
            {
                ModelState.AddModelError("", "Your Bag is empty, add contents first!");
            }          
            else
            {
                string cusName = HttpContext.Request.Form["cusName"];
                string cusPhone = HttpContext.Request.Form["cusPhone"];
                string cusMail = HttpContext.Request.Form["cusMail"];
                string cusAddress = HttpContext.Request.Form["cusAddress"];
                string note = HttpContext.Request.Form["note"];

                Models.Customer Newcustomer = new Models.Customer();
                var cus = _db.Customers.FirstOrDefault(p => p.CusPhone.Equals(cusPhone));

                if (cus != null)
                {
                    cus.CusName = cusName;
                    cus.CusEmail = cusMail;
                    cus.CusAddress = cusAddress;
                    _db.Customers.Update(cus);
                }
                else
                {
                    Newcustomer.CusPhone = cusPhone;
                    Newcustomer.CusName = cusName;
                    Newcustomer.CusEmail = cusMail;
                    Newcustomer.CusAddress = cusAddress;
                    _db.Customers.Add(Newcustomer);
                }
                appointments.Note = note;
                appointments.CustomerAddress = cusAddress;
                appointments.CustomerEmail = cusMail;
                appointments.CustomerName = cusName;
                appointments.CustomerPhoneNumber = cusPhone;
                appointments.AppointmentCreatedDate = DateTime.Now;
                appointments.AppointmentDate = DateTime.Now;
                appointments.isCompleted = false;
                appointments.isConfirmed = false;
                decimal Total = 0;
                foreach (var items in cart)
                {

                    decimal newPrice = (items.Products.Price - ((items.Products.Price * items.Products.Discount) / 100));


                    if (items.Products.Price > newPrice)
                    {
                        Total += (newPrice * items.Amount);
                    }
                    else
                    {
                        Total += (items.Products.Price * items.Amount);
                    }
                    appointments.TotalAppointment = Total;
                }
                _db.Appointments.Add(appointments);

                if (ModelState.IsValid)
                {
                    _orderRepository.CreateOrder(appointments);
                    _shoppingCart.ClearCart();
                    return RedirectToAction("CheckoutComplete");
                }
                _db.SaveChanges();
                return View(appointments);
            }

            return RedirectToAction("EmptyBag");
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.chekcoutCompleteMessage = " Thanks For Your Order ! ^.^ ";
            return View();
        }
        public IActionResult EmptyBag()
        {
            return View();
        }
    }
}