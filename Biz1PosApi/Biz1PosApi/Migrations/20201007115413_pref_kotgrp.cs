using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class pref_kotgrp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "customeraddressinbill",
                table: "StorePreferences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "showchildcategory",
                table: "StorePreferences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Printer",
                table: "KOTGroups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "customeraddressinbill",
                table: "StorePreferences");

            migrationBuilder.DropColumn(
                name: "showchildcategory",
                table: "StorePreferences");

            migrationBuilder.DropColumn(
                name: "Printer",
                table: "KOTGroups");
        }
    }
}
