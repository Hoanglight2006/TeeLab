using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Teelab.Models;
using TeeLab.Helper;
using TeeLab.Models;
using TeeLab.Services;

namespace Teelab.Controllers
{
    // --- KHÓA CHẶT: CHỈ KHÁCH HÀNG MỚI ĐƯỢC VÀO GIỎ HÀNG ---
    [Authorize(Roles = "KhachHang")]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IVnPayService _vnPayService;
        public CartController(AppDbContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService; // Khởi tạo service
        }

        private List<CartItem> GetCart()
        {
            var sessionData = HttpContext.Session.GetString("GioHang");
            return sessionData == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(sessionData)!;
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("GioHang", JsonSerializer.Serialize(cart));
        }

        public async Task<IActionResult> Index() // Thêm async Task để dùng await
        {
            var cart = GetCart();

            // Lấy UserId từ Claims của người dùng đang đăng nhập
            var userIdClaim = User.FindFirstValue("UserId");
            if (userIdClaim != null)
            {
                int userId = int.Parse(userIdClaim);
                // Tìm thông tin khách hàng trong Database
                var khachHang = await _context.KhachHangs.FindAsync(userId);
                // Gửi thông tin sang View qua ViewBag
                ViewBag.UserDefault = khachHang;
            }

            return View(cart);
        }
        // Trong CartController.cs

        [HttpGet]
        public IActionResult AddToCart()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddToCart(string id, int quantity = 1, string? size = null, string? color = null)
        {
            var sp = _context.SanPhams.Find(id);
            if (sp == null || sp.SoLuong < quantity)
            {
                TempData["Error"] = "Số lượng trong kho không đủ!";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSP == id && c.KichThuoc == size && c.MauSac == color);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    MaSP = sp.MaSP!,
                    TenSP = sp.TenSP!,
                    Gia = sp.SoTien,
                    SoLuong = quantity,
                    KichThuoc = size,
                    MauSac = color
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

        public IActionResult Remove(string cartItemId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.CartItemId == cartItemId);
            if (item != null) cart.Remove(item);
            SaveCart(cart);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            // Lấy UserId của người đang đăng nhập
            var userIdClaim = User.FindFirstValue("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");
            int userId = int.Parse(userIdClaim);

            // Tìm thông tin khách hàng trong Database
            var khachHang = await _context.KhachHangs.FindAsync(userId);

            // Gửi thông tin khách hàng sang View qua ViewBag để tự động điền form
            ViewBag.UserDefault = khachHang;

            return View(cart);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(string payment, string HoTen, string DienThoai, string DiaChi, string GhiChu)
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
                TrangThai = "Chờ xác nhận",
                TongTien = cart.Sum(c => c.ThanhTien),
                PhuongThucTT = (payment != null && payment.Contains("VNPay")) ? "VNPay" : "COD",
                HoTen = HoTen,
                DienThoai = DienThoai,
                DiaChi = DiaChi,
                GhiChu = GhiChu
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
                        KichThuoc = item.KichThuoc,
                        MauSac = item.MauSac
                    };
                    _context.ChiTietThanhToans.Add(chiTiet);
                }
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("GioHang");
            if (payment == "Thanh toán VNPay")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = (double)hoaDon.TongTien,
                    CreatedDate = DateTime.Now,
                    Description = $"Thanh toán đơn hàng {hoaDon.MaTT}",
                    FullName = User.Identity?.Name ?? "Khách hàng",
                    OrderId = hoaDon.MaTT // Dùng MaTT (HD...)
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }

            TempData["CheckoutSuccess"] = "Đặt hàng thành công!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            // Kiểm tra nếu response null hoặc thất bại
            if (response == null || !response.Success)
            {
                // Kiểm tra xem có OrderId (MaTT) trả về không mới xử lý cộng kho
                if (!string.IsNullOrEmpty(response?.OrderId))
                {
                    var dsChiTiet = _context.ChiTietThanhToans.Where(x => x.MaTT == response.OrderId).ToList();

                    foreach (var ct in dsChiTiet)
                    {
                        var sp = await _context.SanPhams.FindAsync(ct.MaSP);
                        if (sp != null)
                        {
                            sp.SoLuong += ct.SoLuong;

                            // Nếu trước đó sp hết hàng, giờ có lại thì cập nhật trạng thái
                            if (sp.SoLuong > 0 && sp.TinhTrang == "Hết hàng")
                            {
                                sp.TinhTrang = "Còn hàng";
                            }
                        }
                    }

                    // Cập nhật trạng thái hóa đơn thành 'Thất bại' hoặc 'Đã hủy' thay vì xóa
                    var hoaDonThatBai = _context.ThanhToans.FirstOrDefault(h => h.MaTT == response.OrderId);
                    if (hoaDonThatBai != null)
                    {
                        hoaDonThatBai.TrangThai = "Thanh toán thất bại";
                    }

                    await _context.SaveChangesAsync();
                }

                TempData["Error"] = $"Thanh toán thất bại. Mã lỗi: {response?.VnPayResponseCode}";
                return RedirectToAction("Index");
            }

            // --- TRƯỜNG HỢP THÀNH CÔNG ---
            var hoaDon = _context.ThanhToans.FirstOrDefault(h => h.MaTT == response.OrderId);
            if (hoaDon != null)
            {
                // Đã đổi từ "Đã thanh toán" sang "Chờ xác nhận" theo đúng ý bạn
                hoaDon.TrangThai = "Chờ xác nhận";
                hoaDon.PhuongThucTT = "VNPay (Đã thanh toán)";
                await _context.SaveChangesAsync();
            }

            TempData["CheckoutSuccess"] = "Thanh toán VNPay thành công! Đơn hàng đang chờ xác nhận.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateQuantity(string cartItemId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.CartItemId == cartItemId);

            if (item != null)
            {
                var sp = _context.SanPhams.Find(item.MaSP);
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