using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class dashboard1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DashBoards_CompanyId",
                table: "DashBoards",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DashBoards_StoreId",
                table: "DashBoards",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_DashBoards_Companies_CompanyId",
                table: "DashBoards",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DashBoards_Stores_StoreId",
                table: "DashBoards",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DashBoards_Companies_CompanyId",
                table: "DashBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_DashBoards_Stores_StoreId",
                table: "DashBoards");

            migrationBuilder.DropIndex(
                name: "IX_DashBoards_CompanyId",
                table: "DashBoards");

            migrationBuilder.DropIndex(
                name: "IX_DashBoards_StoreId",
                table: "DashBoards");
        }
    }
}
