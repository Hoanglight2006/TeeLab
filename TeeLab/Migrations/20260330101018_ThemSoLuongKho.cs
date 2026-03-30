using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class ThemSoLuongKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoLuong",
                table: "SanPhams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT001",
                column: "SoLuong",
                value: 50);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT002",
                column: "SoLuong",
                value: 10);

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "MaSP", "SoLuong", "SoTien", "TenSP", "TinhTrang" },
                values: new object[] { "Hoc", 0, 36000m, "Học", "Hết hàng" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "Hoc");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "SanPhams");
        }
    }
}
