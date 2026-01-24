namespace DrinkApi.DTOs
{
    public class DrinkCreateDto
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Sweetness { get; set; }
        public decimal Price { get; set; }


    }
}
