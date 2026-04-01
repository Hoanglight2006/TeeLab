using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarAndProfileUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Nguois",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Nguois",
                keyColumn: "Id",
                keyValue: 1,
                column: "Avatar",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Nguois");
        }
    }
}
