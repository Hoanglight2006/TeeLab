using Microsoft.EntityFrameworkCore;
using TeeLab.Models;
using Teelab.Models;
using System;

namespace Teelab.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Nguoi> Nguois { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<QuanLy> QuanLys { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<ChiTietThanhToan> ChiTietThanhToans { get; set; }
        public DbSet<ChatHistory> ChatHistory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // 1. SEED DATA: TÀI KHOẢN
            modelBuilder.Entity<QuanLy>().HasData(new QuanLy { Id = 1, Hoten = "ADMIN", TenDangNhap = "admin", MatKhau = "$2a$12$g9A8wOWOaOukKb52yMPeru.OAgvWjVQF6N7AkowgHFgSoeKjGAWtm", Sdt = "0123456789", Diachi = "Thái Nguyên", Email = "admin@teelab.vn", Avatar = "default-avatar.png" });
            modelBuilder.Entity<NhanVien>().HasData(new NhanVien { Id = 2, Hoten = "Staff", TenDangNhap = "Staff", MatKhau = "$2a$12$g9A8wOWOaOukKb52yMPeru.OAgvWjVQF6N7AkowgHFgSoeKjGAWtm", Sdt = "0988888888", Diachi = "Hà Nội", Email = "Staff@teelab.vn", Avatar = "default-avatar.png" });
            modelBuilder.Entity<KhachHang>().HasData(new KhachHang { Id = 3, Hoten = "Customer", TenDangNhap = "Customer", MatKhau = "$2a$12$g9A8wOWOaOukKb52yMPeru.OAgvWjVQF6N7AkowgHFgSoeKjGAWtm", Sdt = "0977777777", Diachi = "Hải Phòng", Email = "Customer@gmail.com", Avatar = "default-avatar.png" });

            modelBuilder.Entity<Nguoi>().ToTable("Nguois");
            modelBuilder.Entity<KhachHang>().ToTable("KhachHangs");
            modelBuilder.Entity<NhanVien>().ToTable("NhanViens");

