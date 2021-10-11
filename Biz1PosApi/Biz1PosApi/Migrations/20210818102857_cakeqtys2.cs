using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class cakeqtys2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CakeQuantityId",
                table: "PredefinedQuantities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CakeQuantityId",
                table: "PredefinedQuantities",
                nullable: false,
                defaultValue: 0);
        }
    }
}
