using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class strad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreProductAddons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    AddOnId = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    TakeawayPrice = table.Column<float>(nullable: false),
                    DeliveryPrice = table.Column<float>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreProductAddons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreProductAddons_Addons_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "Addons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_StoreProductAddons_Products_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Products",
                    principalColumn: "Id");
                //onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_StoreProductAddons_Stores_StoreId",
                    column: x => x.StoreId,
                    principalTable: "Stores",
                    principalColumn: "Id");
                        //onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductAddons_AddOnId",
                table: "StoreProductAddons",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductAddons_ProductId",
                table: "StoreProductAddons",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductAddons_StoreId",
                table: "StoreProductAddons",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreProductAddons");
        }
    }
}
