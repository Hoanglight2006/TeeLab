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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // 1. SEED DATA: TÀI KHOẢN
            modelBuilder.Entity<QuanLy>().HasData(new QuanLy { Id = 1, Hoten = "Dương Đình Hoàng", TenDangNhap = "admin", MatKhau = "123", Sdt = "0123456789", Diachi = "Thái Nguyên", Email = "admin@teelab.vn", Avatar = "default-avatar.png" });
            modelBuilder.Entity<NhanVien>().HasData(new NhanVien { Id = 2, Hoten = "Nguyễn Nhân Viên", TenDangNhap = "nhanvien", MatKhau = "123", Sdt = "0988888888", Diachi = "Hà Nội", Email = "nhanvien@teelab.vn", Avatar = "default-avatar.png" });
            modelBuilder.Entity<KhachHang>().HasData(new KhachHang { Id = 3, Hoten = "Lê Minh Hoàng", TenDangNhap = "hoang", MatKhau = "123", Sdt = "0977777777", Diachi = "Hải Phòng", Email = "hoang@gmail.com", Avatar = "default-avatar.png" });

            // 2. SEED DATA: SẢN PHẨM
            modelBuilder.Entity<SanPham>().HasData(
     new SanPham { MaSP = "TS382", TenSP = "Áo Thun Teelab Alter Oversize Cotton 250GSM Trơn Unisex TS382", SoTien = 250000, SoLuong = 50, TinhTrang = "Còn hàng", HinhAnh = "aotrang.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng, Xám tiêu" },
     new SanPham { MaSP = "TS376", TenSP = "Áo Thun Teelab Alter Oversize Cotton In World Tour TOKYO Water Color Unisex TS376", SoTien = 280000, SoLuong = 3, TinhTrang = "Còn hàng", HinhAnh = "aoxam.jpg", KichThuoc = "S, M, L, XL", MauSac = "Trắng, Xám" },
     new SanPham { MaSP = "TS377", TenSP = "Áo Thun Sọc Teelab Alter Oversize Cotton Thêu Pop Star Unisex TS377", SoTien = 280000, SoLuong = 20, TinhTrang = "Còn hàng", HinhAnh = "aoxoc.jpg", KichThuoc = "S, M ,L, XL", MauSac = "Kẻ sọc nâu, Kẻ sọc xanh" },
     new SanPham { MaSP = "TS379", TenSP = "Áo Thun Teelab Alter Oversize Cotton In Essentials Unisex TS379", SoTien = 250000, SoLuong = 100, TinhTrang = "Còn hàng", HinhAnh = "dodo.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng, Xanh Navy, Đỏ đô, Xám tiêu" },
     new SanPham { MaSP = "SH003", TenSP = "Quần Short Teelab Alter Oversize Nỉ Chân Cua In Wourld Tour Unisex SH003", SoTien = 220000, SoLuong = 100, TinhTrang = "Còn hàng", HinhAnh = "quandui.jpg", KichThuoc = "S, M ,L ,XL", MauSac = "Đen, Trắng" },
     new SanPham { MaSP = "TS188", TenSP = "Áo Thun Teelab Alter Oversize Cotton In Wave Line Unisex TS188", SoTien = 250000, SoLuong = 0, TinhTrang = "Hết hàng", HinhAnh = "soc.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen" },
     new SanPham { MaSP = "PS131", TenSP = "Quần Nỉ Ống Suông Teelab Alter Oversize Nỉ In World Tour PS131", SoTien = 250000, SoLuong = 50, TinhTrang = "Còn hàng", HinhAnh = "quanni.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Xanh Navy, Xám trắng" },
     new SanPham { MaSP = "PS116", TenSP = "Quần Dài Local Brand Unisex Teelab Jeans Ống Rộng PS116", SoTien = 250000, SoLuong = 20, TinhTrang = "Còn hàng", HinhAnh = "quanbo.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen Wash, Trắng Wash, Xanh Wash, Xám đen Wash" },
     new SanPham { MaSP = "HD121", TenSP = "Áo Teelab Local Brand Unisex Hoodie zip Stars HD121", SoTien = 399000, SoLuong = 6, TinhTrang = "Còn hàng", HinhAnh = "hutdi.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng, Xám, Nâu" },
     new SanPham { MaSP = "AC085", TenSP = "Balo Da Teelab Local Brand Essentials Leather Backpack AC085", SoTien = 340000, SoLuong = 3, TinhTrang = "Còn hàng", HinhAnh = "balo.jpg" },
     new SanPham { MaSP = "AC057", TenSP = "Tất Teelab Iconic Logo Socks AC057", SoTien = 25000, SoLuong = 25, TinhTrang = "Còn hàng", HinhAnh = "tat.jpg", MauSac = "Đen, Trắng, Kem, Vàng, Xanh lá" },
     new SanPham { MaSP = "AC112", TenSP = "Nón Pillbox Local Brand Unisex Teelab Alter AC112", SoTien = 85000, SoLuong = 26, TinhTrang = "Còn hàng", HinhAnh = "nonda.jpg", MauSac = "Tweed Caro, Sọc, Đen, Da beo" },
     new SanPham { MaSP = "SS052", TenSP = "Áo Sơ Mi Ngắn Tay Teelab Local Brand Unisex Studio Oxford Shirt SS052", SoTien = 250000, SoLuong = 6, TinhTrang = "Còn hàng", HinhAnh = "SS052.jpg", KichThuoc = "S, M, L, XL", MauSac = "Hồng, Xanh, Xám" },
     new SanPham { MaSP = "SS066", TenSP = "Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Oxford shirts SS066", SoTien = 280000, SoLuong = 4, TinhTrang = "Còn hàng", HinhAnh = "somidai.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng, Xanh than, Xanh dương" },
     new SanPham { MaSP = "SS068", TenSP = "Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Eco Oxford Logo Signature Shirt SS068", SoTien = 250000, SoLuong = 12, TinhTrang = "Còn hàng", HinhAnh = "SS068.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng, Xanh than, Xanh dương, Hồng" },
     new SanPham { MaSP = "AP074", TenSP = "Áo Polo Local Brand Unisex Teelab KNIT POLO SHIRT AP074", SoTien = 320000, SoLuong = 0, TinhTrang = "Hết hàng", HinhAnh = "xamtieu.jpg", KichThuoc = "M, L, XL", MauSac = "Đen,Xanh Navy, Melane" }
 );

            // 3. SEED DATA: HÓA ĐƠN (Dùng ngày cố định)
            string m1 = "HD_SAMPLE_01";
            string m2 = "HD_SAMPLE_02";

            modelBuilder.Entity<ThanhToan>().HasData(
                new ThanhToan { MaTT = m1, Id = 3, NgayTao = new DateTime(2026, 3, 30), TrangThai = "Giao hàng thành công", TongTien = 500000 },
                new ThanhToan { MaTT = m2, Id = 3, NgayTao = new DateTime(2026, 3, 31), TrangThai = "Chờ xác nhận", TongTien = 450000 }
            );

            modelBuilder.Entity<ChiTietThanhToan>().HasData(
                new ChiTietThanhToan { Id=1, MaTT = m1, MaSP = "AT001", SoLuong = 2, KichThuoc = "M", MauSac = "Đen" },
                new ChiTietThanhToan { Id=2, MaTT = m2, MaSP = "HD001", SoLuong = 1, KichThuoc = "XL", MauSac = "Xám" }
            );
        }
    }
}