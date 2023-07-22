using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class fbecomprodtblidms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "EComProducts",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "EComProducts",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
