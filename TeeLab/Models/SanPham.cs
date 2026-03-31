using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teelab.Models
{
    public class SanPham
    {
        [Key]
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng kho")]
        public int SoLuong { get; set; }
        public string? TinhTrang { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SoTien { get; set; }
        public string? HinhAnh { get; set; }

        // --- CÁC CỘT MỚI BỔ SUNG THEO YÊU CẦU ĐẶC TẢ ---
        public string? MoTa { get; set; }
        public string? KichThuoc { get; set; } // Ví dụ lưu chuỗi: "S, M, L, XL"
        public string? MauSac { get; set; }    // Ví dụ lưu chuỗi: "Đen, Trắng"

        // Navigation property: 1 Sản phẩm có thể nằm trong nhiều chi tiết thanh toán
        [ValidateNever] // Tránh vòng lặp khi serialize
        public ICollection<ChiTietThanhToan> ChiTietThanhToans { get; set; }
    }
}