            modelBuilder.Entity<SanPham>().HasData(
new SanPham
{
         MaSP = "AT382",
         TenSP = "Áo Thun Teelab Alter Oversize Cotton 250GSM Trơn Unisex TS382",
         SoTien = 250000,
         SoLuong = 50,
         TinhTrang = "Còn hàng",
         HinhAnh = "aotrang.jpg",
         KichThuoc = "S, M, L, XL",
         MauSac = "Đen, Trắng, Xám tiêu",
         MoTa = "Áo thun form oversize basic với chất liệu cotton 250GSM dày dặn, mềm mại và thoáng khí. Thiết kế trơn tối giản, dễ phối đồ, phù hợp mặc hằng ngày hoặc đi chơi."
},

new SanPham
{
    MaSP = "AT376",
    TenSP = "Áo Thun Teelab Alter Oversize Cotton In World Tour TOKYO Water Color Unisex TS376",
    SoTien = 280000,
    SoLuong = 3,
    TinhTrang = "Còn hàng",
    HinhAnh = "aoxam.jpg",
    KichThuoc = "S, M, L, XL",
    MauSac = "Trắng, Xám",
    MoTa = "Áo thun oversize với họa tiết World Tour TOKYO phong cách watercolor độc đáo. Chất cotton cao cấp mang lại cảm giác thoải mái, phù hợp cho phong cách streetwear năng động."
},

new SanPham
{
    MaSP = "AT377",
    TenSP = "Áo Thun Sọc Teelab Alter Oversize Cotton Thêu Pop Star Unisex TS377",
    SoTien = 280000,
    SoLuong = 20,
    TinhTrang = "Còn hàng",
    HinhAnh = "aoxoc.jpg",
    KichThuoc = "S, M ,L, XL",
    MauSac = "Kẻ sọc nâu, Kẻ sọc xanh",
    MoTa = "Áo thun sọc form rộng với điểm nhấn thêu Pop Star tinh tế. Chất liệu cotton mềm mại, thoáng mát, mang lại phong cách trẻ trung và cá tính."
},

new SanPham
{
    MaSP = "Q003",
    TenSP = "Quần Short Teelab Alter Oversize Nỉ Chân Cua In Wourld Tour Unisex SH003",
    SoTien = 220000,
    SoLuong = 100,
    TinhTrang = "Còn hàng",
    HinhAnh = "quandui.jpg",
    KichThuoc = "S, M ,L ,XL",
    MauSac = "Đen, Trắng",
    MoTa = "Quần short nỉ form rộng với chất vải mềm mại, co giãn tốt. Thiết kế in World Tour nổi bật, phù hợp mặc đi chơi, thể thao hoặc ở nhà."
},

new SanPham
{
    MaSP = "AT188",
    TenSP = "Áo Thun Teelab Alter Oversize Cotton In Wave Line Unisex TS188",
    SoTien = 250000,
    SoLuong = 0,
    TinhTrang = "Hết hàng",
    HinhAnh = "soc.jpg",
    KichThuoc = "S, M, L, XL",
    MauSac = "Đen",
    MoTa = "Áo thun oversize với họa tiết Wave Line hiện đại. Chất cotton cao cấp giúp thoáng mát, dễ chịu khi mặc. Thiết kế đơn giản nhưng vẫn tạo điểm nhấn."
},

new SanPham
{
    MaSP = "Q131",
    TenSP = "Quần Nỉ Ống Suông Teelab Alter Oversize Nỉ In World Tour PS131",
    SoTien = 250000,
    SoLuong = 50,
    TinhTrang = "Còn hàng",
    HinhAnh = "quanni.jpg",
    KichThuoc = "S, M, L, XL",
    MauSac = "Đen, Xanh Navy, Xám trắng",
    MoTa = "Quần nỉ ống suông form rộng, mang lại sự thoải mái khi vận động. Họa tiết in World Tour tạo phong cách streetwear năng động."
},

new SanPham
{
    MaSP = "Q116",
    TenSP = "Quần Dài Local Brand Unisex Teelab Jeans Ống Rộng PS116",
    SoTien = 250000,
    SoLuong = 20,
    TinhTrang = "Còn hàng",
    HinhAnh = "quanbo.jpg",
    KichThuoc = "S, M, L, XL",
    MauSac = "Đen Wash, Trắng Wash, Xanh Wash, Xám đen Wash",
    MoTa = "Quần jeans ống rộng phong cách local brand, dễ phối đồ. Chất denim bền bỉ, form rộng thoải mái phù hợp nhiều phong cách khác nhau."
},

new SanPham
{
    MaSP = "AH121",
    TenSP = "Áo Teelab Local Brand Unisex Hoodie zip Stars HD121",
    SoTien = 399000,
    SoLuong = 6,
    TinhTrang = "Còn hàng",
    HinhAnh = "hutdi.jpg",
    KichThuoc = "S, M, L, XL",
    MauSac = "Đen, Trắng, Xám, Nâu",
    MoTa = "Hoodie zip form rộng với họa tiết Stars nổi bật. Chất nỉ dày dặn, giữ ấm tốt, phù hợp mặc trong thời tiết se lạnh."
},

new SanPham
{
    MaSP = "PK085",
    TenSP = "Balo Da Teelab Local Brand Essentials Leather Backpack AC085",
    SoTien = 340000,
    SoLuong = 3,
    TinhTrang = "Còn hàng",
    HinhAnh = "balo.jpg",
    MoTa = "Balo da thiết kế tối giản, sang trọng. Chất liệu da bền đẹp, nhiều ngăn tiện dụng, phù hợp đi học, đi làm hoặc du lịch."
},

new SanPham
{
    MaSP = "PK057",
    TenSP = "Tất Teelab Iconic Logo Socks AC057",
    SoTien = 25000,
    SoLuong = 25,
    TinhTrang = "Còn hàng",
    HinhAnh = "tat.jpg",
    MauSac = "Đen, Trắng, Kem, Vàng, Xanh lá",
    MoTa = "Tất cổ cao với thiết kế logo Teelab nổi bật. Chất liệu cotton co giãn tốt, thoáng khí, mang lại cảm giác dễ chịu khi sử dụng."
},

new SanPham
{
    MaSP = "PK112",
    TenSP = "Nón Pillbox Local Brand Unisex Teelab Alter AC112",
    SoTien = 85000,
    SoLuong = 26,
    TinhTrang = "Còn hàng",
    HinhAnh = "nonda.jpg",
    MauSac = "Tweed Caro, Sọc, Đen, Da beo",
    MoTa = "Nón pillbox phong cách vintage, tạo điểm nhấn cho outfit. Thiết kế độc đáo, dễ phối với nhiều phong cách thời trang."
},

new SanPham
{
    MaSP = "AT052",
    TenSP = "Áo Sơ Mi Ngắn Tay Teelab Local Brand Unisex Studio Oxford Shirt SS052",
    SoTien = 250000,
    SoLuong = 6,
    TinhTrang = "Còn hàng",
    HinhAnh = "SS052.jpg",
    KichThuoc = "S, M, L, XL",
    MauSac = "Hồng, Xanh, Xám",
    MoTa = "Áo sơ mi ngắn tay chất Oxford cao cấp, form rộng thoải mái. Thiết kế đơn giản, phù hợp mặc đi học, đi làm hoặc đi chơi."
},

new SanPham
{
    MaSP = "AT066",
    TenSP = "Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Oxford shirts SS066",
    SoTien = 280000,
    SoLuong = 4,
    TinhTrang = "Còn hàng",
    HinhAnh = "somidai.jpg",
    KichThuoc = "S, M, L, XL",
    MauSac = "Đen, Trắng, Xanh than, Xanh dương",
    MoTa = "Áo sơ mi dài tay với chất liệu Oxford dày dặn, đứng form. Phù hợp phong cách lịch sự nhưng vẫn trẻ trung."
},

new SanPham
{
    MaSP = "AT068",
    TenSP = "Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Eco Oxford Logo Signature Shirt SS068",
    SoTien = 250000,
    SoLuong = 12,
    TinhTrang = "Còn hàng",
    HinhAnh = "SS068.jpg",
    KichThuoc = "S, M, L, XL",
    MauSac = "Đen, Trắng, Xanh than, Xanh dương, Hồng",
    MoTa = "Áo sơ mi cộc tay với thiết kế logo signature tinh tế. Chất liệu Oxford thân thiện, thoáng mát, phù hợp mặc hằng ngày."
},

new SanPham
{
    MaSP = "AT074",
    TenSP = "Áo Polo Local Brand Unisex Teelab KNIT POLO SHIRT AP074",
    SoTien = 320000,
    SoLuong = 0,
    TinhTrang = "Hết hàng",
    HinhAnh = "xamtieu.jpg",
    KichThuoc = "M, L, XL",
    MauSac = "Đen,Xanh Navy, Melane",
    MoTa = "Áo polo dệt kim cao cấp với form vừa vặn. Thiết kế thanh lịch, phù hợp cho cả đi làm và đi chơi."
}
            );
        }
    }
}