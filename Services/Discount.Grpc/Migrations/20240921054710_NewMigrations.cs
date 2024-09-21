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
                    { new Guid("1bb7d6dc-8b47-4ed2-9b9a-d92b615e90e0"), 60m, "OnePlus discount", new Guid("70098a3c-6917-49b9-910c-8078cd7fdf21"), "OnePlus 9" },
                    { new Guid("74ad172c-7a58-482a-906f-29fcd09eac69"), 100m, "Samsung discount", new Guid("c63d2f83-f353-42e1-a07f-8be599379a32"), "Samsung S21" },
                    { new Guid("b95e4e46-568a-436c-8ad0-0f753e44bbea"), 70m, "Google Pixel discount", new Guid("1ad04d68-4871-47af-a2bb-7f6864cefc08"), "Google Pixel 5" },
                    { new Guid("cbe76cae-257b-4514-be3c-520d78ea3312"), 50m, "Iphone discount", new Guid("16676e61-9c66-4f5d-9ba2-a775b6d326d1"), "Iphone X" },
                    { new Guid("e6168cf9-13e2-4c9f-bf52-960950369a38"), 15m, "Xiami discount", new Guid("6967eb61-fb97-4791-a808-7c3c0208cd70"), "Xiaomi Mi 11" }
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
