using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class aldlasjd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RepliedMessage",
                table: "ContactForms",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isshow",
                table: "ContactForms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Careers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<string>(nullable: true),
                    PhoneNo = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    EducationDetails = table.Column<string>(nullable: true),
                    WorkDetails = table.Column<string>(nullable: true),
                    AdditionalDetails = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Careers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Careers");

            migrationBuilder.DropColumn(
                name: "RepliedMessage",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "isshow",
                table: "ContactForms");
        }
    }
}
