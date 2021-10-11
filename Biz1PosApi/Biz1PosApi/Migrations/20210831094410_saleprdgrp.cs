using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class saleprdgrp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<double>(
            //    name: "Factor",
            //    table: "SaleProductGroups",
            //    nullable: true,
            //    oldClrType: typeof(int));

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsOnline",
            //    table: "SaleProductGroups",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "SortOrder",
            //    table: "SaleProductGroups",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "IsOnline",
            //    table: "SaleProductGroups");

            //migrationBuilder.DropColumn(
            //    name: "SortOrder",
            //    table: "SaleProductGroups");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Factor",
            //    table: "SaleProductGroups",
            //    nullable: false,
            //    oldClrType: typeof(double),
            //    oldNullable: true);
        }
    }
}
