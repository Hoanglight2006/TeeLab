using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BotResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatHistory", x => x.Id);
                });

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
                    MatKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    TenSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KichThuoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MauSac = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.MaSP);
                });

            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    HangThanhVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KhachHangs_Nguois_Id",
                        column: x => x.Id,
                        principalTable: "Nguois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhanViens_Nguois_Id",
                        column: x => x.Id,
                        principalTable: "Nguois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuanLys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuanLys_Nguois_Id",
                        column: x => x.Id,
                        principalTable: "Nguois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToans",
                columns: table => new
                {
                    MaTT = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    PhuongThucTT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToans", x => x.MaTT);
                    table.ForeignKey(
                        name: "FK_ThanhToans_KhachHangs_Id",
                        column: x => x.Id,
                        principalTable: "KhachHangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietThanhToans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTT = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        principalColumn: "MaSP");
                    table.ForeignKey(
                        name: "FK_ChiTietThanhToans_ThanhToans_MaTT",
                        column: x => x.MaTT,
                        principalTable: "ThanhToans",
                        principalColumn: "MaTT");
                });

            migrationBuilder.InsertData(
                table: "Nguois",
                columns: new[] { "Id", "Avatar", "Diachi", "Email", "Hoten", "MatKhau", "Ngaysinh", "Sdt", "TenDangNhap" },
                values: new object[,]
                {
                    { 1, "default-avatar.png", "Thái Nguyên", "admin@teelab.vn", "ADMIN", "$2a$12$g9A8wOWOaOukKb52yMPeru.OAgvWjVQF6N7AkowgHFgSoeKjGAWtm", null, "0123456789", "admin" },
                    { 2, "default-avatar.png", "Hà Nội", "Staff@teelab.vn", "Staff", "$2a$12$g9A8wOWOaOukKb52yMPeru.OAgvWjVQF6N7AkowgHFgSoeKjGAWtm", null, "0988888888", "Staff" },
                    { 3, "default-avatar.png", "Hải Phòng", "Customer@gmail.com", "Customer", "$2a$12$g9A8wOWOaOukKb52yMPeru.OAgvWjVQF6N7AkowgHFgSoeKjGAWtm", null, "0977777777", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "MaSP", "HinhAnh", "KichThuoc", "MauSac", "MoTa", "SoLuong", "SoTien", "TenSP", "TinhTrang" },
                values: new object[,]
                {
                    { "AH121", "hutdi.jpg", "S, M, L, XL", "Đen, Trắng, Xám, Nâu", "Hoodie zip form rộng với họa tiết Stars nổi bật. Chất nỉ dày dặn, giữ ấm tốt, phù hợp mặc trong thời tiết se lạnh.", 6, 399000m, "Áo Teelab Local Brand Unisex Hoodie zip Stars HD121", "Còn hàng" },
                    { "AT052", "SS052.jpg", "S, M, L, XL", "Hồng, Xanh, Xám", "Áo sơ mi ngắn tay chất Oxford cao cấp, form rộng thoải mái. Thiết kế đơn giản, phù hợp mặc đi học, đi làm hoặc đi chơi.", 6, 250000m, "Áo Sơ Mi Ngắn Tay Teelab Local Brand Unisex Studio Oxford Shirt SS052", "Còn hàng" },
                    { "AT066", "somidai.jpg", "S, M, L, XL", "Đen, Trắng, Xanh than, Xanh dương", "Áo sơ mi dài tay với chất liệu Oxford dày dặn, đứng form. Phù hợp phong cách lịch sự nhưng vẫn trẻ trung.", 4, 280000m, "Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Oxford shirts SS066", "Còn hàng" },
                    { "AT068", "SS068.jpg", "S, M, L, XL", "Đen, Trắng, Xanh than, Xanh dương, Hồng", "Áo sơ mi cộc tay với thiết kế logo signature tinh tế. Chất liệu Oxford thân thiện, thoáng mát, phù hợp mặc hằng ngày.", 12, 250000m, "Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Eco Oxford Logo Signature Shirt SS068", "Còn hàng" },
                    { "AT074", "xamtieu.jpg", "M, L, XL", "Đen,Xanh Navy, Melane", "Áo polo dệt kim cao cấp với form vừa vặn. Thiết kế thanh lịch, phù hợp cho cả đi làm và đi chơi.", 0, 320000m, "Áo Polo Local Brand Unisex Teelab KNIT POLO SHIRT AP074", "Hết hàng" },
                    { "AT188", "soc.jpg", "S, M, L, XL", "Đen", "Áo thun oversize với họa tiết Wave Line hiện đại. Chất cotton cao cấp giúp thoáng mát, dễ chịu khi mặc. Thiết kế đơn giản nhưng vẫn tạo điểm nhấn.", 0, 250000m, "Áo Thun Teelab Alter Oversize Cotton In Wave Line Unisex TS188", "Hết hàng" },
                    { "AT376", "aoxam.jpg", "S, M, L, XL", "Trắng, Xám", "Áo thun oversize với họa tiết World Tour TOKYO phong cách watercolor độc đáo. Chất cotton cao cấp mang lại cảm giác thoải mái, phù hợp cho phong cách streetwear năng động.", 3, 280000m, "Áo Thun Teelab Alter Oversize Cotton In World Tour TOKYO Water Color Unisex TS376", "Còn hàng" },
                    { "AT377", "aoxoc.jpg", "S, M ,L, XL", "Kẻ sọc nâu, Kẻ sọc xanh", "Áo thun sọc form rộng với điểm nhấn thêu Pop Star tinh tế. Chất liệu cotton mềm mại, thoáng mát, mang lại phong cách trẻ trung và cá tính.", 20, 280000m, "Áo Thun Sọc Teelab Alter Oversize Cotton Thêu Pop Star Unisex TS377", "Còn hàng" },
                    { "AT382", "aotrang.jpg", "S, M, L, XL", "Đen, Trắng, Xám tiêu", "Áo thun form oversize basic với chất liệu cotton 250GSM dày dặn, mềm mại và thoáng khí. Thiết kế trơn tối giản, dễ phối đồ, phù hợp mặc hằng ngày hoặc đi chơi.", 50, 250000m, "Áo Thun Teelab Alter Oversize Cotton 250GSM Trơn Unisex TS382", "Còn hàng" },
                    { "PK057", "tat.jpg", null, "Đen, Trắng, Kem, Vàng, Xanh lá", "Tất cổ cao với thiết kế logo Teelab nổi bật. Chất liệu cotton co giãn tốt, thoáng khí, mang lại cảm giác dễ chịu khi sử dụng.", 25, 25000m, "Tất Teelab Iconic Logo Socks AC057", "Còn hàng" },
                    { "PK085", "balo.jpg", null, null, "Balo da thiết kế tối giản, sang trọng. Chất liệu da bền đẹp, nhiều ngăn tiện dụng, phù hợp đi học, đi làm hoặc du lịch.", 3, 340000m, "Balo Da Teelab Local Brand Essentials Leather Backpack AC085", "Còn hàng" },
                    { "PK112", "nonda.jpg", null, "Tweed Caro, Sọc, Đen, Da beo", "Nón pillbox phong cách vintage, tạo điểm nhấn cho outfit. Thiết kế độc đáo, dễ phối với nhiều phong cách thời trang.", 26, 85000m, "Nón Pillbox Local Brand Unisex Teelab Alter AC112", "Còn hàng" },
                    { "Q003", "quandui.jpg", "S, M ,L ,XL", "Đen, Trắng", "Quần short nỉ form rộng với chất vải mềm mại, co giãn tốt. Thiết kế in World Tour nổi bật, phù hợp mặc đi chơi, thể thao hoặc ở nhà.", 100, 220000m, "Quần Short Teelab Alter Oversize Nỉ Chân Cua In Wourld Tour Unisex SH003", "Còn hàng" },
                    { "Q116", "quanbo.jpg", "S, M, L, XL", "Đen Wash, Trắng Wash, Xanh Wash, Xám đen Wash", "Quần jeans ống rộng phong cách local brand, dễ phối đồ. Chất denim bền bỉ, form rộng thoải mái phù hợp nhiều phong cách khác nhau.", 20, 250000m, "Quần Dài Local Brand Unisex Teelab Jeans Ống Rộng PS116", "Còn hàng" },
                    { "Q131", "quanni.jpg", "S, M, L, XL", "Đen, Xanh Navy, Xám trắng", "Quần nỉ ống suông form rộng, mang lại sự thoải mái khi vận động. Họa tiết in World Tour tạo phong cách streetwear năng động.", 50, 250000m, "Quần Nỉ Ống Suông Teelab Alter Oversize Nỉ In World Tour PS131", "Còn hàng" }
                });

            migrationBuilder.InsertData(
                table: "KhachHangs",
                columns: new[] { "Id", "HangThanhVien", "IsLocked" },
                values: new object[] { 3, null, false });

            migrationBuilder.InsertData(
                table: "NhanViens",
                column: "Id",
                value: 2);

            migrationBuilder.InsertData(
                table: "QuanLys",
                column: "Id",
                value: 1);

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
                name: "ChatHistory");

            migrationBuilder.DropTable(
                name: "ChiTietThanhToans");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "QuanLys");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "ThanhToans");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.DropTable(
                name: "Nguois");
        }
    }
}
