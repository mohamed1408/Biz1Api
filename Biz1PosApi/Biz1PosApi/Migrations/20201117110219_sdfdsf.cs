using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class sdfdsf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "minblock",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "minquantity",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "minblock",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "minquantity",
                table: "Products");
        }
    }
}
