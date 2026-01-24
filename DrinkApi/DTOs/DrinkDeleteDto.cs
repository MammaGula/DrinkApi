using System.ComponentModel.DataAnnotations;

namespace DrinkApi.DTOs
{
    public class DrinkDeleteDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(200)]
        public string? Reason { get; set; }
    }
}
