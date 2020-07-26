using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.Models.ViewModel
{
    public class ProductTypesViewModel
    {
        public ProductTypes ProductTypes { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
