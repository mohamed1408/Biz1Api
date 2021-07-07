using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class ee1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_OrdItemAddons_DropDowns_StatusId",
            //    table: "OrdItemAddons");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "OrdItemAddons",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DiningTables",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "DiningTables",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DiningAreas",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "DiningAreas",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.CreateIndex(
                name: "IX_DiningTables_CompanyId",
                table: "DiningTables",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DiningTables_StoreId",
                table: "DiningTables",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_DiningAreas_CompanyId",
                table: "DiningAreas",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DiningAreas_StoreId",
                table: "DiningAreas",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiningAreas_Companies_CompanyId",
                table: "DiningAreas",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiningAreas_Stores_StoreId",
                table: "DiningAreas",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiningTables_Companies_CompanyId",
                table: "DiningTables",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiningTables_Stores_StoreId",
                table: "DiningTables",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
                //onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrdItemAddons_DropDowns_StatusId",
            //    table: "OrdItemAddons",
            //    column: "StatusId",
            //    principalTable: "DropDowns",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiningAreas_Companies_CompanyId",
                table: "DiningAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_DiningAreas_Stores_StoreId",
                table: "DiningAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_DiningTables_Companies_CompanyId",
                table: "DiningTables");

            migrationBuilder.DropForeignKey(
                name: "FK_DiningTables_Stores_StoreId",
                table: "DiningTables");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdItemAddons_DropDowns_StatusId",
                table: "OrdItemAddons");

            migrationBuilder.DropIndex(
                name: "IX_DiningTables_CompanyId",
                table: "DiningTables");

            migrationBuilder.DropIndex(
                name: "IX_DiningTables_StoreId",
                table: "DiningTables");

            migrationBuilder.DropIndex(
                name: "IX_DiningAreas_CompanyId",
                table: "DiningAreas");

            migrationBuilder.DropIndex(
                name: "IX_DiningAreas_StoreId",
                table: "DiningAreas");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DiningTables");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "DiningTables");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DiningAreas");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "DiningAreas");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "OrdItemAddons",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdItemAddons_DropDowns_StatusId",
                table: "OrdItemAddons",
                column: "StatusId",
                principalTable: "DropDowns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
