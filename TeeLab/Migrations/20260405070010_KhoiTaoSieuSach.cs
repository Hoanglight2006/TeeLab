using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class KhoiTaoSieuSach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nguois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hoten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diachi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ngaysinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sdt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenDangNhap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    HangThanhVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nguois", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenSP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KichThuoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MauSac = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.MaSP);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToans",
                columns: table => new
                {
                    MaTT = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToans", x => x.MaTT);
                    table.ForeignKey(
                        name: "FK_ThanhToans_Nguois_Id",
                        column: x => x.Id,
                        principalTable: "Nguois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietThanhToans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTT = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    KichThuoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MauSac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gia = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietThanhToans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietThanhToans_SanPhams_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPhams",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietThanhToans_ThanhToans_MaTT",
                        column: x => x.MaTT,
                        principalTable: "ThanhToans",
                        principalColumn: "MaTT",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Nguois",
                columns: new[] { "Id", "Avatar", "Diachi", "Discriminator", "Email", "Hoten", "MatKhau", "Ngaysinh", "Sdt", "TenDangNhap" },
                values: new object[,]
                {
                    { 1, "default-avatar.png", "Thái Nguyên", "QuanLy", "admin@teelab.vn", "Dương Đình Hoàng", "123", null, "0123456789", "admin" },
                    { 2, "default-avatar.png", "Hà Nội", "NhanVien", "nhanvien@teelab.vn", "Nguyễn Nhân Viên", "123", null, "0988888888", "nhanvien" }
                });

            migrationBuilder.InsertData(
                table: "Nguois",
                columns: new[] { "Id", "Avatar", "Diachi", "Discriminator", "Email", "HangThanhVien", "Hoten", "IsLocked", "MatKhau", "Ngaysinh", "Sdt", "TenDangNhap" },
                values: new object[] { 3, "default-avatar.png", "Hải Phòng", "KhachHang", "hoang@gmail.com", null, "Lê Minh Hoàng", false, "123", null, "0977777777", "hoang" });

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "MaSP", "HinhAnh", "KichThuoc", "MauSac", "MoTa", "SoLuong", "SoTien", "TenSP", "TinhTrang" },
                values: new object[,]
                {
                    { "AC057", "tat.jpg", null, "Đen, Trắng, Kem, Vàng, Xanh lá", null, 25, 25000m, "Tất Teelab Iconic Logo Socks AC057", "Còn hàng" },
                    { "AC085", "balo.jpg", null, null, null, 3, 340000m, "Balo Da Teelab Local Brand Essentials Leather Backpack AC085", "Còn hàng" },
                    { "AC112", "nonda.jpg", null, "Tweed Caro, Sọc, Đen, Da beo", null, 26, 85000m, "Nón Pillbox Local Brand Unisex Teelab Alter AC112", "Còn hàng" },
                    { "AP074", "xamtieu.jpg", "M, L, XL", "Đen,Xanh Navy, Melane", null, 0, 320000m, "Áo Polo Local Brand Unisex Teelab KNIT POLO SHIRT AP074", "Hết hàng" },
                    { "AT001", "SS052.jpg", "S, M, L, XL", "Hồng, Xanh, Xám", null, 6, 250000m, "Áo Sơ Mi Ngắn Tay Teelab Local Brand Unisex Studio Oxford Shirt SS052", "Còn hàng" },
                    { "HD001", "somidai.jpg", "S, M, L, XL", "Đen, Trắng, Xanh than, Xanh dương", null, 4, 280000m, "Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Oxford shirts SS066", "Còn hàng" },
                    { "HD121", "hutdi.jpg", "S, M, L, XL", "Đen, Trắng, Xám, Nâu", null, 6, 399000m, "Áo Teelab Local Brand Unisex Hoodie zip Stars HD121", "Còn hàng" },
                    { "PS116", "quanbo.jpg", "S, M, L, XL", "Đen Wash, Trắng Wash, Xanh Wash, Xám đen Wash", null, 20, 250000m, "Quần Dài Local Brand Unisex Teelab Jeans Ống Rộng PS116", "Còn hàng" },
                    { "PS131", "quanni.jpg", "S, M, L, XL", "Đen, Xanh Navy, Xám trắng", null, 50, 250000m, "Quần Nỉ Ống Suông Teelab Alter Oversize Nỉ In World Tour PS131", "Còn hàng" },
                    { "SH003", "quandui.jpg", "S, M ,L ,XL", "Đen, Trắng", null, 100, 220000m, "Quần Short Teelab Alter Oversize Nỉ Chân Cua In Wourld Tour Unisex SH003", "Còn hàng" },
                    { "SS068", "SS068.jpg", "S, M, L, XL", "Đen, Trắng, Xanh than, Xanh dương, Hồng", null, 12, 250000m, "Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Eco Oxford Logo Signature Shirt SS068", "Còn hàng" },
                    { "TS188", "soc.jpg", "S, M, L, XL", "Đen", null, 0, 250000m, "Áo Thun Teelab Alter Oversize Cotton In Wave Line Unisex TS188", "Hết hàng" },
                    { "TS376", "aoxam.jpg", "S, M, L, XL", "Trắng, Xám", null, 3, 280000m, "Áo Thun Teelab Alter Oversize Cotton In World Tour TOKYO Water Color Unisex TS376", "Còn hàng" },
                    { "TS377", "aoxoc.jpg", "S, M ,L, XL", "Kẻ sọc nâu, Kẻ sọc xanh", null, 20, 280000m, "Áo Thun Sọc Teelab Alter Oversize Cotton Thêu Pop Star Unisex TS377", "Còn hàng" },
                    { "TS379", "dodo.jpg", "S, M, L, XL", "Đen, Trắng, Xanh Navy, Đỏ đô, Xám tiêu", null, 100, 250000m, "Áo Thun Teelab Alter Oversize Cotton In Essentials Unisex TS379", "Còn hàng" },
                    { "TS382", "aotrang.jpg", "S, M, L, XL", "Đen, Trắng, Xám tiêu", null, 50, 250000m, "Áo Thun Teelab Alter Oversize Cotton 250GSM Trơn Unisex TS382", "Còn hàng" }
                });

            migrationBuilder.InsertData(
                table: "ThanhToans",
                columns: new[] { "MaTT", "Id", "NgayTao", "TongTien", "TrangThai" },
                values: new object[,]
                {
                    { "HD_SAMPLE_01", 3, new DateTime(2026, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 500000m, "Giao hàng thành công" },
                    { "HD_SAMPLE_02", 3, new DateTime(2026, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 450000m, "Chờ xác nhận" }
                });

            migrationBuilder.InsertData(
                table: "ChiTietThanhToans",
                columns: new[] { "Id", "Gia", "KichThuoc", "MaSP", "MaTT", "MauSac", "SoLuong" },
                values: new object[,]
                {
                    { 1, 250000.0, "M", "TS382", "HD_SAMPLE_01", "Đen", 2 },
                    { 2, 399000.0, "XL", "HD121", "HD_SAMPLE_02", "Xám", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThanhToans_MaSP",
                table: "ChiTietThanhToans",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThanhToans_MaTT",
                table: "ChiTietThanhToans",
                column: "MaTT");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_Id",
                table: "ThanhToans",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietThanhToans");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "ThanhToans");

            migrationBuilder.DropTable(
                name: "Nguois");
        }
    }
}
