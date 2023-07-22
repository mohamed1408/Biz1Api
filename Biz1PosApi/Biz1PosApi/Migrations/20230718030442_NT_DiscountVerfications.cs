using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class NT_DiscountVerfications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscountVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Reference = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    VerifiedBy = table.Column<int>(nullable: true),
                    OrderId = table.Column<int>(nullable: true),
                    VerifiedDatetime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountVerifications_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscountVerifications_Users_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountVerifications_OrderId",
                table: "DiscountVerifications",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountVerifications_VerifiedBy",
                table: "DiscountVerifications",
                column: "VerifiedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountVerifications");
        }
    }
}
