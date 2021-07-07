using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class gggggggggg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AllItemDisc",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AllItemTaxDisc",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AllItemTotalDisc",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OrderDiscount",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OrderTaxDisc",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OrderTotDisc",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ItemDiscount",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OrderDiscount",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxItemDiscount",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxOrderDiscount",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllItemDisc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AllItemTaxDisc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AllItemTotalDisc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderDiscount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderTaxDisc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderTotDisc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ItemDiscount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderDiscount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "TaxItemDiscount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "TaxOrderDiscount",
                table: "OrderItems");
        }
    }
}
