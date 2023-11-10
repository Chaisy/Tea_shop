using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_153505_Shevtsova_D.API.Migrations
{
    /// <inheritdoc />
    public partial class upt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_teas_basesType_CategoryId",
                table: "teas");

            migrationBuilder.AlterColumn<string>(
                name: "MIMEType",
                table: "teas",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "teas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_teas_basesType_CategoryId",
                table: "teas",
                column: "CategoryId",
                principalTable: "basesType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_teas_basesType_CategoryId",
                table: "teas");

            migrationBuilder.AlterColumn<string>(
                name: "MIMEType",
                table: "teas",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "teas",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_teas_basesType_CategoryId",
                table: "teas",
                column: "CategoryId",
                principalTable: "basesType",
                principalColumn: "Id");
        }
    }
}
