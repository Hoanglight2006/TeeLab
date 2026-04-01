using System.ComponentModel.DataAnnotations;

namespace TeeLab.Models
{
    public abstract class Nguoi
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string? Hoten { get; set; }

        public string? Diachi { get; set; }

        public DateTime? Ngaysinh { get; set; }

        public string? Sdt { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string MatKhau { get; set; }

        // --- BỔ SUNG 2 CỘT MỚI ---
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        public string? Avatar { get; set; } // Chỉ lưu tên file ảnh (VD: hoang.jpg)
    }
}