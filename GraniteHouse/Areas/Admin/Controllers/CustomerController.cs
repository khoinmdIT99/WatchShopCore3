using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GraniteHouse.Models.ViewModel;
using GraniteHouse.Models;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private int PageSize = 10;
        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int productPage = 1, string cusPhoneNumber = null)
        {
            
            CustomerViewPage customerViewPage = new CustomerViewPage()
            {
                Customers=new List<Models.Customer>()
            };
            StringBuilder param = new StringBuilder();
            param.Append("/Admin/Customer?productPage=:");
            param.Append("&cusPhoneNumber=");
            if (cusPhoneNumber != null)
            {
                param.Append(cusPhoneNumber);
            }
            customerViewPage.Customers = _db.Customers.ToList();
            if (cusPhoneNumber != null)
            {
                customerViewPage.Customers = customerViewPage.Customers.Where(a => a.CusPhone.ToLower().Contains(cusPhoneNumber.ToLower())).ToList();
            }
            
            
            var count = customerViewPage.Customers.Count;

            customerViewPage.Customers = customerViewPage.Customers
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();


            customerViewPage.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
            return View(customerViewPage);
        }
    }
}