using System;
using System.Collections.Generic;
using System.Linq;

namespace _02_Entities.Entities
{
    public class Cart
    {
        public Cart()
        {
            CartLines = new List<CartLine>();
        }
        public List<CartLine> CartLines { get; set; }

        public decimal TotalPrice
        {
            get { return (decimal)CartLines.Sum(r => r.Book.UnitPrice * r.Quantity); }
        }
    }
}
