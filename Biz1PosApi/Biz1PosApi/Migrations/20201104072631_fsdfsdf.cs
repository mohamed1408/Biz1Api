using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class fsdfsdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodPanda",
                table: "UrbanPiperStores");

            migrationBuilder.DropColumn(
                name: "IsUrbanPiper",
                table: "UrbanPiperStores");

            migrationBuilder.DropColumn(
                name: "UberEats",
                table: "UrbanPiperStores");

            migrationBuilder.DropColumn(
                name: "UrbanPiper",
                table: "UrbanPiperStores");

            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarCode",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "FoodPanda",
                table: "UrbanPiperStores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUrbanPiper",
                table: "UrbanPiperStores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UberEats",
                table: "UrbanPiperStores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UrbanPiper",
                table: "UrbanPiperStores",
                nullable: false,
                defaultValue: false);
        }
    }
}
