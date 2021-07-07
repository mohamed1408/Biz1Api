using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class dfjhjilfdsgk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StorePreferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KOTGenerate = table.Column<bool>(nullable: false),
                    EnforceCustomerNo = table.Column<bool>(nullable: false),
                    QuickOrder = table.Column<bool>(nullable: false),
                    FreeQuantity = table.Column<bool>(nullable: false),
                    ShowUpcategory = table.Column<bool>(nullable: false),
                    ShowTaxonBill = table.Column<bool>(nullable: false),
                    AdminOnlyCancel = table.Column<bool>(nullable: false),
                    DineIn = table.Column<bool>(nullable: false),
                    TakeAway = table.Column<bool>(nullable: false),
                    AdvanceOrder = table.Column<bool>(nullable: false),
                    OnlineOrder = table.Column<bool>(nullable: false),
                    Delivery = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorePreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorePreferences_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StorePreferences_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StorePreferences_CompanyId",
                table: "StorePreferences",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePreferences_StoreId",
                table: "StorePreferences",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorePreferences");
        }
    }
}
