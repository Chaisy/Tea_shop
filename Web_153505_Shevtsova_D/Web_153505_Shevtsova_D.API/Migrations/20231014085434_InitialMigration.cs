using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_153505_Shevtsova_D.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "basesType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NormalizedName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basesType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "teas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    PhotoPath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teas_basesType_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "basesType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_teas_CategoryId",
                table: "teas",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "teas");

            migrationBuilder.DropTable(
                name: "basesType");
        }
    }
}
