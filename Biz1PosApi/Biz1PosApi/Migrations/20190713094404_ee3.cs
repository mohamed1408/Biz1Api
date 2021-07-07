using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class ee3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Variants",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "VariantGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "StoreProductVariants",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "StoreProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "StoreProductAddons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ProductVariants",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ProductAddOns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Addons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "AddonGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Variants_CompanyId",
                table: "Variants",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_VariantGroups_CompanyId",
                table: "VariantGroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductVariants_CompanyId",
                table: "StoreProductVariants",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProducts_CompanyId",
                table: "StoreProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductAddons_CompanyId",
                table: "StoreProductAddons",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_CompanyId",
                table: "ProductVariants",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId",
                table: "Products",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAddOns_CompanyId",
                table: "ProductAddOns",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Addons_CompanyId",
                table: "Addons",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AddonGroups_CompanyId",
                table: "AddonGroups",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddonGroups_Companies_CompanyId",
                table: "AddonGroups",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Addons_Companies_CompanyId",
                table: "Addons",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAddOns_Companies_CompanyId",
                table: "ProductAddOns",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Companies_CompanyId",
                table: "ProductVariants",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreProductAddons_Companies_CompanyId",
                table: "StoreProductAddons",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreProducts_Companies_CompanyId",
                table: "StoreProducts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreProductVariants_Companies_CompanyId",
                table: "StoreProductVariants",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantGroups_Companies_CompanyId",
                table: "VariantGroups",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_Companies_CompanyId",
                table: "Variants",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddonGroups_Companies_CompanyId",
                table: "AddonGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Addons_Companies_CompanyId",
                table: "Addons");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAddOns_Companies_CompanyId",
                table: "ProductAddOns");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Companies_CompanyId",
                table: "ProductVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreProductAddons_Companies_CompanyId",
                table: "StoreProductAddons");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreProducts_Companies_CompanyId",
                table: "StoreProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreProductVariants_Companies_CompanyId",
                table: "StoreProductVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantGroups_Companies_CompanyId",
                table: "VariantGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Variants_Companies_CompanyId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_CompanyId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_VariantGroups_CompanyId",
                table: "VariantGroups");

            migrationBuilder.DropIndex(
                name: "IX_StoreProductVariants_CompanyId",
                table: "StoreProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_StoreProducts_CompanyId",
                table: "StoreProducts");

            migrationBuilder.DropIndex(
                name: "IX_StoreProductAddons_CompanyId",
                table: "StoreProductAddons");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_CompanyId",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_Products_CompanyId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductAddOns_CompanyId",
                table: "ProductAddOns");

            migrationBuilder.DropIndex(
                name: "IX_Addons_CompanyId",
                table: "Addons");

            migrationBuilder.DropIndex(
                name: "IX_AddonGroups_CompanyId",
                table: "AddonGroups");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "VariantGroups");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "StoreProductVariants");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "StoreProducts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "StoreProductAddons");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ProductAddOns");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Addons");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AddonGroups");
        }
    }
}
