using Microsoft.EntityFrameworkCore;
using TeeLab.Models;

namespace Teelab.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Khai báo các DbSet (tương đương với các bảng trong SQL)
        public DbSet<Nguoi> Nguois { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<QuanLy> QuanLys { get; set; }

        // Commit lần 2
        // Thêm 3 bảng mới
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<ChiTietThanhToan> ChiTietThanhToans { get; set; }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình khóa chính kép cho ChiTietThanhToan (giữ nguyên cái cũ của bạn)
            modelBuilder.Entity<ChiTietThanhToan>().HasKey(c => new { c.MaTT, c.MaSP });

            // TỰ ĐỘNG BƠM DỮ LIỆU MẪU (SEED DATA)
            modelBuilder.Entity<QuanLy>().HasData(new QuanLy
            {
                Id = 1, // Vì bạn dùng string làm Id nên để "1"
                Hoten = "Admin",
                TenDangNhap = "admin",
                MatKhau = "123456",
                Sdt = "0123456789"
            });

            modelBuilder.Entity<SanPham>().HasData(
                new SanPham { MaSP = "AT001", TenSP = "Áo thun Teelab Basic", SoTien = 250000, SoLuong = 50, TinhTrang = "Còn hàng" },
                new SanPham { MaSP = "AT002", TenSP = "Áo Hoodie Teelab", SoTien = 450000, SoLuong = 10, TinhTrang = "Còn hàng" },
                new SanPham { MaSP = "Hoc", TenSP = "Học", SoTien = 36000, SoLuong = 0, TinhTrang = "Hết hàng" }
            );
        }
    }
}