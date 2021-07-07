using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class _550 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FK_Products_DropDowns_TypeId",
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

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Variants",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "VariantGroups",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Tax3",
                table: "TaxGroups",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Tax2",
                table: "TaxGroups",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Tax1",
                table: "TaxGroups",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TakeawayPrice",
                table: "StoreProductVariants",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "StoreProductVariants",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "DeliveryPrice",
                table: "StoreProductVariants",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "StoreProductVariants",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TakeawayPrice",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<bool>(
                name: "IsTakeAwayService",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDineInService",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeliveryService",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "DeliveryPrice",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TakeawayPrice",
                table: "StoreProductAddons",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "StoreProductAddons",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "StoreProductAddons",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "DeliveryPrice",
                table: "StoreProductAddons",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "StoreProductAddons",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TakeawayPrice",
                table: "ProductVariants",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ProductVariants",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "DeliveryPrice",
                table: "ProductVariants",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "ProductVariants",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TakeawayPrice",
                table: "Products",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "DeliveryPrice",
                table: "Products",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TakeawayPrice",
                table: "ProductAddOns",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ProductAddOns",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "DeliveryPrice",
                table: "ProductAddOns",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "ProductAddOns",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "OrdItemAddons",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "PaidAmount",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "DiscPercent",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "DiscAmount",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "BillAmount",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Tax3",
                table: "OrderItems",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Tax2",
                table: "OrderItems",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Tax1",
                table: "OrderItems",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "DiscPercent",
                table: "OrderItems",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "DiscAmount",
                table: "OrderItems",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Addons",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "AddonGroups",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AddonGroups_Companies_CompanyId",
                table: "AddonGroups",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addons_Companies_CompanyId",
                table: "Addons",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAddOns_Companies_CompanyId",
                table: "ProductAddOns",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_DropDowns_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "DropDowns",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Companies_CompanyId",
                table: "ProductVariants",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreProductAddons_Companies_CompanyId",
                table: "StoreProductAddons",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreProducts_Companies_CompanyId",
                table: "StoreProducts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreProductVariants_Companies_CompanyId",
                table: "StoreProductVariants",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantGroups_Companies_CompanyId",
                table: "VariantGroups",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
            //onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_Companies_CompanyId",
                table: "Variants",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
                //onDelete: ReferentialAction.Cascade);
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
                name: "FK_Products_DropDowns_TypeId",
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

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Variants",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "VariantGroups",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "Amount",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Tax3",
                table: "TaxGroups",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Tax2",
                table: "TaxGroups",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Tax1",
                table: "TaxGroups",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "TakeawayPrice",
                table: "StoreProductVariants",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "StoreProductVariants",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "DeliveryPrice",
                table: "StoreProductVariants",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "StoreProductVariants",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "TakeawayPrice",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<bool>(
                name: "IsTakeAwayService",
                table: "StoreProducts",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDineInService",
                table: "StoreProducts",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeliveryService",
                table: "StoreProducts",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "StoreProducts",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<float>(
                name: "DeliveryPrice",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "StoreProducts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "TakeawayPrice",
                table: "StoreProductAddons",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "StoreProductAddons",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "StoreProductAddons",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<float>(
                name: "DeliveryPrice",
                table: "StoreProductAddons",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "StoreProductAddons",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "TakeawayPrice",
                table: "ProductVariants",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "ProductVariants",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "DeliveryPrice",
                table: "ProductVariants",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "ProductVariants",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "TakeawayPrice",
                table: "Products",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Products",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "DeliveryPrice",
                table: "Products",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "TakeawayPrice",
                table: "ProductAddOns",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "ProductAddOns",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "DeliveryPrice",
                table: "ProductAddOns",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "ProductAddOns",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "OrdItemAddons",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "PaidAmount",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "DiscPercent",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "DiscAmount",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "BillAmount",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Tax3",
                table: "OrderItems",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Tax2",
                table: "OrderItems",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Tax1",
                table: "OrderItems",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "DiscPercent",
                table: "OrderItems",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "DiscAmount",
                table: "OrderItems",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Addons",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "AddonGroups",
                nullable: true,
                oldClrType: typeof(int));

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
                name: "FK_Products_DropDowns_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "DropDowns",
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
    }
}
