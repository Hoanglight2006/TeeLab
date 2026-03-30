using Microsoft.AspNetCore.Mvc;
using Teelab.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json; // Để chuyển đổi danh sách sang chuỗi lưu vào Session
using Microsoft.AspNetCore.Authorization;

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

        // 2. Thêm vào giỏ
        public IActionResult AddToCart(string id)
        {
            var sp = _context.SanPhams.Find(id);
            // Kiểm tra: Nếu không thấy SP hoặc Tình trạng là Hết hàng thì không cho mua
            if (sp == null || sp.TinhTrang == "Hết hàng")
            {
                TempData["Error"] = "Sản phẩm này hiện đang hết hàng!";
                return RedirectToAction("Index", "Home");
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSP == id);

            if (item == null)
            {
                cart.Add(new CartItem { MaSP = sp.MaSP, TenSP = sp.TenSP, Gia = sp.SoTien, SoLuong = 1 });
            }
            else
            {
                item.SoLuong++;
            }

            SaveCart(cart);
            TempData["Success"] = $"Đã thêm {sp.TenSP} vào giỏ hàng!";
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
    }
}