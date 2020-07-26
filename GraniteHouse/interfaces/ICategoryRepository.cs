using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Models;
namespace GraniteHouse.interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
        IEnumerable<ProductTypes> ProductTypes { get; }
        IEnumerable<Brand> Brands { get; }
        IEnumerable<Products> Products { get; }
    }
}
