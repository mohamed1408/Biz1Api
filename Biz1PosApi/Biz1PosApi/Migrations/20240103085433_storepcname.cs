using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class storepcname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardName",
                table: "Stores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhonePeName",
                table: "Stores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardName",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "PhonePeName",
                table: "Stores");
        }
    }
}
