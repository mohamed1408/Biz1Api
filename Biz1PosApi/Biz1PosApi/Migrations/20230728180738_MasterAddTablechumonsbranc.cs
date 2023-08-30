using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class MasterAddTablechumonsbranc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChumonBranches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    qy = table.Column<float>(nullable: false),
                    pr = table.Column<float>(nullable: false),
                    ob = table.Column<int>(nullable: false),
                    pi = table.Column<int>(nullable: false),
                    to = table.Column<double>(nullable: false),
                    tt = table.Column<double>(nullable: false),
                    tth = table.Column<double>(nullable: false),
                    dp = table.Column<double>(nullable: false),
                    da = table.Column<double>(nullable: false),
                    imd = table.Column<double>(nullable: true),
                    tid = table.Column<double>(nullable: true),
                    od = table.Column<double>(nullable: true),
                    tod = table.Column<double>(nullable: true),
                    sti = table.Column<int>(nullable: false),
                    kui = table.Column<int>(nullable: true),
                    ki = table.Column<int>(nullable: true),
                    n = table.Column<string>(nullable: true),
                    msg = table.Column<string>(nullable: true),
                    ta = table.Column<double>(nullable: true),
                    ext = table.Column<double>(nullable: true),
                    kri = table.Column<string>(nullable: true),
                    ri = table.Column<string>(nullable: true),
                    issu = table.Column<bool>(nullable: false),
                    cati = table.Column<int>(nullable: true),
                    oj = table.Column<string>(nullable: true),
                    cqy = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChumonBranches", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChumonBranches");
        }
    }
}
