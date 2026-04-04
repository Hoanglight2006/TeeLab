using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HangThanhVien",
                table: "Nguois",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Nguois",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Gia",
                table: "ChiTietThanhToans",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "ChiTietThanhToans",
                keyColumns: new[] { "MaSP", "MaTT" },
                keyValues: new object[] { "AT001", "HD_SAMPLE_01" },
                column: "Gia",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "ChiTietThanhToans",
                keyColumns: new[] { "MaSP", "MaTT" },
                keyValues: new object[] { "HD001", "HD_SAMPLE_02" },
                column: "Gia",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "Nguois",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HangThanhVien", "IsLocked" },
                values: new object[] { null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HangThanhVien",
                table: "Nguois");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Nguois");

            migrationBuilder.DropColumn(
                name: "Gia",
                table: "ChiTietThanhToans");
        }
    }
}
