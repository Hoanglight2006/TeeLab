using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT001");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT002");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "HD001");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "PK001");

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "MaSP", "HinhAnh", "KichThuoc", "MauSac", "MoTa", "SoLuong", "SoTien", "TenSP", "TinhTrang" },
                values: new object[,]
                {
                    { "AC057", "tat.jpg", null, "Đen, Trắng, Kem, Vàng, Xanh lá", null, 25, 25000m, "Tất Teelab Iconic Logo Socks AC057", "Còn hàng" },
                    { "AC085", "balo.jpg", null, null, null, 3, 340000m, "Balo Da Teelab Local Brand Essentials Leather Backpack AC085", "Còn hàng" },
                    { "AC112", "nonda.jpg", null, "Tweed Caro, Sọc, Đen, Da beo", null, 26, 85000m, "Nón Pillbox Local Brand Unisex Teelab Alter AC112", "Còn hàng" },
                    { "AP074", "xamtieu.jpg", "M, L, XL", "Đen,Xanh Navy, Melane", null, 0, 320000m, "Áo Polo Local Brand Unisex Teelab KNIT POLO SHIRT AP074", "Hết hàng" },
                    { "HD121", "hutdi.jpg", "S, M, L, XL", "Đen, Trắng, Xám, Nâu", null, 6, 399000m, "Áo Teelab Local Brand Unisex Hoodie zip Stars HD121", "Còn hàng" },
                    { "PS116", "quanbo.jpg", "S, M, L, XL", "Đen Wash, Trắng Wash, Xanh Wash, Xám đen Wash", null, 20, 250000m, "Quần Dài Local Brand Unisex Teelab Jeans Ống Rộng PS116", "Còn hàng" },
                    { "PS131", "quanni.jpg", "S, M, L, XL", "Đen, Xanh Navy, Xám trắng", null, 50, 250000m, "Quần Nỉ Ống Suông Teelab Alter Oversize Nỉ In World Tour PS131", "Còn hàng" },
                    { "SH003", "quandui.jpg", "S, M ,L ,XL", "Đen, Trắng", null, 100, 220000m, "Quần Short Teelab Alter Oversize Nỉ Chân Cua In Wourld Tour Unisex SH003", "Còn hàng" },
                    { "SS052", "SS052.jpg", "S, M, L, XL", "Hồng, Xanh, Xám", null, 6, 250000m, "Áo Sơ Mi Ngắn Tay Teelab Local Brand Unisex Studio Oxford Shirt SS052", "Còn hàng" },
                    { "SS066", "somidai.jpg", "S, M, L, XL", "Đen, Trắng, Xanh than, Xanh dương", null, 4, 280000m, "Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Oxford shirts SS066", "Còn hàng" },
                    { "SS068", "SS068.jpg", "S, M, L, XL", "Đen, Trắng, Xanh than, Xanh dương, Hồng", null, 12, 250000m, "Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Eco Oxford Logo Signature Shirt SS068", "Còn hàng" },
                    { "TS188", "soc.jpg", "S, M, L, XL", "Đen", null, 0, 250000m, "Áo Thun Teelab Alter Oversize Cotton In Wave Line Unisex TS188", "Hết hàng" },
                    { "TS376", "aoxam.jpg", "S, M, L, XL", "Trắng, Xám", null, 3, 280000m, "Áo Thun Teelab Alter Oversize Cotton In World Tour TOKYO Water Color Unisex TS376", "Còn hàng" },
                    { "TS377", "aoxoc.jpg", "S, M ,L, XL", "Kẻ sọc nâu, Kẻ sọc xanh", null, 20, 280000m, "Áo Thun Sọc Teelab Alter Oversize Cotton Thêu Pop Star Unisex TS377", "Còn hàng" },
                    { "TS379", "dodo.jpg", "S, M, L, XL", "Đen, Trắng, Xanh Navy, Đỏ đô, Xám tiêu", null, 100, 250000m, "Áo Thun Teelab Alter Oversize Cotton In Essentials Unisex TS379", "Còn hàng" },
                    { "TS382", "aotrang.jpg", "S, M, L, XL", "Đen, Trắng, Xám tiêu", null, 50, 250000m, "Áo Thun Teelab Alter Oversize Cotton 250GSM Trơn Unisex TS382", "Còn hàng" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AC057");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AC085");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AC112");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AP074");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "HD121");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "PS116");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "PS131");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "SH003");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "SS052");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "SS066");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "SS068");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "TS188");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "TS376");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "TS377");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "TS379");

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "TS382");

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
        }
    }
}
