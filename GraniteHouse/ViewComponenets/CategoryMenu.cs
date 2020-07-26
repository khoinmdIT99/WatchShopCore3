using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraniteHouse.Models;
using GraniteHouse.interfaces;
using GraniteHouse.Models.ViewModel;
namespace GraniteHouse.Components
{
    public class CategoryMenu:ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryMenu(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IViewComponentResult Invoke()
        {
            var categories = _categoryRepository.Categories;
            var productTypes = _categoryRepository.ProductTypes;
            var brand = _categoryRepository.Brands;
            var product = _categoryRepository.Products;
            var homeViewmodel = new HomeViewModel()
            {
                Categories = categories.ToList(),
                ProductTypes = productTypes.ToList(),
                Brands=brand.ToList(),
                Products=product.ToList()
            };

            
            return View(homeViewmodel);
        }
    }
}
