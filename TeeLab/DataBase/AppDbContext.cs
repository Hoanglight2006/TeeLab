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
    }
}