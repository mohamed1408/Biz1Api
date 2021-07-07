using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class qqq2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "KOTs",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "KOTs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "KOTs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "KOTs",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.CreateIndex(
                name: "IX_KOTs_CompanyId",
                table: "KOTs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_KOTs_StoreId",
                table: "KOTs",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_KOTs_Companies_CompanyId",
                table: "KOTs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KOTs_Stores_StoreId",
                table: "KOTs",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
                //onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KOTs_Companies_CompanyId",
                table: "KOTs");

            migrationBuilder.DropForeignKey(
                name: "FK_KOTs_Stores_StoreId",
                table: "KOTs");

            migrationBuilder.DropIndex(
                name: "IX_KOTs_CompanyId",
                table: "KOTs");

            migrationBuilder.DropIndex(
                name: "IX_KOTs_StoreId",
                table: "KOTs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "KOTs");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "KOTs");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "KOTs");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "KOTs");
        }
    }
}
