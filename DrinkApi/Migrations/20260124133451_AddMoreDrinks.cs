using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrinkApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreDrinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sweetness = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "Id", "Name", "Price", "Sweetness", "Type" },
                values: new object[,]
                {
                    { 1, "Latte", 39.00m, 5, "Coffee" },
                    { 2, "Green Tea", 29.00m, 2, "Tea" },
                    { 3, "Mojito", 89.00m, 7, "Cocktail" },
                    { 4, "Espresso", 29.00m, 1, "Coffee" },
                    { 5, "Cappuccino", 42.00m, 4, "Coffee" },
                    { 6, "Americano", 35.00m, 1, "Coffee" },
                    { 7, "Mocha", 45.00m, 7, "Coffee" },
                    { 8, "Hot Chocolate", 38.00m, 9, "Chocolate" },
                    { 9, "Chai Latte", 42.00m, 6, "Tea" },
                    { 10, "Matcha Latte", 48.00m, 5, "Tea" },
                    { 11, "Earl Grey", 29.00m, 2, "Tea" },
                    { 12, "Peppermint Tea", 29.00m, 3, "Tea" },
                    { 13, "Orange Juice", 35.00m, 8, "Juice" },
                    { 14, "Apple Juice", 35.00m, 7, "Juice" },
                    { 15, "Lemonade", 32.00m, 8, "Juice" },
                    { 16, "Smoothie", 55.00m, 7, "Smoothie" },
                    { 17, "Milkshake", 52.00m, 10, "Milkshake" },
                    { 18, "Cola", 25.00m, 9, "Soda" },
                    { 19, "Sprite", 25.00m, 8, "Soda" },
                    { 20, "Iced Tea", 32.00m, 6, "Tea" },
                    { 21, "Margarita", 95.00m, 6, "Cocktail" },
                    { 22, "Piña Colada", 99.00m, 9, "Cocktail" },
                    { 23, "Cosmopolitan", 92.00m, 5, "Cocktail" },
                    { 24, "Long Island Iced Tea", 105.00m, 7, "Cocktail" },
                    { 25, "Water", 15.00m, 0, "Water" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drinks");
        }
    }
}
