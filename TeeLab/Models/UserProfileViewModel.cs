using System.ComponentModel.DataAnnotations;

namespace Teelab.Models
{
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        // Bỏ EmailAddress ở đây vì ta sẽ xử lý che dấu * ở Controller
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        // Bỏ RegularExpression ở đây, ta sẽ check thủ công trong Controller 
        // nếu chuỗi gửi lên KHÔNG chứa dấu *
        public string SoDienThoai { get; set; }

        public string? DiaChi { get; set; }
        public string? Avatar { get; set; }

        // --- BỔ SUNG CÁC TRƯỜNG ĐỂ ĐỔI MẬT KHẨU ---
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu mới phải từ 6 ký tự")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không khớp")]
        public string? ConfirmPassword { get; set; }
    }
}