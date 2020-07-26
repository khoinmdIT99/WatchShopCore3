using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.interfaces;
using GraniteHouse.Models;
namespace GraniteHouse.Respositories
{
    public class CategoryRespository:ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRespository(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Category> Categories => _db.Categories;
        public IEnumerable<ProductTypes> ProductTypes => _db.ProductTypes;
        public IEnumerable<Brand> Brands => _db.Brands;
        public IEnumerable<Products> Products => _db.Products;
    }
}
