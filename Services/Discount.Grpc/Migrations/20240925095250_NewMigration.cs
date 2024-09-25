using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Discount.Grpc.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
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
                    { new Guid("206bb5a8-a2a9-4e6b-8a12-74737be21aad"), 50m, "Iphone discount", new Guid("16676e61-9c66-4f5d-9ba2-a775b6d326d1"), "Iphone X" },
                    { new Guid("964c1485-e5fd-45b3-8fe8-15f1a8d7149a"), 15m, "Xiami discount", new Guid("6967eb61-fb97-4791-a808-7c3c0208cd70"), "Xiaomi Mi 11" },
                    { new Guid("97a5ccb3-714e-4942-bd51-6e0a0ceac7ea"), 70m, "Google Pixel discount", new Guid("1ad04d68-4871-47af-a2bb-7f6864cefc08"), "Google Pixel 5" },
                    { new Guid("9c9d97c3-7135-41ec-8acf-6ba72d0636e2"), 60m, "OnePlus discount", new Guid("70098a3c-6917-49b9-910c-8078cd7fdf21"), "OnePlus 9" },
                    { new Guid("9d86d345-6c10-4483-9368-428f3ce5667d"), 100m, "Samsung discount", new Guid("c63d2f83-f353-42e1-a07f-8be599379a32"), "Samsung S21" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_ProductId",
                table: "Coupons",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
