using System.ComponentModel.DataAnnotations;

namespace DrinkApi.DTOs
{
    // Use in Admin.js: Admin create a new drink
    public class DrinkCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string Type { get; set; } = null!;

        [Range(0, 10)]
        public int Sweetness { get; set; }

        [Range(0.01, 9999)]
        public decimal Price { get; set; }
    }
}