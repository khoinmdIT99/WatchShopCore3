using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.interfaces;
using GraniteHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Respositories
{
    public class ProductRespository:IProductsRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRespository(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Products> Products => _db.Products.Include(s => s.ProductTypes).Include(s => s.SpecialTags).Include(s => s.Brand);
        public Products GetProductsById(int productId) => _db.Products.FirstOrDefault(p => p.Id == productId);
    }
}
