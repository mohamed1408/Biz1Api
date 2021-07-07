using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class biz1db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdItemAddons_ProductAddOns_ProductAddonId",
                table: "OrdItemAddons");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdItemVariants_ProductVariants_ProductVariantId",
                table: "OrdItemVariants");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "OrdItemVariants",
                newName: "VariantId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdItemVariants_ProductVariantId",
                table: "OrdItemVariants",
                newName: "IX_OrdItemVariants_VariantId");

            migrationBuilder.RenameColumn(
                name: "ProductAddonId",
                table: "OrdItemAddons",
                newName: "AddonId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdItemAddons_ProductAddonId",
                table: "OrdItemAddons",
                newName: "IX_OrdItemAddons_AddonId");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrdItemVariants",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "OrdItemAddons",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "POSOrderId",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdItemAddons_Addons_AddonId",
                table: "OrdItemAddons",
                column: "AddonId",
                principalTable: "Addons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdItemVariants_Variants_VariantId",
                table: "OrdItemVariants",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdItemAddons_Addons_AddonId",
                table: "OrdItemAddons");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdItemVariants_Variants_VariantId",
                table: "OrdItemVariants");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrdItemVariants");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrdItemAddons");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "POSOrderId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "VariantId",
                table: "OrdItemVariants",
                newName: "ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdItemVariants_VariantId",
                table: "OrdItemVariants",
                newName: "IX_OrdItemVariants_ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "AddonId",
                table: "OrdItemAddons",
                newName: "ProductAddonId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdItemAddons_AddonId",
                table: "OrdItemAddons",
                newName: "IX_OrdItemAddons_ProductAddonId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdItemAddons_ProductAddOns_ProductAddonId",
                table: "OrdItemAddons",
                column: "ProductAddonId",
                principalTable: "ProductAddOns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdItemVariants_ProductVariants_ProductVariantId",
                table: "OrdItemVariants",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
