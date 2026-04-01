using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Teelab.Models;
using TeeLab.Models;
using Microsoft.AspNetCore.Hosting; // Thêm thư viện này

namespace Teelab.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env; // Dùng để lấy đường dẫn wwwroot

        public AccountController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

                    // Gắn thêm Avatar vào "Chứng minh thư" (Claims) của phiên đăng nhập
                    string avatarPath = string.IsNullOrEmpty(user.Avatar) ? "default-avatar.png" : user.Avatar;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Hoten),
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.Email, user.Email ?? ""), // Lưu Email
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Avatar", avatarPath) // LƯU AVATAR ĐỂ VIEW LẤY LÊN
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    if (role == "QuanLy") return RedirectToAction("Index", "QuanLies");
                    if (role == "NhanVien") return RedirectToAction("Index", "NhanViens");
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
                if (_context.Nguois.Any(u => u.TenDangNhap == model.TenDangNhap))
                {
                    ModelState.AddModelError("", "Tên đăng nhập này đã có người dùng rồi!");
                    return View(model);
                }

                string fileName = "default-avatar.png"; // Ảnh mặc định

                // Xử lý lưu ảnh nếu người dùng có chọn file
                if (model.AvatarFile != null && model.AvatarFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(_env.WebRootPath, "images", "avatars");
                    if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                    // Đổi tên file để không bị trùng (dùng Guid)
                    fileName = Guid.NewGuid().ToString() + "_" + model.AvatarFile.FileName;
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.AvatarFile.CopyToAsync(fileStream);
                    }
                }

                var khachHang = new KhachHang
                {
                    Hoten = model.Hoten,
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = model.MatKhau,
                    Sdt = model.Sdt,
                    Email = model.Email,
                    Avatar = fileName // Lưu tên file vào Database
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}