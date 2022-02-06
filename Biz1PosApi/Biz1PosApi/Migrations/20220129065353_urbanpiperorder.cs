using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class urbanpiperorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrbanPiperOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UPOrderId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    OrderStatusId = table.Column<int>(nullable: false),
                    Json = table.Column<string>(nullable: true),
                    RiderDetails = table.Column<string>(nullable: true),
                    OrderedDateTime = table.Column<DateTime>(nullable: false),
                    AcceptedTimeStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrbanPiperOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrbanPiperOrders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrbanPiperOrders_StoreId",
                table: "UrbanPiperOrders",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrbanPiperOrders");
        }
    }
}
