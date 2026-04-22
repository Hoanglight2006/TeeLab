using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeeLab.Models;

namespace Teelab.Models
{
    public class ThanhToan
    {
        [Key]
        public string? MaTT { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public string? TrangThai { get; set; } = "Chờ xác nhận";
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TongTien { get; set; }
        // -----------------------------------------------

        public int Id { get; set; }
        [ForeignKey("Id")]
        public KhachHang? KhachHang { get; set; }

        public string? PhuongThucTT { get; set; }
        public string? HoTen { get; set; }
        public string? DienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? GhiChu { get; set; }
        public ICollection<ChiTietThanhToan>? ChiTietThanhToans { get; set; }
    }
}