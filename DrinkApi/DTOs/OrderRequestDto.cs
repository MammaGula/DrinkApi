using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace DrinkApi.DTOs
{
    // Use in orders.js to send order creation requests
    // Define the information the customer submits when placing an order.
    // Short request DTO: frontend -> backend
    public class OrderRequestDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Drink name must not exceed 100 characters")]
        public string DrinkName { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }
    }
}