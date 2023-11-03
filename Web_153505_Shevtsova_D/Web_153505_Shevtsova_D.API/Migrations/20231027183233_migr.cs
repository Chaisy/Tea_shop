using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_153505_Shevtsova_D.API.Migrations
{
    /// <inheritdoc />
    public partial class migr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MIMEType",
                table: "teas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MIMEType",
                table: "teas");
        }
    }
}
