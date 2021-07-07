using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class option1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_OptionGroups_OptionGroupId",
                table: "Options");

            migrationBuilder.AlterColumn<int>(
                name: "OptionGroupId",
                table: "Options",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Options_OptionGroups_OptionGroupId",
                table: "Options",
                column: "OptionGroupId",
                principalTable: "OptionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_OptionGroups_OptionGroupId",
                table: "Options");

            migrationBuilder.AlterColumn<int>(
                name: "OptionGroupId",
                table: "Options",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_OptionGroups_OptionGroupId",
                table: "Options",
                column: "OptionGroupId",
                principalTable: "OptionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
