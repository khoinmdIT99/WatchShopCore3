﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.Models.ViewModel
{
    public class ProductsViewPage
    {
        public List<Products> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
