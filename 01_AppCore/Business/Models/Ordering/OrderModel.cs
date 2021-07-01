using System;
namespace _01_AppCore.Business.Models.Ordering
{
    public class OrderModel
    {
        public string Expression { get; set; }
        public bool DirectionAscending { get; set; } = true;
    }
}
