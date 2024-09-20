using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Discount.Grpc.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "ProductId", "Amount", "Description", "ProductName" },
                values: new object[,]
                {
                    { new Guid("18bbc613-6d2d-4312-88a3-2468dff9fb64"), 50m, "Iphone discount", "Iphone X" },
                    { new Guid("558dbb83-6eae-4d05-bfe3-7518ab307673"), 70m, "Google Pixel discount", "Google Pixel 5" },
                    { new Guid("5b52a704-1c17-4d5a-90e3-3d2f787472f7"), 30m, "Xiaomi discount", "Xiaomi Mi 11" },
                    { new Guid("66447ae7-0d70-4f6f-ae5b-d31044ec789a"), 70m, "Google Pixel discount", "Google Pixel 5" },
                    { new Guid("95d4bc92-f6be-4634-8c5e-39f456866b3e"), 100m, "Samsung discount", "Samsung S21" },
                    { new Guid("9e58b473-9679-4c48-bf72-6a56ff2a0a22"), 80m, "OnePlus discount", "OnePlus 9" },
                    { new Guid("c6e740c3-acef-4cba-8fac-0e64b236e71e"), 70m, "Google Pixel discount", "Google Pixel 5" },
                    { new Guid("df65e387-8e1d-4185-879f-17fc1bb95711"), 30m, "Xiaomi discount", "Xiaomi Mi 11" },
                    { new Guid("eb354dda-b488-4b73-8d81-f4eafbc17cf7"), 30m, "Xiaomi discount", "Xiaomi Mi 11" },
                    { new Guid("feb6d5ae-8a58-4a64-bf1c-bc788021ae96"), 50m, "Iphone discount", "Iphone X" }
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
