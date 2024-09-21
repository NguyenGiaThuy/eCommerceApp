using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Discount.Grpc.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Amount", "Description", "ProductId", "ProductName" },
                values: new object[,]
                {
                    { new Guid("05da7b19-44ff-4441-8097-1f75cc4475df"), 100m, "Samsung discount", new Guid("c63d2f83-f353-42e1-a07f-8be599379a32"), "Samsung S21" },
                    { new Guid("6a3f64ad-a605-4cb2-85c7-63c9facb1d8d"), 15m, "Xiami discount", new Guid("6967eb61-fb97-4791-a808-7c3c0208cd70"), "Xiaomi Mi 11" },
                    { new Guid("e4116eba-0f9f-42f3-8de2-93af39a760aa"), 60m, "OnePlus discount", new Guid("70098a3c-6917-49b9-910c-8078cd7fdf21"), "OnePlus 9" },
                    { new Guid("f2e3b9ec-1030-4de0-a76f-e93f01782749"), 70m, "Google Pixel discount", new Guid("1ad04d68-4871-47af-a2bb-7f6864cefc08"), "Google Pixel 5" },
                    { new Guid("fe579b97-9006-4bde-82f5-c5676e20d6ea"), 50m, "Iphone discount", new Guid("16676e61-9c66-4f5d-9ba2-a775b6d326d1"), "Iphone X" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
