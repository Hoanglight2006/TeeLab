using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailAndAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Nguois",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Nguois",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Nguois",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Avatar", "Email" },
                values: new object[] { "default-avatar.png", "admin@teelab.vn" });

            migrationBuilder.UpdateData(
                table: "Nguois",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Avatar", "Email" },
                values: new object[] { "default-avatar.png", "nhanvien@teelab.vn" });

            migrationBuilder.UpdateData(
                table: "Nguois",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Avatar", "Email" },
                values: new object[] { "default-avatar.png", "hoang@gmail.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Nguois");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Nguois");
        }
    }
}
