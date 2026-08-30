using System;

namespace DrinkApi.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        // FK to Drink
        public int DrinkId { get; set; }
        public Drink? Drink { get; set; }

      
        public string DrinkName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

// - Drink? Drink (the navigation property + DrinkId) exists to join/reference back to the current drink data 
// (e.g. to look up its image, type, or sweetness) or for use when creating an order.

// - DrinkName, UnitPrice, and TotalPrice, on the other hand, are denormalization — deliberately copying 
// and storing that data redundantly on Order to "lock in" the state at the time the order was placed, 
// so it doesn't change if the Drink record is edited later. This is a standard pattern for any system involving transactions (orders/invoices).