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
                    { new Guid("31c0e677-325f-4611-ae0f-fc22828a542b"), 15m, "Xiami discount", new Guid("6967eb61-fb97-4791-a808-7c3c0208cd70"), "Xiaomi Mi 11" },
                    { new Guid("72fa5226-3e79-4cf8-b426-8f3f6eef2dff"), 50m, "Iphone discount", new Guid("16676e61-9c66-4f5d-9ba2-a775b6d326d1"), "Iphone X" },
                    { new Guid("738efc5b-e91d-49c0-bbc0-828b2e972709"), 70m, "Google Pixel discount", new Guid("1ad04d68-4871-47af-a2bb-7f6864cefc08"), "Google Pixel 5" },
                    { new Guid("7a4e79db-591f-4f28-9173-e6e4b2c2843e"), 100m, "Samsung discount", new Guid("c63d2f83-f353-42e1-a07f-8be599379a32"), "Samsung S21" },
                    { new Guid("d179434d-0d55-47d4-9bc0-813ae707ddba"), 60m, "OnePlus discount", new Guid("70098a3c-6917-49b9-910c-8078cd7fdf21"), "OnePlus 9" }
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
