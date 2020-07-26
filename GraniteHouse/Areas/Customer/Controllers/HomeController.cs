using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraniteHouse.Models;
using GraniteHouse.Models.ViewModel;
using GraniteHouse.Data;
using Microsoft.EntityFrameworkCore;
using GraniteHouse.Extensions;
using Microsoft.AspNetCore.Authorization;
using GraniteHouse.Utility;
using System.Text;
using X.PagedList;
namespace GraniteHouse.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.ProductByDiscount = _db.Products.Where(x=>x.Discount!=0).OrderByDescending(x => x.Discount).ToList();
            ViewBag.ProductNew = _db.Products.OrderByDescending(x => x.CreatedDate).ToList();
           
            return View();
        }
        

        public async Task<IActionResult> Details(int id)
        {
            
            var product = await _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTags).Include(m => m.Brand).Where(m=>m.Id==id).FirstOrDefaultAsync();
            return View(product);
        }
        public IActionResult ProductsByProductTypes(int id,int? page)
        {
            int PageSize = 12;
            int PageNumber = page ?? 1;      
            var pt = _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTags).Include(m => m.Brand).Where(p=>p.ProductTypeId==id).OrderByDescending(p=>p.CreatedDate).ToPagedList(PageNumber, PageSize);
            if (pt.Count == 0)
            {
                ViewBag.ProductTypes = "No result";
            }
            else
            {
                ViewBag.ProductTypes = _db.ProductTypes.SingleOrDefault(p => p.Id == id).Name + " (" + pt.Count + ")";
            }
            return View(pt);
        }
        public IActionResult ProductsByBrands(int id, int? page)
        {
            int PageSize = 12;
            int PageNumber = page ?? 1;
            var br = _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTags).Include(m => m.Brand).Where(p => p.BrandId == id).OrderByDescending(p => p.CreatedDate).ToPagedList(PageNumber, PageSize);          
            if(br.Count==0)
            {
                ViewBag.Brand = "No result";
            }
            else
            {
                ViewBag.Brand = _db.Brands.SingleOrDefault(p => p.Id == id).Name +" ("+br.Count+")";
            }
            
            return View(br);
        }
        public IActionResult ProductsByCategory(int id, int? page)
        {
            int PageSize = 12;
            int PageNumber = page ?? 1;
            var cat = _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTags).Include(m => m.Brand).Where(a => a.ProductTypes.CategoryId == id).ToPagedList(PageNumber, PageSize);
            ViewBag.Category = _db.Categories.SingleOrDefault(p => p.Id == id).Name;
            if(cat.Count==0)
            {
                ViewBag.Category= "No result";

            }
            else
            {
                ViewBag.Category = _db.Categories.SingleOrDefault(p => p.Id == id).Name + " ("+cat.Count+")";
            }
            return View(cat.ToPagedList(PageNumber, PageSize));
        }
        public IActionResult ProductBySearch(int? page, string name=null)
        {
           
            if (name != null)
            {
                int PageSize = 12;
                int PageNumber = page ?? 1;
                var proByName = _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTags).Include(m => m.Brand).Where(p => p.Name.ToLower().Contains(name.ToLower())).OrderByDescending(p => p.Name).ToPagedList(PageNumber, PageSize);
                if(proByName.Count==0)
                {
                    ViewBag.name = name;
                    ViewBag.Search = "No results for: " + name;
                    
                }
                else
                {
                    ViewBag.name = name;
                    ViewBag.Search = "Search for: " + name + " ("+proByName.Count+")";

                }
                return View(proByName);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
           
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
