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
            modelBuilder.Entity<QuanLy>().HasData(new QuanLy { Id = 1, Hoten = "ADMIN", TenDangNhap = "admin", MatKhau = "$2a$11$RmtZhPdmyjWci4YLDNfdBONzf.ML4a011L3hMlelCviuk2m.VoHgW", Sdt = "0123456789", Diachi = "Thái Nguyên", Email = "admin@teelab.vn", Avatar = "default-avatar.png" });
            modelBuilder.Entity<NhanVien>().HasData(new NhanVien { Id = 2, Hoten = "Staff", TenDangNhap = "Staff", MatKhau = "$2a$11$RmtZhPdmyjWci4YLDNfdBONzf.ML4a011L3hMlelCviuk2m.VoHgW", Sdt = "0988888888", Diachi = "Hà Nội", Email = "Staff@teelab.vn", Avatar = "default-avatar.png" });
            modelBuilder.Entity<KhachHang>().HasData(new KhachHang { Id = 3, Hoten = "Customer", TenDangNhap = "Customer", MatKhau = "$2a$11$RmtZhPdmyjWci4YLDNfdBONzf.ML4a011L3hMlelCviuk2m.VoHgW", Sdt = "0977777777", Diachi = "Hải Phòng", Email = "Customer@gmail.com", Avatar = "default-avatar.png" });
            
            // 2. SEED DATA: SẢN PHẨM
            modelBuilder.Entity<SanPham>().HasData(
     new SanPham { MaSP = "AT382", TenSP = "Áo Thun Teelab Alter Oversize Cotton 250GSM Trơn Unisex TS382", SoTien = 250000, SoLuong = 50, TinhTrang = "Còn hàng", HinhAnh = "aotrang.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng, Xám tiêu" },
     new SanPham { MaSP = "AT376", TenSP = "Áo Thun Teelab Alter Oversize Cotton In World Tour TOKYO Water Color Unisex TS376", SoTien = 280000, SoLuong = 3, TinhTrang = "Còn hàng", HinhAnh = "aoxam.jpg", KichThuoc = "S, M, L, XL", MauSac = "Trắng, Xám" },
     new SanPham { MaSP = "AT377", TenSP = "Áo Thun Sọc Teelab Alter Oversize Cotton Thêu Pop Star Unisex TS377", SoTien = 280000, SoLuong = 20, TinhTrang = "Còn hàng", HinhAnh = "aoxoc.jpg", KichThuoc = "S, M ,L, XL", MauSac = "Kẻ sọc nâu, Kẻ sọc xanh" },
     new SanPham { MaSP = "Q003", TenSP = "Quần Short Teelab Alter Oversize Nỉ Chân Cua In Wourld Tour Unisex SH003", SoTien = 220000, SoLuong = 100, TinhTrang = "Còn hàng", HinhAnh = "quandui.jpg", KichThuoc = "S, M ,L ,XL", MauSac = "Đen, Trắng" },
     new SanPham { MaSP = "AT188", TenSP = "Áo Thun Teelab Alter Oversize Cotton In Wave Line Unisex TS188", SoTien = 250000, SoLuong = 0, TinhTrang = "Hết hàng", HinhAnh = "soc.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen" },
     new SanPham { MaSP = "Q131", TenSP = "Quần Nỉ Ống Suông Teelab Alter Oversize Nỉ In World Tour PS131", SoTien = 250000, SoLuong = 50, TinhTrang = "Còn hàng", HinhAnh = "quanni.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Xanh Navy, Xám trắng" },
     new SanPham { MaSP = "Q116", TenSP = "Quần Dài Local Brand Unisex Teelab Jeans Ống Rộng PS116", SoTien = 250000, SoLuong = 20, TinhTrang = "Còn hàng", HinhAnh = "quanbo.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen Wash, Trắng Wash, Xanh Wash, Xám đen Wash" },
     new SanPham { MaSP = "AH121", TenSP = "Áo Teelab Local Brand Unisex Hoodie zip Stars HD121", SoTien = 399000, SoLuong = 6, TinhTrang = "Còn hàng", HinhAnh = "hutdi.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng, Xám, Nâu" },
     new SanPham { MaSP = "PK085", TenSP = "Balo Da Teelab Local Brand Essentials Leather Backpack AC085", SoTien = 340000, SoLuong = 3, TinhTrang = "Còn hàng", HinhAnh = "balo.jpg" },
     new SanPham { MaSP = "PK057", TenSP = "Tất Teelab Iconic Logo Socks AC057", SoTien = 25000, SoLuong = 25, TinhTrang = "Còn hàng", HinhAnh = "tat.jpg", MauSac = "Đen, Trắng, Kem, Vàng, Xanh lá" },
     new SanPham { MaSP = "PK112", TenSP = "Nón Pillbox Local Brand Unisex Teelab Alter AC112", SoTien = 85000, SoLuong = 26, TinhTrang = "Còn hàng", HinhAnh = "nonda.jpg", MauSac = "Tweed Caro, Sọc, Đen, Da beo" },
     new SanPham { MaSP = "AT052", TenSP = "Áo Sơ Mi Ngắn Tay Teelab Local Brand Unisex Studio Oxford Shirt SS052", SoTien = 250000, SoLuong = 6, TinhTrang = "Còn hàng", HinhAnh = "SS052.jpg", KichThuoc = "S, M, L, XL", MauSac = "Hồng, Xanh, Xám" },
     new SanPham { MaSP = "AT066", TenSP = "Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Oxford shirts SS066", SoTien = 280000, SoLuong = 4, TinhTrang = "Còn hàng", HinhAnh = "somidai.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng, Xanh than, Xanh dương" },
     new SanPham { MaSP = "AT068", TenSP = "Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Eco Oxford Logo Signature Shirt SS068", SoTien = 250000, SoLuong = 12, TinhTrang = "Còn hàng", HinhAnh = "SS068.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng, Xanh than, Xanh dương, Hồng" },
     new SanPham { MaSP = "AT074", TenSP = "Áo Polo Local Brand Unisex Teelab KNIT POLO SHIRT AP074", SoTien = 320000, SoLuong = 0, TinhTrang = "Hết hàng", HinhAnh = "xamtieu.jpg", KichThuoc = "M, L, XL", MauSac = "Đen,Xanh Navy, Melane" }
 );
        }
    }
}