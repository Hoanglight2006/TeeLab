using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeLab.Migrations
{
    /// <inheritdoc />
    public partial class initCreateMota : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThanhToans_SanPhams_MaSP",
                table: "ChiTietThanhToans");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThanhToans_ThanhToans_MaTT",
                table: "ChiTietThanhToans");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "ThanhToans",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucTT",
                table: "ThanhToans",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TenSP",
                table: "SanPhams",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaTT",
                table: "ChiTietThanhToans",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "MaSP",
                table: "ChiTietThanhToans",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AH121",
                column: "MoTa",
                value: "Hoodie zip form rộng với họa tiết Stars nổi bật. Chất nỉ dày dặn, giữ ấm tốt, phù hợp mặc trong thời tiết se lạnh.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT052",
                column: "MoTa",
                value: "Áo sơ mi ngắn tay chất Oxford cao cấp, form rộng thoải mái. Thiết kế đơn giản, phù hợp mặc đi học, đi làm hoặc đi chơi.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT066",
                column: "MoTa",
                value: "Áo sơ mi dài tay với chất liệu Oxford dày dặn, đứng form. Phù hợp phong cách lịch sự nhưng vẫn trẻ trung.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT068",
                column: "MoTa",
                value: "Áo sơ mi cộc tay với thiết kế logo signature tinh tế. Chất liệu Oxford thân thiện, thoáng mát, phù hợp mặc hằng ngày.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT074",
                column: "MoTa",
                value: "Áo polo dệt kim cao cấp với form vừa vặn. Thiết kế thanh lịch, phù hợp cho cả đi làm và đi chơi.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT188",
                column: "MoTa",
                value: "Áo thun oversize với họa tiết Wave Line hiện đại. Chất cotton cao cấp giúp thoáng mát, dễ chịu khi mặc. Thiết kế đơn giản nhưng vẫn tạo điểm nhấn.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT376",
                column: "MoTa",
                value: "Áo thun oversize với họa tiết World Tour TOKYO phong cách watercolor độc đáo. Chất cotton cao cấp mang lại cảm giác thoải mái, phù hợp cho phong cách streetwear năng động.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT377",
                column: "MoTa",
                value: "Áo thun sọc form rộng với điểm nhấn thêu Pop Star tinh tế. Chất liệu cotton mềm mại, thoáng mát, mang lại phong cách trẻ trung và cá tính.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT382",
                column: "MoTa",
                value: "Áo thun form oversize basic với chất liệu cotton 250GSM dày dặn, mềm mại và thoáng khí. Thiết kế trơn tối giản, dễ phối đồ, phù hợp mặc hằng ngày hoặc đi chơi.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "PK057",
                column: "MoTa",
                value: "Tất cổ cao với thiết kế logo Teelab nổi bật. Chất liệu cotton co giãn tốt, thoáng khí, mang lại cảm giác dễ chịu khi sử dụng.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "PK085",
                column: "MoTa",
                value: "Balo da thiết kế tối giản, sang trọng. Chất liệu da bền đẹp, nhiều ngăn tiện dụng, phù hợp đi học, đi làm hoặc du lịch.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "PK112",
                column: "MoTa",
                value: "Nón pillbox phong cách vintage, tạo điểm nhấn cho outfit. Thiết kế độc đáo, dễ phối với nhiều phong cách thời trang.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "Q003",
                column: "MoTa",
                value: "Quần short nỉ form rộng với chất vải mềm mại, co giãn tốt. Thiết kế in World Tour nổi bật, phù hợp mặc đi chơi, thể thao hoặc ở nhà.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "Q116",
                column: "MoTa",
                value: "Quần jeans ống rộng phong cách local brand, dễ phối đồ. Chất denim bền bỉ, form rộng thoải mái phù hợp nhiều phong cách khác nhau.");

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "Q131",
                column: "MoTa",
                value: "Quần nỉ ống suông form rộng, mang lại sự thoải mái khi vận động. Họa tiết in World Tour tạo phong cách streetwear năng động.");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThanhToans_SanPhams_MaSP",
                table: "ChiTietThanhToans",
                column: "MaSP",
                principalTable: "SanPhams",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThanhToans_ThanhToans_MaTT",
                table: "ChiTietThanhToans",
                column: "MaTT",
                principalTable: "ThanhToans",
                principalColumn: "MaTT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThanhToans_SanPhams_MaSP",
                table: "ChiTietThanhToans");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThanhToans_ThanhToans_MaTT",
                table: "ChiTietThanhToans");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "ThanhToans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucTT",
                table: "ThanhToans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenSP",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MaTT",
                table: "ChiTietThanhToans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSP",
                table: "ChiTietThanhToans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AH121",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT052",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT066",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT068",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT074",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT188",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT376",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT377",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "AT382",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "PK057",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "PK085",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "PK112",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "Q003",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "Q116",
                column: "MoTa",
                value: null);

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "MaSP",
                keyValue: "Q131",
                column: "MoTa",
                value: null);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThanhToans_SanPhams_MaSP",
                table: "ChiTietThanhToans",
                column: "MaSP",
                principalTable: "SanPhams",
                principalColumn: "MaSP",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThanhToans_ThanhToans_MaTT",
                table: "ChiTietThanhToans",
                column: "MaTT",
                principalTable: "ThanhToans",
                principalColumn: "MaTT",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
