using Microsoft.AspNetCore.Mvc;
using Teelab.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json; // Để chuyển đổi danh sách sang chuỗi lưu vào Session
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Teelab.Controllers
{
    [Authorize] // Chỉ cho phép người dùng đã đăng nhập truy cập vào CartController
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Lấy giỏ hàng từ Session ra
        private List<CartItem> GetCart()
        {
            var sessionData = HttpContext.Session.GetString("GioHang");
            return sessionData == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(sessionData);
        }

        // Lưu giỏ hàng ngược lại vào Session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("GioHang", JsonSerializer.Serialize(cart));
        }

        // 1. Trang xem giỏ hàng
        public IActionResult Index()
        {
            return View(GetCart());
        }

        [HttpPost] // Đổi sang HttpPost vì mình dùng Form
        public IActionResult AddToCart(string id, int quantity = 1)
        {
            var sp = _context.SanPhams.Find(id);
            if (sp == null || sp.SoLuong < quantity)
            {
                TempData["Error"] = "Số lượng trong kho không đủ!";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSP == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    Gia = sp.SoTien,
                    SoLuong = quantity // Lấy theo số lượng khách nhập
                });
            }
            else
            {
                item.SoLuong += quantity; // Cộng dồn thêm số lượng mới
            }

            SaveCart(cart);
            TempData["Success"] = $"Đã thêm {quantity} {sp.TenSP} vào giỏ!";
            return RedirectToAction("Index");
        }

        // 3. Xóa món đồ khỏi giỏ
        public IActionResult Remove(string id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSP == id);
            if (item != null) cart.Remove(item);
            SaveCart(cart);
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            // 1. Lấy Id của khách hàng đang đăng nhập
            var userIdClaim = User.FindFirstValue("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");
            int userId = int.Parse(userIdClaim);

            // 2. Tạo một hóa đơn mới (ThanhToan)
            var hoaDon = new ThanhToan
            {
                MaTT = "HD" + DateTime.Now.Ticks.ToString().Substring(10), // Tạo mã HD ngẫu nhiên
                NgayTao = DateTime.Now,
                Id = userId // Gắn ID người mua vào
            };
            _context.ThanhToans.Add(hoaDon);

            // 3. Duyệt qua từng món trong giỏ để lưu chi tiết và TRỪ KHO
            foreach (var item in cart)
            {
                // Tìm sản phẩm trong Database để cập nhật số lượng
                var sp = await _context.SanPhams.FindAsync(item.MaSP);
                if (sp != null)
                {
                    // Kiểm tra xem kho còn đủ hàng không (phòng trường hợp người khác mua hết trước)
                    if (sp.SoLuong < item.SoLuong)
                    {
                        TempData["Error"] = $"Sản phẩm {sp.TenSP} chỉ còn {sp.SoLuong} cái, không đủ để thanh toán!";
                        return RedirectToAction("Index");
                    }

                    // --- ĐOẠN QUAN TRỌNG: TRỪ SỐ LƯỢNG KHO ---
                    sp.SoLuong -= item.SoLuong;

                    // Nếu trừ xong mà hết sạch hàng thì cập nhật trạng thái luôn
                    if (sp.SoLuong <= 0)
                    {
                        sp.SoLuong = 0;
                        sp.TinhTrang = "Hết hàng";
                    }

                    // Tạo bản ghi Chi tiết hóa đơn
                    var chiTiet = new ChiTietThanhToan
                    {
                        MaTT = hoaDon.MaTT,
                        MaSP = item.MaSP,
                        SoLuong = item.SoLuong
                    };
                    _context.ChiTietThanhToans.Add(chiTiet);
                }
            }

            // 4. Lưu tất cả thay đổi xuống Database (Transaction)
            await _context.SaveChangesAsync();

            // 5. Xóa sạch giỏ hàng trong Session sau khi mua xong
            HttpContext.Session.Remove("GioHang");

            TempData["Success"] = "Đặt hàng thành công! Đơn hàng của bạn đang được xử lý.";
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult UpdateQuantity(string id, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSP == id);
            var sp = _context.SanPhams.Find(id);

            if (item != null && sp != null)
            {
                if (quantity <= sp.SoLuong) // Kiểm tra kho trước khi cập nhật
                    item.SoLuong = quantity;
                else
                    TempData["Error"] = "Vượt quá số lượng trong kho!";
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }
    }
}