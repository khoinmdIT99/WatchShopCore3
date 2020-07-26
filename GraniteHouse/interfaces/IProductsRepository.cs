using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Models;
namespace GraniteHouse.interfaces
{
    public interface IProductsRepository
    {
        IEnumerable<Products> Products { get;  }
        Products GetProductsById(int productId);
    }
}
