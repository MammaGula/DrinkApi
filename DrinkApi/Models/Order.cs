using System;

namespace DrinkApi.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string DrinkName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
