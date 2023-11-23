using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class storeob_expense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Transactions",
            //    table: "Transactions");

            //migrationBuilder.DropColumn(
            //    name: "bd",
            //    table: "Odrs");

            //migrationBuilder.DropColumn(
            //    name: "bdt",
            //    table: "Odrs");

            //migrationBuilder.DropColumn(
            //    name: "odt",
            //    table: "Odrs");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "Transactions",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "TransactionId",
            //    table: "Transactions",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<double>(
                name: "Expense",
                table: "StoreOB",
                nullable: false,
                defaultValue: 0.0);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Transactions",
            //    table: "Transactions",
            //    column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Expense",
                table: "StoreOB");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "bd",
                table: "Odrs",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "bdt",
                table: "Odrs",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "odt",
                table: "Odrs",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");
        }
    }
}
