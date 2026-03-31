using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class InitialProject : Migration
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
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
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
                    MaTT = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    KichThuoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MauSac = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietThanhToans", x => new { x.MaTT, x.MaSP });
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
                columns: new[] { "Id", "Diachi", "Discriminator", "Hoten", "MatKhau", "Ngaysinh", "Sdt", "TenDangNhap" },
                values: new object[,]
                {
                    { 1, "Thái Nguyên", "QuanLy", "Dương Đình Hoàng", "123", null, "0123456789", "admin" },
                    { 2, "Hà Nội", "NhanVien", "Nguyễn Nhân Viên", "123", null, "0988888888", "nhanvien" },
                    { 3, "Hải Phòng", "KhachHang", "Lê Minh Hoàng", "123", null, "0977777777", "hoang" }
                });

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "MaSP", "HinhAnh", "KichThuoc", "MauSac", "MoTa", "SoLuong", "SoTien", "TenSP", "TinhTrang" },
                values: new object[,]
                {
                    { "AT001", "at001.jpg", "S, M, L, XL", "Đen, Trắng", null, 50, 250000m, "Áo thun Teelab Basic", "Còn hàng" },
                    { "AT002", "at002.jpg", "M, L", "Trắng", null, 3, 290000m, "Áo thun Rabbit Edition", "Còn hàng" },
                    { "HD001", "hd001.jpg", "L, XL", "Xám", null, 20, 450000m, "Hoodie Teelab Signature", "Còn hàng" },
                    { "PK001", "pk001.jpg", "Free size", "Đen", null, 100, 150000m, "Mũ Cap Teelab", "Còn hàng" }
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
                columns: new[] { "MaSP", "MaTT", "KichThuoc", "MauSac", "SoLuong" },
                values: new object[,]
                {
                    { "AT001", "HD_SAMPLE_01", "M", "Đen", 2 },
                    { "HD001", "HD_SAMPLE_02", "XL", "Xám", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThanhToans_MaSP",
                table: "ChiTietThanhToans",
                column: "MaSP");

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
