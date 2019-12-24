using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigmaTestTask.Models
{
    public class ProductInCart
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}