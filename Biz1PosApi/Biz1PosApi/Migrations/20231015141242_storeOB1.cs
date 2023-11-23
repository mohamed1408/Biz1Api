using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class storeOB1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreOB_Stores_StoreId",
            //    table: "StoreOB");

            //migrationBuilder.DropIndex(
            //    name: "IX_StoreOB_StoreId",
            //    table: "StoreOB");

            //migrationBuilder.AddColumn<double>(
            //    name: "OpeningBalance",
            //    table: "StoreOB",
            //    nullable: false,
            //    defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                table: "StoreOB");

            migrationBuilder.CreateIndex(
                name: "IX_StoreOB_StoreId",
                table: "StoreOB",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreOB_Stores_StoreId",
                table: "StoreOB",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
