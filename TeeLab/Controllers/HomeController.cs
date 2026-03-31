using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Teelab.Models;
using TeeLab.Models;
using System.Linq;

namespace TeeLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // --- CẬP NHẬT: THÊM TÍNH NĂNG SEARCH ---
        public async Task<IActionResult> Index(string searchString)
        {
            // Lấy toàn bộ danh sách sản phẩm
            var sanPhams = from s in _context.SanPhams
                           select s;

            // Nếu người dùng có nhập từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                sanPhams = sanPhams.Where(s => s.TenSP.Contains(searchString));
            }

            // Truyền từ khóa ngược lại View để giữ chữ hiển thị trên ô input
            ViewData["CurrentFilter"] = searchString;

            return View(await sanPhams.ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FirstOrDefaultAsync(m => m.MaSP == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}