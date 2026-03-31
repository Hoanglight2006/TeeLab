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

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

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

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // --- SỬA Ở ĐÂY: CHUYỂN HƯỚNG VỀ ĐÚNG TRANG CÁ NHÂN ---
                    if (role == "QuanLy") return RedirectToAction("Index", "QuanLies");
                    if (role == "NhanVien") return RedirectToAction("Index", "NhanViens");
                    // -----------------------------------------------------

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var check = _context.Nguois.Any(u => u.TenDangNhap == model.TenDangNhap);
                if (check)
                {
                    ModelState.AddModelError("", "Tên đăng nhập này đã có người dùng rồi!");
                    return View(model);
                }

                var khachHang = new KhachHang
                {
                    Hoten = model.Hoten,
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = model.MatKhau,
                    Diachi = model.Diachi,
                    Sdt = model.Sdt
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}