using System;

namespace DrinkApi.DTOs
{
    // Short response DTO: backend -> frontend
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