using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
namespace GraniteHouse.Models.ViewModel
{
    public class HomeViewModel
    {
       public List<ProductTypes> ProductTypes { get; set; }
        
       public List<Category> Categories { get; set; }

       public List<Brand> Brands { get; set; }

       public PagingInfo PagingInfo { get; set; }

        public List<Products> Products { get; set; }
    }
}
