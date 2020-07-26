using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace GraniteHouse.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Products Products { get; set; }
        public int Amount { get; set; }
        public string ShoppingCartId { get; set; }
    }
}
