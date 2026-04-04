using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class AddGiaToChiTiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gia",
                table: "ChiTietThanhToans");
        }
    }
}
