using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; // Đảm bảo có dòng này để dùng Session
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Teelab.Models;
using TeeLab.Models;
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
                var user = _context.Nguois.FirstOrDefault(u => u.TenDangNhap == model.TenDangNhap);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.MatKhau, user.MatKhau))
                {
                    if (user is KhachHang kh && kh.IsLocked)
                    {
                        ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ Admin!");
                        return View(model);
                    }
                    string role = "KhachHang";
                    if (user is QuanLy) role = "QuanLy";
                    else if (user is NhanVien) role = "NhanVien";

                    // 1. Lấy tên file ảnh (Chỉ lấy tên file: default-avatar.png hoặc xxxx.jpg)
                    string fileName = string.IsNullOrEmpty(user.Avatar) ? "default-avatar.png" : user.Avatar;

                    // 2. Nạp vào Session đường dẫn đầy đủ để Layout dùng
                    HttpContext.Session.SetString("UserAvatar", "/images/avatars/" + fileName);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Hoten!),
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.Email, user.Email ?? ""),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Avatar", fileName) // Dùng fileName ở đây để hết lỗi CS0103
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("UserAvatar");
            HttpContext.Session.Remove("GioHang");
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
                if (!string.IsNullOrEmpty(model.Sdt))
                {
                    var phoneRegex = @"^(0[3|5|7|8|9])[0-9]{8}$";
                    if (!Regex.IsMatch(model.Sdt, phoneRegex))
                    {
                        ModelState.AddModelError("Sdt", "Số điện thoại không đúng định dạng!");
                        return View(model);
                    }
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
                    MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau),
                    Sdt = model.Sdt,
                    Email = model.Email,
                    Avatar = fileName // Lưu tên file vào Database
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Tai Khoan Cua Ban Duoc Tao Thanh Cong!";
                return RedirectToAction("Register");
            }
            return View(model);
        }
        [HttpGet]
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
                Avatar = string.IsNullOrEmpty(user.Avatar) ? "default-avatar.png" : user.Avatar
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel model, IFormFile? AvatarFile, string? OldPassword, string? NewPassword, string? ConfirmPassword)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId)) return RedirectToAction("Login");

            var userInDb = _context.Nguois.FirstOrDefault(u => u.Id == userId);
            if (userInDb == null) return NotFound();

            if (!string.IsNullOrEmpty(NewPassword))
            {
                // 1. Kiểm tra mật khẩu cũ bằng Verify
                if (string.IsNullOrEmpty(OldPassword) || !BCrypt.Net.BCrypt.Verify(OldPassword, userInDb.MatKhau))
                {
                    ModelState.AddModelError("OldPassword", "Mật khẩu cũ không chính xác!");
                }

                // 2. Kiểm tra mật khẩu xác nhận
                if (NewPassword != ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp!");
                }

                if (ModelState.IsValid)
                {
                    // 3. Băm mật khẩu mới trước khi lưu
                    userInDb.MatKhau = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                }
                else
                {
                    model.Avatar = userInDb.Avatar;
                    return View("Profile", model);
                }
            }

            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(AvatarFile.FileName);
                // Lưu đúng vào thư mục avatars
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatars", fileName); 
                using (var stream = new FileStream(path, FileMode.Create)) { await AvatarFile.CopyToAsync(stream); }
                userInDb.Avatar = fileName;
            }

            if (!string.IsNullOrEmpty(model.Email) && !model.Email.Contains("*")) userInDb.Email = model.Email;
            if (!string.IsNullOrEmpty(model.SoDienThoai) && !model.SoDienThoai.Contains("*")) userInDb.Sdt = model.SoDienThoai;

            userInDb.Hoten = model.HoTen;
            userInDb.Diachi = model.DiaChi;

            await _context.SaveChangesAsync();

            // Cập nhật lại Session ngay để Header đổi ảnh
            string finalAvatar = string.IsNullOrEmpty(userInDb.Avatar) ? "default-avatar.png" : userInDb.Avatar;
            HttpContext.Session.SetString("UserAvatar", "/images/avatars/" + finalAvatar);

            TempData["Success"] = "";
            return RedirectToAction("Profile");
        }
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra khớp cả Tên đăng nhập VÀ Email
                var user = await _context.Nguois.FirstOrDefaultAsync(u =>
                    u.TenDangNhap == model.TenDangNhap && u.Email == model.Email);

                if (user != null)
                {
                    string otpCode = new Random().Next(100000, 999999).ToString();
                    HttpContext.Session.SetString("OTPCode", otpCode);
                    HttpContext.Session.SetString("OTPEmail", model.Email);
                    HttpContext.Session.SetString("ResetUser", model.TenDangNhap); // Lưu lại để biết đổi cho ai

                    try
                    {
                        var emailService = new EmailService();
                        await emailService.SendEmailAsync(model.Email, "Xác nhận đổi mật khẩu", $"Mã OTP của bạn là: {otpCode}");
                        return RedirectToAction("VerifyOTP");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi gửi mail: " + ex.Message);
                    }
                }
                else
                {
                    // Thông báo lỗi chung để bảo mật hoặc báo chi tiết tùy ông
                    ModelState.AddModelError("", "Tên đăng nhập hoặc Email không chính xác!");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            var userName = HttpContext.Session.GetString("ResetUser");
            var email = HttpContext.Session.GetString("OTPEmail");

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("ForgotPassword");
            }

            // Gán sẵn Email vào model để vượt qua kiểm tra Valid
            var model = new ResetPasswordViewModel
            {
                Email = email ?? ""
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            // Bỏ qua kiểm tra Email và Token vì mình dùng Session để xác định User
            ModelState.Remove("Email");
            ModelState.Remove("Token");

            if (ModelState.IsValid)
            {
                var userName = HttpContext.Session.GetString("ResetUser");
                if (string.IsNullOrEmpty(userName)) return RedirectToAction("ForgotPassword");

                var user = await _context.Nguois.FirstOrDefaultAsync(u => u.TenDangNhap == userName);

                if (user != null)
                {
                    // 1. Gán mật khẩu mới đã băm
                    user.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

                    // 2. Ép Entity Framework đánh dấu là đã thay đổi
                    _context.Nguois.Update(user);

                    // 3. Lưu vào DB
                    var result = await _context.SaveChangesAsync();

                    if (result > 0) // Kiểm tra xem có dòng nào được cập nhật không
                    {
                        // 4. Dọn dẹp Session
                        HttpContext.Session.Remove("ResetUser");
                        HttpContext.Session.Remove("OTPCode");
                        HttpContext.Session.Remove("OTPEmail");

                        TempData["Success"] = "Đổi mật khẩu thành công! Vui lòng đăng nhập.";
                        return RedirectToAction("Login");
                    }
                }
            }

            // Nếu bị văng ra đây, ông hãy Debug xem ModelState có lỗi gì:
            // var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(model);
        }
        [HttpGet]
        public IActionResult VerifyOTP()
        {
            return View();
        }
        [HttpPost]
        public IActionResult VerifyOTP(string otp)
        {
            var savedOtp = HttpContext.Session.GetString("OTPCode");
            var userName = HttpContext.Session.GetString("ResetUser"); 

            if (otp == savedOtp && !string.IsNullOrEmpty(userName))
            {
                HttpContext.Session.SetString("ResetUser", userName);
                return RedirectToAction("ResetPassword");
            }

            ModelState.AddModelError("", "Mã xác nhận không chính xác!");
            return View();
        }
    }
   
}