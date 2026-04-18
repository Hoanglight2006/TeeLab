namespace Teelab.Models
{
    public class CartItem
    {
        // Thêm một mã ID duy nhất cho mỗi dòng trong giỏ 
        // (Để phân biệt cùng 1 áo nhưng khác size/màu)
        public string CartItemId { get; set; } = Guid.NewGuid().ToString();

        public string? MaSP { get; set; }
        public string? TenSP { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }

        // --- THÊM 2 THUỘC TÍNH NÀY ---
        public string? KichThuoc { get; set; }
        public string? MauSac { get; set; }

        public decimal ThanhTien => Gia * SoLuong;
    }
}