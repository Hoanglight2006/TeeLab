using System.ComponentModel.DataAnnotations;

namespace Teelab.Models
{
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "SĐT phải từ 10-11 số")]
        public string SoDienThoai { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? NgaySinh { get; set; }

        public string? GioiTinh { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string DiaChi { get; set; }

        public string? Avatar { get; set; }
    }
}