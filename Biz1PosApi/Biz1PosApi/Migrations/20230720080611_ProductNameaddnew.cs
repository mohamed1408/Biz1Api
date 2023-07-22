using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class ProductNameaddnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Products");
        }
    }
}
