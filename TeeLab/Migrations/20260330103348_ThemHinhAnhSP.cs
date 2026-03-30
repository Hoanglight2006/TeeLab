using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class ThemHinhAnhSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HinhAnh",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT001",
                column: "HinhAnh",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT002",
                column: "HinhAnh",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "Hoc",
                column: "HinhAnh",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhAnh",
                table: "SanPhams");
        }
    }
}
