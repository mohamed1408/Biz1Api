using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class AddDummyOrderResturant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InvoiceNo = table.Column<string>(nullable: true),
                    BillAmount = table.Column<float>(nullable: false),
                    PaidAmount = table.Column<float>(nullable: false),
                    Tax1 = table.Column<double>(nullable: true),
                    Tax2 = table.Column<double>(nullable: true),
                    Tax3 = table.Column<double>(nullable: true),
                    OrderJson = table.Column<string>(nullable: true),
                    ItemJson = table.Column<string>(nullable: true),
                    OrderType = table.Column<int>(nullable: true),
                    OrderStatus = table.Column<int>(nullable: true),
                    OrderedDate = table.Column<DateTime>(type: "Date", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "Date", nullable: false),
                    StoreId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantOrders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantOrders");
        }
    }
}
