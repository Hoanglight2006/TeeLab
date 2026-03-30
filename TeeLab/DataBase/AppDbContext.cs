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

        // Cấu hình Khóa chính kép cho lớp trung gian
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChiTietThanhToan>()
                .HasKey(c => new { c.MaTT, c.MaSP }); // Dùng cả 2 mã làm khóa chính
        }
    }
}