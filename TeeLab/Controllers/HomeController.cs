using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Teelab.Models;
using TeeLab.Models;

namespace TeeLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var danhSachSanPham = await _context.SanPhams.ToListAsync();
            return View(danhSachSanPham);
        }

        // --- THÊM HÀM DETAILS NÀY VÀO ---
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Tìm sản phẩm trong DB theo ID
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