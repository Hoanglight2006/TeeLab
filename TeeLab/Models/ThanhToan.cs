using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeeLab.Models;

namespace Teelab.Models
{
    public class ThanhToan
    {
        [Key]
        public string MaTT { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // --- BỔ SUNG 2 CỘT NÀY CHO NGHIỆP VỤ ĐƠN HÀNG ---
        public string TrangThai { get; set; } = "Chờ xác nhận";

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TongTien { get; set; }
        // -----------------------------------------------

        // Kết nối với Khách Hàng (1 Khách hàng có nhiều hóa đơn Thanh toán)
        public int Id { get; set; } // Khóa ngoại trỏ về bảng Nguoi
        [ForeignKey("Id")]
        public KhachHang KhachHang { get; set; }

        // Navigation property: 1 Thanh toán có nhiều chi tiết thanh toán
        public ICollection<ChiTietThanhToan> ChiTietThanhToans { get; set; }
    }
}