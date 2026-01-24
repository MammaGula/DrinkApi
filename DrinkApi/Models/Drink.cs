namespace DrinkApi.Models
{
    public class Drink
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Sweetness { get; set; }
        public decimal Price { get; set; }

    }
}
