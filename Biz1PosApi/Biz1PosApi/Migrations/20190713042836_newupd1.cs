using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class newupd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "OrdItemAddons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdItemAddons_StatusId",
                table: "OrdItemAddons",
                column: "StatusId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrdItemAddons_DropDowns_StatusId",
            //    table: "OrdItemAddons",
            //    column: "StatusId",
            //    principalTable: "DropDowns",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdItemAddons_DropDowns_StatusId",
                table: "OrdItemAddons");

            migrationBuilder.DropIndex(
                name: "IX_OrdItemAddons_StatusId",
                table: "OrdItemAddons");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OrdItemAddons");
        }
    }
}
