using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class orderstatusbttn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStatusButtons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    OrderStatusId = table.Column<int>(nullable: false),
                    OrderTypePreferenceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusButtons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatusButtons_OrderTypePreferences_OrderTypePreferenceId",
                        column: x => x.OrderTypePreferenceId,
                        principalTable: "OrderTypePreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusButtons_OrderTypePreferenceId",
                table: "OrderStatusButtons",
                column: "OrderTypePreferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStatusButtons");
        }
    }
}
