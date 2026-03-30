using System.ComponentModel.DataAnnotations;

namespace TeeLab.Models
{
    // Dùng abstract class để không thể tạo đối tượng trực tiếp từ lớp Nguoi
    public abstract class Nguoi
    {
        [Key]
        public int Id { get; set; } // Dùng Id chung thay cho MaKH, MaNV, MaQL

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string? Hoten { get; set; }

        public string? Diachi { get; set; } // Dấu ? nghĩa là có thể để trống (null)

        public DateTime? Ngaysinh { get; set; }

        public string? Sdt { get; set; } // Đưa Số điện thoại lên đây dùng chung luôn

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string MatKhau { get; set; }

    }
}
