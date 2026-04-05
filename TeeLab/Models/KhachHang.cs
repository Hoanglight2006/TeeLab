namespace TeeLab.Models
{
    public class KhachHang : Nguoi // Kế thừa từ lớp Nguoi
    {
        public string? HangThanhVien { get; set; }
        public bool IsLocked { get; set; } = false;
    }
}
