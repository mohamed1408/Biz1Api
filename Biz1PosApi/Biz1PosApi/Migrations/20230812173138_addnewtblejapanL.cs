using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class addnewtblejapanL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "E",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ksid = table.Column<int>(nullable: false),
                    Ins = table.Column<string>(nullable: true),
                    Kn = table.Column<string>(nullable: true),
                    rid = table.Column<string>(nullable: true),
                    orid = table.Column<string>(nullable: true),
                    j = table.Column<string>(nullable: true),
                    oi = table.Column<int>(nullable: false),
                    cd = table.Column<DateTime>(nullable: false),
                    md = table.Column<DateTime>(nullable: false),
                    ci = table.Column<int>(nullable: false),
                    si = table.Column<int>(nullable: true),
                    kgi = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Q",
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
                    oti = table.Column<int>(nullable: false),
                    inoj = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Q", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "R",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    oii = table.Column<int>(nullable: false),
                    oirid = table.Column<string>(nullable: true),
                    p = table.Column<double>(nullable: false),
                    opi = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    oi = table.Column<int>(nullable: false),
                    acid = table.Column<int>(nullable: true),
                    cp = table.Column<double>(nullable: false),
                    ca = table.Column<double>(nullable: false),
                    t1 = table.Column<double>(nullable: false),
                    t2 = table.Column<double>(nullable: false),
                    t3 = table.Column<double>(nullable: false),
                    cd = table.Column<DateTime>(nullable: false),
                    md = table.Column<DateTime>(nullable: false),
                    ci = table.Column<int>(nullable: false),
                    si = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "W",
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
                    table.PrimaryKey("PK_W", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Y",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    a = table.Column<double>(nullable: false),
                    oi = table.Column<int>(nullable: true),
                    cui = table.Column<int>(nullable: true),
                    ptid = table.Column<int>(nullable: false),
                    sptid = table.Column<int>(nullable: true),
                    ttid = table.Column<int>(nullable: false),
                    psid = table.Column<int>(nullable: true),
                    tdt = table.Column<DateTime>(nullable: false),
                    mdt = table.Column<DateTime>(nullable: true),
                    td = table.Column<DateTime>(type: "Date", nullable: false),
                    ui = table.Column<int>(nullable: true),
                    ci = table.Column<int>(nullable: false),
                    si = table.Column<int>(nullable: true),
                    n = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Y", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "E");

            migrationBuilder.DropTable(
                name: "Q");

            migrationBuilder.DropTable(
                name: "R");

            migrationBuilder.DropTable(
                name: "T");

            migrationBuilder.DropTable(
                name: "W");

            migrationBuilder.DropTable(
                name: "Y");
        }
    }
}
