using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class pd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupSortOrder",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isonline",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupSortOrder",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "isonline",
                table: "Products");
        }
    }
}
