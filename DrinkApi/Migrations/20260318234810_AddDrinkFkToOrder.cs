using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrinkApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDrinkFkToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add nullable column first to avoid FK conflict with existing data
            migrationBuilder.AddColumn<int>(
                name: "DrinkId",
                table: "Orders",
                type: "int",
                nullable: true);

            // Try to map existing orders to drinks by name
            migrationBuilder.Sql(@"UPDATE o SET DrinkId = d.Id FROM Orders o INNER JOIN Drinks d ON o.DrinkName = d.Name;");

            // If any orders still have NULL DrinkId, set them to a valid existing drink id (1) as fallback
            migrationBuilder.Sql("UPDATE Orders SET DrinkId = 1 WHERE DrinkId IS NULL;");

            // Make column non-nullable now that values exist
            migrationBuilder.AlterColumn<int>(
                name: "DrinkId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DrinkId",
                table: "Orders",
                column: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Drinks_DrinkId",
                table: "Orders",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Drinks_DrinkId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DrinkId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DrinkId",
                table: "Orders");
        }
    }
}
