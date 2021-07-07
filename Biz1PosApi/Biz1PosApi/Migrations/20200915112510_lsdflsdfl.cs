using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class lsdflsdfl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Extra",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "OrderItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extra",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "OrderItems");
        }
    }
}
