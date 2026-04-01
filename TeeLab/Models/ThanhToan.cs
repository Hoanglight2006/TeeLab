using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teelab.Models;

namespace Teelab.Models
{
    public class ThanhToan
    {
        [Key]
        public string MaTT { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Kết nối với Khách Hàng (1 Khách hàng có nhiều hóa đơn Thanh toán)
        public int Id { get; set; } // Khóa ngoại trỏ về bảng Nguoi
        [ForeignKey("Id")]
        public KhachHang KhachHang { get; set; }

        // Navigation property: 1 Thanh toán có nhiều chi tiết thanh toán
        public ICollection<ChiTietThanhToan> ChiTietThanhToans { get; set; }
    }
}