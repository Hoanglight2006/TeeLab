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

        // --- CẬP NHẬT: THÊM BỘ LỌC DANH MỤC VÀ GIÁ TIỀN ---
        public async Task<IActionResult> Index(string searchString, string category, string priceRange)
        {
            // Lấy toàn bộ danh sách sản phẩm
            var sanPhams = from s in _context.SanPhams
                           select s;

            // 1. Lọc theo Danh mục (Dựa vào tiền tố của Mã SP. VD: "AT" cho Áo thun)
            if (!string.IsNullOrEmpty(category))
            {
                sanPhams = sanPhams.Where(s => s.MaSP.StartsWith(category));
            }

            // 2. Lọc theo Từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                sanPhams = sanPhams.Where(s => s.TenSP.Contains(searchString));
            }

            // 3. Lọc theo Khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange)
                {
                    case "duoi300":
                        sanPhams = sanPhams.Where(s => s.SoTien < 300000);
                        break;
                    case "300den500":
                        sanPhams = sanPhams.Where(s => s.SoTien >= 300000 && s.SoTien <= 500000);
                        break;
                    case "tren500":
                        sanPhams = sanPhams.Where(s => s.SoTien > 500000);
                        break;
                }
            }

            // Truyền dữ liệu về View để giữ trạng thái đang chọn của người dùng
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentPrice"] = priceRange;

            return View(await sanPhams.ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var sanPham = await _context.SanPhams.FirstOrDefaultAsync(m => m.MaSP == id);
            if (sanPham == null) return NotFound();
            return View(sanPham);
        }

        public IActionResult Privacy() { return View(); }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}