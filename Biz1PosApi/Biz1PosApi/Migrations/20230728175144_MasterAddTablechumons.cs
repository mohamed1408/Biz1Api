using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class MasterAddTablechumons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chumons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    on = table.Column<int>(nullable: false),
                    ino = table.Column<string>(nullable: true),
                    cr = table.Column<string>(nullable: true),
                    soi = table.Column<int>(nullable: true),
                    aoi = table.Column<string>(nullable: true),
                    upoi = table.Column<long>(nullable: true),
                    si = table.Column<int>(nullable: true),
                    dsi = table.Column<int>(nullable: true),
                    cui = table.Column<int>(nullable: true),
                    cai = table.Column<int>(nullable: true),
                    dri = table.Column<int>(nullable: true),
                    osi = table.Column<int>(nullable: false),
                    psi = table.Column<int>(nullable: true),
                    ba = table.Column<double>(nullable: false),
                    ta = table.Column<double>(nullable: true),
                    pa = table.Column<double>(nullable: false),
                    ra = table.Column<double>(nullable: false),
                    s = table.Column<string>(nullable: true),
                    to = table.Column<double>(nullable: false),
                    tt = table.Column<double>(nullable: false),
                    tth = table.Column<double>(nullable: false),
                    bsi = table.Column<int>(nullable: false),
                    sti = table.Column<int>(nullable: true),
                    dp = table.Column<double>(nullable: false),
                    da = table.Column<double>(nullable: false),
                    isao = table.Column<bool>(nullable: false),
                    cud = table.Column<string>(nullable: true),
                    dti = table.Column<int>(nullable: true),
                    wi = table.Column<int>(nullable: true),
                    oddt = table.Column<DateTime>(nullable: false),
                    od = table.Column<DateTime>(type: "Date", nullable: false),
                    did = table.Column<DateTime>(type: "Date", nullable: true),
                    ddd = table.Column<DateTime>(type: "Date", nullable: true),
                    odt = table.Column<TimeSpan>(nullable: false),
                    didt = table.Column<DateTime>(nullable: true),
                    dddt = table.Column<DateTime>(nullable: true),
                    bdt = table.Column<DateTime>(nullable: false),
                    bd = table.Column<DateTime>(type: "Date", nullable: false),
                    n = table.Column<string>(nullable: true),
                    osd = table.Column<string>(nullable: true),
                    rsd = table.Column<string>(nullable: true),
                    fr = table.Column<bool>(nullable: false),
                    c = table.Column<bool>(nullable: false),
                    isp = table.Column<bool>(nullable: false),
                    oj = table.Column<string>(nullable: true),
                    ij = table.Column<string>(nullable: true),
                    cj = table.Column<string>(nullable: true),
                    cgesb = table.Column<double>(nullable: true),
                    odb = table.Column<double>(nullable: true),
                    otadb = table.Column<double>(nullable: true),
                    otodb = table.Column<double>(nullable: true),
                    aidb = table.Column<double>(nullable: true),
                    aitadb = table.Column<double>(nullable: true),
                    aitodb = table.Column<double>(nullable: true),
                    cts = table.Column<long>(nullable: true),
                    md = table.Column<DateTime>(nullable: false),
                    ui = table.Column<int>(nullable: true),
                    ci = table.Column<int>(nullable: false),
                    oti = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chumons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chumons");
        }
    }
}
