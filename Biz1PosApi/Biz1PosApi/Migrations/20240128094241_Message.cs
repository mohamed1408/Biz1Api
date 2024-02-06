using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class Message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MsgTransType = table.Column<int>(nullable: false),
                    MessageType = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ImgId = table.Column<int>(nullable: true),
                    StoreId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    MessageDate = table.Column<DateTime>(type: "Date", nullable: false),
                    MessageDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Imgs_ImgId",
                        column: x => x.ImgId,
                        principalTable: "Imgs",
                        principalColumn: "ImgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ImgId",
                table: "Messages",
                column: "ImgId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
