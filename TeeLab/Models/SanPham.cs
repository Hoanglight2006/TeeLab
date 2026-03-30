using System.ComponentModel.DataAnnotations;

namespace Teelab.Models
{
    public class SanPham
    {
        [Key]
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string TinhTrang { get; set; }
        public decimal SoTien { get; set; }

        // Navigation property: 1 Sản phẩm có thể nằm trong nhiều chi tiết thanh toán
        public ICollection<ChiTietThanhToan> ChiTietThanhToans { get; set; }
    }
}