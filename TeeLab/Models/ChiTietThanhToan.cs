using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teelab.Models
{
    public class ChiTietThanhToan
    {
        [Key] // Thêm dòng này để làm khóa chính độc lập
        public int Id { get; set; }
        // Khóa ngoại trỏ về ThanhToan
        public string MaTT { get; set; }
        [ForeignKey("MaTT")]
        public ThanhToan ThanhToan { get; set; }

        // Khóa ngoại trỏ về SanPham
        public string MaSP { get; set; }
        [ForeignKey("MaSP")]
        public SanPham SanPham { get; set; }

        public int SoLuong { get; set; }

        // --- BỔ SUNG THÊM 2 CỘT LƯU SIZE VÀ MÀU ---
        public string? KichThuoc { get; set; }
        public string? MauSac { get; set; }
        public double Gia { get; set; }
    }
}