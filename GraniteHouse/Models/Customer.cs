using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.Models
{
    public class Customer
    {
        [Key]
        public string CusPhone { get; set; }

        public string CusName { get; set; }
        
        public string CusEmail { get; set; }

        public string CusAddress { get; set; }
    }
}
