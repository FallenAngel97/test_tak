using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigmaTestTask.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<ProductInCart> ProductsInCart { get; set; } // Product and count
    }
}