using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class AddLockAndRankToKhachHang : Migration
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

            migrationBuilder.UpdateData(
                table: "Nguois",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HangThanhVien", "IsLocked" },
                values: new object[] { "Đồng", false });
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
        }
    }
}
