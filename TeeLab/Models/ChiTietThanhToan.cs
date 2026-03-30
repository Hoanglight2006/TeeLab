using System.ComponentModel.DataAnnotations.Schema;

namespace Teelab.Models
{
    public class ChiTietThanhToan
    {
        // Khóa ngoại trỏ về ThanhToan
        public string MaTT { get; set; }
        [ForeignKey("MaTT")]
        public ThanhToan ThanhToan { get; set; }

        // Khóa ngoại trỏ về SanPham
        public string MaSP { get; set; }
        [ForeignKey("MaSP")]
        public SanPham SanPham { get; set; }

        public int SoLuong { get; set; }
    }
}