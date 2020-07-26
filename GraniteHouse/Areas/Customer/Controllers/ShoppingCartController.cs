using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Extensions;
using GraniteHouse.Models;
using GraniteHouse.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraniteHouse.interfaces;

namespace GraniteHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {

        
        private readonly IProductsRepository _productsRepository;

        private readonly ShoppingCart _shoppingCart;
        private readonly ApplicationDbContext _db;
   

        public ShoppingCartController(IProductsRepository productsRepository,ShoppingCart shoppingCart, ApplicationDbContext db)
        {
            _productsRepository = productsRepository;
            _shoppingCart = shoppingCart;

            _db = db;
           

        }
        public IActionResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            var sCVM = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart
            };
            if (items.Count == 0||items==null)
            {
                return View("EmptyBag");
            }
            return View(sCVM);
        }

        public IActionResult AddToShoppingCart(int productId,string strUrl)
        {
            var selectedProduct = _productsRepository.Products.FirstOrDefault(p => p.Id == productId);
            if(selectedProduct.Available==false)
            {
                return View("OutOfStock");
            }
            else
            {
                if (selectedProduct != null)
                {
                    _shoppingCart.AddToCart(selectedProduct, 1);
                }
            }
            return Redirect(strUrl);
        }

        public IActionResult AddToShoppingCartDetailPages(int productId)
        {
            var selectedProduct = _productsRepository.Products.FirstOrDefault(p => p.Id == productId);
           
            if (selectedProduct != null)
            {
                _shoppingCart.AddToCart(selectedProduct, 1);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromShoppingCart(int productId)
        {
            var selectedProduct = _productsRepository.Products.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct != null)
            {
                _shoppingCart.RemoveFromCart(selectedProduct);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            if (items != null)
            {
                _shoppingCart.ClearCart();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> FixAmount(int productId, int newAmount)
        {
            List<ShoppingCartItem> items = _shoppingCart.GetShoppingCartItems();
            ShoppingCartItem selectedProduct = items.FirstOrDefault(p=>p.Products.Id==productId);

            if (selectedProduct != null)
            {
                if (newAmount < 1 || newAmount > selectedProduct.Products.Quantity)
                {

                }
                else
                {
                    selectedProduct.Amount = newAmount;
                    
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
      
    }
}