using Microsoft.AspNetCore.Mvc;
using Teelab.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teelab.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCart()
        {
            var sessionData = HttpContext.Session.GetString("GioHang");
            return sessionData == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(sessionData);
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("GioHang", JsonSerializer.Serialize(cart));
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }

        [HttpPost]
        // --- CẬP NHẬT: Nhận thêm biến size và color từ Form gửi lên ---
        public IActionResult AddToCart(string id, int quantity = 1, string size = null, string color = null)
        {
            var sp = _context.SanPhams.Find(id);
            if (sp == null || sp.SoLuong < quantity)
            {
                TempData["Error"] = "Số lượng trong kho không đủ!";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            var cart = GetCart();
            // TÌM KIẾM: Phải trùng cả Mã SP, trùng Size và trùng Màu thì mới cộng dồn số lượng
            var item = cart.FirstOrDefault(c => c.MaSP == id && c.KichThuoc == size && c.MauSac == color);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    Gia = sp.SoTien,
                    SoLuong = quantity,
                    KichThuoc = size, // Lưu size vào giỏ
                    MauSac = color    // Lưu màu vào giỏ
                });
            }
            else
            {
                item.SoLuong += quantity;
            }

            SaveCart(cart);
            TempData["Success"] = $"Đã thêm {quantity} {sp.TenSP} vào giỏ!";
            return RedirectToAction("Index");
        }

        // --- CẬP NHẬT: Xóa dựa trên CartItemId để không xóa nhầm áo khác size ---
        public IActionResult Remove(string cartItemId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.CartItemId == cartItemId);
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

            var userIdClaim = User.FindFirstValue("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");
            int userId = int.Parse(userIdClaim);

            var hoaDon = new ThanhToan
            {
                MaTT = "HD" + DateTime.Now.Ticks.ToString().Substring(10),
                NgayTao = DateTime.Now,
                Id = userId,
                TrangThai = "Chờ xác nhận", // Mặc định khi mới đặt
                TongTien = cart.Sum(c => c.ThanhTien) // Tính tổng tiền đơn hàng
            };
            _context.ThanhToans.Add(hoaDon);

            foreach (var item in cart)
            {
                var sp = await _context.SanPhams.FindAsync(item.MaSP);
                if (sp != null)
                {
                    if (sp.SoLuong < item.SoLuong)
                    {
                        TempData["Error"] = $"Sản phẩm {sp.TenSP} chỉ còn {sp.SoLuong} cái, không đủ để thanh toán!";
                        return RedirectToAction("Index");
                    }

                    sp.SoLuong -= item.SoLuong;
                    if (sp.SoLuong <= 0)
                    {
                        sp.SoLuong = 0;
                        sp.TinhTrang = "Hết hàng";
                    }

                    var chiTiet = new ChiTietThanhToan
                    {
                        MaTT = hoaDon.MaTT,
                        MaSP = item.MaSP,
                        SoLuong = item.SoLuong,
                        KichThuoc = item.KichThuoc, // NHẶT SIZE TỪ GIỎ HÀNG
                        MauSac = item.MauSac        // NHẶT MÀU TỪ GIỎ HÀNG
                    };
                    _context.ChiTietThanhToans.Add(chiTiet);
                }
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("GioHang");

            TempData["CheckoutSuccess"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "KhachHangs");
        }

        [HttpPost]
        // --- CẬP NHẬT: Cập nhật số lượng dựa trên CartItemId ---
        public IActionResult UpdateQuantity(string cartItemId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.CartItemId == cartItemId);

            if (item != null)
            {
                var sp = _context.SanPhams.Find(item.MaSP); // Tìm SP gốc trong DB để check tồn kho
                if (sp != null)
                {
                    if (quantity <= sp.SoLuong)
                        item.SoLuong = quantity;
                    else
                        TempData["Error"] = "Vượt quá số lượng trong kho!";
                }
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }
    }
}