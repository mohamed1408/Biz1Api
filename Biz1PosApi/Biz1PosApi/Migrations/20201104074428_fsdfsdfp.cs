using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class fsdfsdfp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
