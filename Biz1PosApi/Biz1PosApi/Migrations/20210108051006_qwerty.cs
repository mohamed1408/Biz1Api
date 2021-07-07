using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class qwerty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "orderitemrefid",
                table: "OrdItemOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "kotrefid",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "refid",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "orderrefid",
                table: "KOTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "refid",
                table: "KOTs",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "Devices",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Token = table.Column<string>(nullable: true),
            //        UserId = table.Column<int>(nullable: false),
            //        CompanyId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Devices", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Devices_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Devices_Users_UserId",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "KOTGroupUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        KOTGroupId = table.Column<int>(nullable: false),
            //        UserId = table.Column<int>(nullable: false),
            //        CompanyId = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_KOTGroupUsers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_KOTGroupUsers_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_KOTGroupUsers_KOTGroups_KOTGroupId",
            //            column: x => x.KOTGroupId,
            //            principalTable: "KOTGroups",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_KOTGroupUsers_Users_UserId",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Devices_CompanyId",
            //    table: "Devices",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Devices_UserId",
            //    table: "Devices",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_KOTGroupUsers_CompanyId",
            //    table: "KOTGroupUsers",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_KOTGroupUsers_KOTGroupId",
            //    table: "KOTGroupUsers",
            //    column: "KOTGroupId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_KOTGroupUsers_UserId",
            //    table: "KOTGroupUsers",
            //    column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Devices");

            //migrationBuilder.DropTable(
            //    name: "KOTGroupUsers");

            migrationBuilder.DropColumn(
                name: "orderitemrefid",
                table: "OrdItemOptions");

            migrationBuilder.DropColumn(
                name: "kotrefid",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "refid",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "orderrefid",
                table: "KOTs");

            migrationBuilder.DropColumn(
                name: "refid",
                table: "KOTs");
        }
    }
}
