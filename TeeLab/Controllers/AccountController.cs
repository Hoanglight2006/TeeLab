using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Teelab.Models;
using TeeLab.Models;

namespace Teelab.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Hiển thị Form đăng nhập
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Lưu lại địa chỉ trang khách định vào
            return View();
        }

        // 2. Xử lý khi bấm nút Đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Nguois.FirstOrDefault(u => u.TenDangNhap == model.TenDangNhap && u.MatKhau == model.MatKhau);

                if (user != null)
                {
                    string role = "KhachHang";
                    if (user is QuanLy) role = "QuanLy";
                    else if (user is NhanVien) role = "NhanVien";

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Hoten),
                new Claim(ClaimTypes.Role, role),
                new Claim("UserId", user.Id.ToString())
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 1. Thực hiện Đăng nhập
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // 2. Xử lý chuyển hướng (Redirect)
                    // Ưu tiên quay lại trang cũ nếu có returnUrl
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // Nếu không có returnUrl, phân quyền về trang tương ứng
                    if (role == "QuanLy" || role == "NhanVien")
                        return RedirectToAction("Index", "SanPhams");

                    return RedirectToAction("Index", "Home");
                }

                // Nếu sai tài khoản/mật khẩu
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            // Nếu dữ liệu không hợp lệ hoặc sai pass, trả về chính trang Login kèm lỗi
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // 3. Đăng xuất
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        // 1. Hiển thị trang Đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 2. Xử lý lưu Khách hàng mới
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                var check = _context.Nguois.Any(u => u.TenDangNhap == model.TenDangNhap);
                if (check)
                {
                    ModelState.AddModelError("", "Tên đăng nhập này đã có người dùng rồi!");
                    return View(model);
                }

                // Tạo một đối tượng Khách Hàng mới
                var khachHang = new KhachHang
                {
                    Hoten = model.Hoten,
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = model.MatKhau, // Trong thực tế nên mã hóa mật khẩu nhé!
                    Diachi = model.Diachi,
                    Sdt = model.Sdt
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login"); // Đăng ký xong thì cho sang trang Đăng nhập
            }
            return View(model);
        }
    }
}