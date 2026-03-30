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
        public IActionResult Login()
        {
            return View();
        }

        // 2. Xử lý khi bấm nút Đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Dò tìm tài khoản và mật khẩu trong bảng Nguois
                var user = _context.Nguois.FirstOrDefault(u => u.TenDangNhap == model.TenDangNhap && u.MatKhau == model.MatKhau);

                if (user != null)
                {
                    // EF Core cực hay: Tự biết user này là thuộc class nào (QuanLy, NhanVien hay KhachHang)
                    string role = "KhachHang";
                    if (user is QuanLy) role = "QuanLy";
                    else if (user is NhanVien) role = "NhanVien";

                    // Tạo "Chứng minh thư" (Claims) cho phiên đăng nhập này
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Hoten),
                        new Claim(ClaimTypes.Role, role), // Gắn vai trò để phân quyền
                        new Claim("UserId", user.Id.ToString()) // Lưu lại Id để sau này làm Giỏ hàng
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Chính thức Đăng nhập (Lưu Cookie)
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Đăng nhập xong thì đi đâu?
                    if (role == "QuanLy" || role == "NhanVien")
                        return RedirectToAction("Index", "SanPhams"); // Sếp/Nhân viên thì cho vào kho
                    else
                        return RedirectToAction("Index", "Home"); // Khách thì đẩy ra trang chủ mua hàng
                }

                // Nếu sai tài khoản/mật khẩu
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            return View(model);
        }

        // 3. Đăng xuất
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}