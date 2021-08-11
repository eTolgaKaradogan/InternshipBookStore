using System;
namespace _04_Business.Models
{
    public class CartModel
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string BookName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
