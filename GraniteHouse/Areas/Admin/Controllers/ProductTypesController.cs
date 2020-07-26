using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using GraniteHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraniteHouse.Models.ViewModel;
namespace GraniteHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ProductTypesController : Controller
    {

        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ProductTypesViewModel ProductTypesVM { get; set; }

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
            ProductTypesVM = new ProductTypesViewModel()
            {
                Categories = _db.Categories.ToList(),
                ProductTypes=new Models.ProductTypes()
            };
        }

        public async Task<IActionResult> Index()
        {
            var productTypes = _db.ProductTypes.Include(m => m.Category);
            return View(await productTypes.ToListAsync());
        }

        //GET Create Action Method
        public IActionResult Create()
        {
            return View(ProductTypesVM);
        }

        //POST Create action Method
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if(!ModelState.IsValid)
            {
                return View(ProductTypesVM);
            }
            _db.ProductTypes.Add(ProductTypesVM.ProductTypes);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            ProductTypesVM.ProductTypes = await _db.ProductTypes.Include(m=>m.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductTypesVM.ProductTypes == null)
            {
                return NotFound();
            }

            return View(ProductTypesVM);
        }

        //POST Edit action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {         
            if (ModelState.IsValid)
            {
                var productTypesFromDb = _db.ProductTypes.Where(m => m.Id == ProductTypesVM.ProductTypes.Id).FirstOrDefault();
                productTypesFromDb.Name = ProductTypesVM.ProductTypes.Name;
                productTypesFromDb.CategoryId = ProductTypesVM.ProductTypes.CategoryId;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ProductTypesVM);
        }

        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductTypesVM.ProductTypes = await _db.ProductTypes.Include(m=>m.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductTypesVM.ProductTypes == null)
            {
                return NotFound();
            }

            return View(ProductTypesVM);
        }


        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductTypesVM.ProductTypes = await _db.ProductTypes.Include(m=>m.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductTypesVM.ProductTypes == null)
            {
                return NotFound();
            }

            return View(ProductTypesVM);
        }

        //POST Delete action Method
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTypes = await _db.ProductTypes.FindAsync(id);
            _db.ProductTypes.Remove(productTypes);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}