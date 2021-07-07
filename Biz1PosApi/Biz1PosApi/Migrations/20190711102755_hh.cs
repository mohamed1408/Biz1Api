using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class hh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "OrderType",
            //    table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Variants",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Variants",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StoreProductVariants",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "StoreProductVariants",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StoreProducts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "StoreProducts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StoreProductAddons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "StoreProductAddons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductVariants",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ProductVariants",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductAddOns",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ProductAddOns",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<int>(
            //    name: "OrderTypeId",
            //    table: "Orders",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PostalCode",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Addons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Addons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.CreateIndex(
            //    name: "IX_Orders_OrderTypeId",
            //    table: "Orders",
            //    column: "OrderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_KOTId",
                table: "OrderItems",
                column: "KOTId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_KOTs_KOTId",
                table: "OrderItems",
                column: "KOTId",
                principalTable: "KOTs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Orders_DropDowns_OrderTypeId",
            //    table: "Orders",
            //    column: "OrderTypeId",
            //    principalTable: "DropDowns",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_KOTs_KOTId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DropDowns_OrderTypeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderTypeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_KOTId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StoreProductVariants");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "StoreProductVariants");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StoreProducts");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "StoreProducts");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StoreProductAddons");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "StoreProductAddons");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProductAddOns");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "ProductAddOns");

            migrationBuilder.DropColumn(
                name: "OrderTypeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Addons");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Addons");

            //migrationBuilder.AddColumn<string>(
            //    name: "OrderType",
            //    table: "Orders",
            //    nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostalCode",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
