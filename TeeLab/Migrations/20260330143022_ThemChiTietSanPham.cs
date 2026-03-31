using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class ThemChiTietSanPham : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TinhTrang",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "KichThuoc",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MauSac",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT001",
                columns: new[] { "KichThuoc", "MauSac", "MoTa" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT002",
                columns: new[] { "KichThuoc", "MauSac", "MoTa" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "Hoc",
                columns: new[] { "KichThuoc", "MauSac", "MoTa" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KichThuoc",
                table: "SanPhams");

            migrationBuilder.DropColumn(
                name: "MauSac",
                table: "SanPhams");

            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "SanPhams");

            migrationBuilder.AlterColumn<string>(
                name: "TinhTrang",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
