using System;

namespace DrinkApi.DTOs
{
    // - For sending order details back to the frontend after an order is created
    // - For admin to view order details
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public string DrinkName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}