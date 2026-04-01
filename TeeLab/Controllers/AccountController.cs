using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Teelab.DataBase;
using Teelab.Models;

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
                        new Claim("UserId", user.Id.ToString()) // Lưu ID để dùng cho Profile
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    if (role == "QuanLy" || role == "NhanVien") return RedirectToAction("Index", "SanPhams");

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
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Nguois.Any(u => u.TenDangNhap == model.TenDangNhap))
                {
                    ModelState.AddModelError("", "Tên đăng nhập này đã tồn tại!");
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
        [Authorize]
        public IActionResult Profile()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return RedirectToAction("Login");

            int userId = int.Parse(userIdClaim);
            var user = _context.Nguois.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();

            var model = new UserProfileViewModel
            {
                HoTen = user.Hoten ?? "",
                Email = user.Email ?? "",
                SoDienThoai = user.Sdt ?? "",
                DiaChi = user.Diachi ?? "",
                Avatar = user.Avatar // Cần thêm thuộc tính này vào ViewModel
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel model, IFormFile? AvatarFile, string? OldPassword, string? NewPassword, string? ConfirmPassword)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId)) return RedirectToAction("Login");

            var userInDb = _context.Nguois.FirstOrDefault(u => u.Id == userId);
            if (userInDb == null) return NotFound();

            // --- KIỂM TRA MẬT KHẨU ---
            if (!string.IsNullOrEmpty(NewPassword))
            {
                // 1. Kiểm tra mật khẩu cũ (So sánh trực tiếp nếu bạn chưa hash, hoặc dùng BCrypt nếu có hash)
                if (userInDb.MatKhau != OldPassword)
                {
                    ModelState.AddModelError("OldPassword", "Mật khẩu cũ không chính xác!");
                }

                // 2. Kiểm tra xác nhận mật khẩu (Dù JS đã check nhưng Server vẫn nên check lại)
                if (NewPassword != ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp!");
                }

                // Nếu có lỗi mật khẩu, trả về View luôn để hiện thông báo đỏ
                if (!ModelState.IsValid)
                {
                    // Gán lại Avatar hiện tại để View không bị lỗi hiển thị ảnh
                    model.Avatar = userInDb.Avatar;
                    return View("Profile", model);
                }

                // Nếu mọi thứ ổn thì mới gán mật khẩu mới
                userInDb.MatKhau = NewPassword;
            }

            // --- CẬP NHẬT THÔNG TIN KHÁC ---
            // Lưu Avatar
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(AvatarFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(path, FileMode.Create)) { await AvatarFile.CopyToAsync(stream); }
                userInDb.Avatar = "/images/" + fileName;
            }

            // Cập nhật Email/SĐT (Logic giữ nguyên của bạn)
            if (!string.IsNullOrEmpty(model.Email) && !model.Email.Contains("*")) userInDb.Email = model.Email;
            if (!string.IsNullOrEmpty(model.SoDienThoai) && !model.SoDienThoai.Contains("*")) userInDb.Sdt = model.SoDienThoai;

            userInDb.Hoten = model.HoTen;
            userInDb.Diachi = model.DiaChi;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật thành công!";

            return RedirectToAction("Profile");
        }
    }
}