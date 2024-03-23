using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class AddProdUpOrdItm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Addons_Products_ProductId",
            //    table: "Addons");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_EComProducts_Products_ProductId",
            //    table: "EComProducts");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_OrderItems_Products_ProductId",
            //    table: "OrderItems");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_PredefinedQuantities_Products_ProductId",
            //    table: "PredefinedQuantities");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductAddonGroups_Products_ProductId",
            //    table: "ProductAddonGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductAddOns_Products_ProductId",
            //    table: "ProductAddOns");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductOptionGroups_Products_ProductId",
            //    table: "ProductOptionGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductOptions_Products_ProductId",
            //    table: "ProductOptions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductVariantGroups_Products_ProductId",
            //    table: "ProductVariantGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductVariants_Products_ProductId",
            //    table: "ProductVariants");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_SaleProductGroups_Products_SaleProductId",
            //    table: "SaleProductGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_SaleProductGroups_Products_StockProductId",
            //    table: "SaleProductGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProductAddons_Products_ProductId",
            //    table: "StoreProductAddons");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProductOptions_Products_MappedProductId",
            //    table: "StoreProductOptions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProductOptions_Products_ProductId",
            //    table: "StoreProductOptions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProducts_Products_ProductId",
            //    table: "StoreProducts");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProductVariants_Products_ProductId",
            //    table: "StoreProductVariants");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TagMappings_Products_ProductId",
            //    table: "TagMappings");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_UPProducts_Products_ProductId",
            //    table: "UPProducts");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Products",
            //    table: "Products");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "Products",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProductId",
            //    table: "Products",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Products",
            //    table: "Products",
            //    column: "ProductId");

            //migrationBuilder.CreateTable(
            //    name: "UpOrdItms",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        OrdId = table.Column<int>(nullable: false),
            //        ProdName = table.Column<string>(nullable: true),
            //        Qty = table.Column<float>(nullable: false),
            //        Price = table.Column<float>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UpOrdItms", x => x.Id);
            //    });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Addons_Products_ProductId",
            //    table: "Addons",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_EComProducts_Products_ProductId",
            //    table: "EComProducts",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrderItems_Products_ProductId",
            //    table: "OrderItems",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PredefinedQuantities_Products_ProductId",
            //    table: "PredefinedQuantities",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductAddonGroups_Products_ProductId",
            //    table: "ProductAddonGroups",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductAddOns_Products_ProductId",
            //    table: "ProductAddOns",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductOptionGroups_Products_ProductId",
            //    table: "ProductOptionGroups",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductOptions_Products_ProductId",
            //    table: "ProductOptions",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductVariantGroups_Products_ProductId",
            //    table: "ProductVariantGroups",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductVariants_Products_ProductId",
            //    table: "ProductVariants",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SaleProductGroups_Products_SaleProductId",
            //    table: "SaleProductGroups",
            //    column: "SaleProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SaleProductGroups_Products_StockProductId",
            //    table: "SaleProductGroups",
            //    column: "StockProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProductAddons_Products_ProductId",
            //    table: "StoreProductAddons",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProductOptions_Products_MappedProductId",
            //    table: "StoreProductOptions",
            //    column: "MappedProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProductOptions_Products_ProductId",
            //    table: "StoreProductOptions",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProducts_Products_ProductId",
            //    table: "StoreProducts",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProductVariants_Products_ProductId",
            //    table: "StoreProductVariants",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TagMappings_Products_ProductId",
            //    table: "TagMappings",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_UPProducts_Products_ProductId",
            //    table: "UPProducts",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "ProductId",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Addons_Products_ProductId",
            //    table: "Addons");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_EComProducts_Products_ProductId",
            //    table: "EComProducts");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_OrderItems_Products_ProductId",
            //    table: "OrderItems");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_PredefinedQuantities_Products_ProductId",
            //    table: "PredefinedQuantities");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductAddonGroups_Products_ProductId",
            //    table: "ProductAddonGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductAddOns_Products_ProductId",
            //    table: "ProductAddOns");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductOptionGroups_Products_ProductId",
            //    table: "ProductOptionGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductOptions_Products_ProductId",
            //    table: "ProductOptions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductVariantGroups_Products_ProductId",
            //    table: "ProductVariantGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductVariants_Products_ProductId",
            //    table: "ProductVariants");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_SaleProductGroups_Products_SaleProductId",
            //    table: "SaleProductGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_SaleProductGroups_Products_StockProductId",
            //    table: "SaleProductGroups");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProductAddons_Products_ProductId",
            //    table: "StoreProductAddons");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProductOptions_Products_MappedProductId",
            //    table: "StoreProductOptions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProductOptions_Products_ProductId",
            //    table: "StoreProductOptions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProducts_Products_ProductId",
            //    table: "StoreProducts");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_StoreProductVariants_Products_ProductId",
            //    table: "StoreProductVariants");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TagMappings_Products_ProductId",
            //    table: "TagMappings");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_UPProducts_Products_ProductId",
            //    table: "UPProducts");

            //migrationBuilder.DropTable(
            //    name: "UpOrdItms");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Products",
            //    table: "Products");

            //migrationBuilder.DropColumn(
            //    name: "ProductId",
            //    table: "Products");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "Products",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Products",
            //    table: "Products",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Addons_Products_ProductId",
            //    table: "Addons",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_EComProducts_Products_ProductId",
            //    table: "EComProducts",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrderItems_Products_ProductId",
            //    table: "OrderItems",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PredefinedQuantities_Products_ProductId",
            //    table: "PredefinedQuantities",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductAddonGroups_Products_ProductId",
            //    table: "ProductAddonGroups",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductAddOns_Products_ProductId",
            //    table: "ProductAddOns",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductOptionGroups_Products_ProductId",
            //    table: "ProductOptionGroups",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductOptions_Products_ProductId",
            //    table: "ProductOptions",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductVariantGroups_Products_ProductId",
            //    table: "ProductVariantGroups",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductVariants_Products_ProductId",
            //    table: "ProductVariants",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SaleProductGroups_Products_SaleProductId",
            //    table: "SaleProductGroups",
            //    column: "SaleProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SaleProductGroups_Products_StockProductId",
            //    table: "SaleProductGroups",
            //    column: "StockProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProductAddons_Products_ProductId",
            //    table: "StoreProductAddons",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProductOptions_Products_MappedProductId",
            //    table: "StoreProductOptions",
            //    column: "MappedProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProductOptions_Products_ProductId",
            //    table: "StoreProductOptions",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProducts_Products_ProductId",
            //    table: "StoreProducts",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StoreProductVariants_Products_ProductId",
            //    table: "StoreProductVariants",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TagMappings_Products_ProductId",
            //    table: "TagMappings",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_UPProducts_Products_ProductId",
            //    table: "UPProducts",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
