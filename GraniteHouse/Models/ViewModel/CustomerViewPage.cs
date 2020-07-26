using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.Models.ViewModel
{
    public class CustomerViewPage
    {
        public List<Customer> Customers { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
