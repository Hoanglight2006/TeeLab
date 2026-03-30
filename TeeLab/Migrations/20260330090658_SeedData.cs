using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Nguois",
                columns: new[] { "Id", "Diachi", "Discriminator", "Hoten", "MatKhau", "Ngaysinh", "Sdt", "TenDangNhap" },
                values: new object[] { 1, null, "QuanLy", "Admin", "123456", null, "0123456789", "admin" });

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "MaSP", "SoTien", "TenSP", "TinhTrang" },
                values: new object[,]
                {
                    { "AT001", 250000m, "Áo thun Teelab Basic", "Còn hàng" },
                    { "AT002", 450000m, "Áo Hoodie Teelab", "Còn hàng" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nguois",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT001");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT002");
        }
    }
}
