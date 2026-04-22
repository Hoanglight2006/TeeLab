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
        [RegularExpression(@"^(0[3|5|7|8|9])[0-9]{8}$", ErrorMessage = "Số điện thoại không đúng định dạng Việt Nam (ví dụ: 0987654321)")]
        public string? Sdt { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string? TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ hoa, 1 chữ thường và 1 chữ số")]
        public string MatKhau { get; set; }

        // --- BỔ SUNG 2 CỘT MỚI ---
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        public string? Avatar { get; set; } // Chỉ lưu tên file ảnh (VD: hoang.jpg)
    }
}