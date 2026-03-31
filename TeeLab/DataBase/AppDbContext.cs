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

            modelBuilder.Entity<ChiTietThanhToan>().HasKey(c => new { c.MaTT, c.MaSP });

            // 1. SEED DATA: TÀI KHOẢN
            modelBuilder.Entity<QuanLy>().HasData(new QuanLy { Id = 1, Hoten = "Dương Đình Hoàng", TenDangNhap = "admin", MatKhau = "123", Sdt = "0123456789", Diachi = "Thái Nguyên" });
            modelBuilder.Entity<NhanVien>().HasData(new NhanVien { Id = 2, Hoten = "Nguyễn Nhân Viên", TenDangNhap = "nhanvien", MatKhau = "123", Sdt = "0988888888", Diachi = "Hà Nội" });
            modelBuilder.Entity<KhachHang>().HasData(new KhachHang { Id = 3, Hoten = "Lê Minh Hoàng", TenDangNhap = "hoang", MatKhau = "123", Sdt = "0977777777", Diachi = "Hải Phòng" });

            // 2. SEED DATA: SẢN PHẨM
            modelBuilder.Entity<SanPham>().HasData(
                new SanPham { MaSP = "AT001", TenSP = "Áo thun Teelab Basic", SoTien = 250000, SoLuong = 50, TinhTrang = "Còn hàng", HinhAnh = "at001.jpg", KichThuoc = "S, M, L, XL", MauSac = "Đen, Trắng" },
                new SanPham { MaSP = "AT002", TenSP = "Áo thun Rabbit Edition", SoTien = 290000, SoLuong = 3, TinhTrang = "Còn hàng", HinhAnh = "at002.jpg", KichThuoc = "M, L", MauSac = "Trắng" },
                new SanPham { MaSP = "HD001", TenSP = "Hoodie Teelab Signature", SoTien = 450000, SoLuong = 20, TinhTrang = "Còn hàng", HinhAnh = "hd001.jpg", KichThuoc = "L, XL", MauSac = "Xám" },
                new SanPham { MaSP = "PK001", TenSP = "Mũ Cap Teelab", SoTien = 150000, SoLuong = 100, TinhTrang = "Còn hàng", HinhAnh = "pk001.jpg", KichThuoc = "Free size", MauSac = "Đen" }
            );

            // 3. SEED DATA: HÓA ĐƠN (Dùng ngày cố định)
            string m1 = "HD_SAMPLE_01";
            string m2 = "HD_SAMPLE_02";

            modelBuilder.Entity<ThanhToan>().HasData(
                new ThanhToan { MaTT = m1, Id = 3, NgayTao = new DateTime(2026, 3, 30), TrangThai = "Giao hàng thành công", TongTien = 500000 },
                new ThanhToan { MaTT = m2, Id = 3, NgayTao = new DateTime(2026, 3, 31), TrangThai = "Chờ xác nhận", TongTien = 450000 }
            );

            modelBuilder.Entity<ChiTietThanhToan>().HasData(
                new ChiTietThanhToan { MaTT = m1, MaSP = "AT001", SoLuong = 2, KichThuoc = "M", MauSac = "Đen" },
                new ChiTietThanhToan { MaTT = m2, MaSP = "HD001", SoLuong = 1, KichThuoc = "XL", MauSac = "Xám" }
            );
        }
    }
}