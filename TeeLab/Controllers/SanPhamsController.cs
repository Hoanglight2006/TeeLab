using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teelab.Models;

namespace TeeLab.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SanPhamsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
            return View(await _context.SanPhams.ToListAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(m => m.MaSP == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bỏ MaSP ra khỏi Bind, thêm tham số string MaLoai
        public async Task<IActionResult> Create([Bind("TenSP,SoTien,SoLuong,MoTa")] SanPham sanPham, string MaLoai, IFormFile? hinhAnhSP, List<string> selectedSizes, List<string> selectedColors)
        {
            // Bỏ qua kiểm tra lỗi cho các trường này vì ta tự xử lý
            ModelState.Remove("MaSP");
            ModelState.Remove("KichThuoc");
            ModelState.Remove("MauSac");

            // --- SỬA Ở ĐÂY: LOGIC TỰ ĐỘNG SINH MÃ SẢN PHẨM ---
            // Tìm sản phẩm có mã bắt đầu bằng MaLoai (VD: "AT") và có số thứ tự lớn nhất
            var lastSp = await _context.SanPhams
                .Where(s => s.MaSP!.StartsWith(MaLoai))
                .OrderByDescending(s => s.MaSP)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastSp != null)
            {
                // Cắt bỏ phần chữ, lấy phần số phía sau và tăng lên 1
                string lastNumberStr = lastSp.MaSP!.Substring(MaLoai.Length);
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            // Gán mã mới theo chuẩn: Tiền tố + 3 chữ số (Ví dụ: AT001, AT002)
            sanPham.MaSP = MaLoai + nextNumber.ToString("D3");
            // --------------------------------------------------

            if (ModelState.IsValid)
            {
                // Gộp danh sách checkbox thành chuỗi cách nhau bằng dấu phẩy
                sanPham.KichThuoc = selectedSizes.Any() ? string.Join(", ", selectedSizes) : null;
                sanPham.MauSac = selectedColors.Any() ? string.Join(", ", selectedColors) : null;

                // --- XỬ LÝ UPLOAD FILE ẢNH Ở ĐÂY ---
                if (hinhAnhSP != null && hinhAnhSP.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + hinhAnhSP.FileName;
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "sanpham");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnhSP.CopyToAsync(fileStream);
                    }
                    sanPham.HinhAnh = uniqueFileName;
                }

                sanPham.TinhTrang = sanPham.SoLuong > 0 ? "Còn hàng" : "Hết hàng";

                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bỏ KichThuoc, MauSac ra khỏi Bind, thay bằng 2 tham số List<string>
        public async Task<IActionResult> Edit(string id, [Bind("MaSP,TenSP,SoTien,SoLuong,MoTa")] SanPham sanPham, IFormFile? hinhAnhSP, List<string> selectedSizes, List<string> selectedColors)
        {
            if (id != sanPham.MaSP) return NotFound();

            ModelState.Remove("TinhTrang");
            ModelState.Remove("HinhAnh");
            ModelState.Remove("KichThuoc");
            ModelState.Remove("MauSac");

            if (ModelState.IsValid)
            {
                try
                {
                    // Gộp danh sách checkbox thành chuỗi
                    sanPham.KichThuoc = selectedSizes.Any() ? string.Join(", ", selectedSizes) : null;
                    sanPham.MauSac = selectedColors.Any() ? string.Join(", ", selectedColors) : null;

                    var sanPhamCu = await _context.SanPhams.AsNoTracking().FirstOrDefaultAsync(s => s.MaSP == id);

                    if (hinhAnhSP != null && hinhAnhSP.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(hinhAnhSP.FileName);
                        string path = Path.Combine(_env.WebRootPath, "images", "sanpham", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await hinhAnhSP.CopyToAsync(stream);
                        }
                        sanPham.HinhAnh = fileName;

                        if (sanPhamCu != null && !string.IsNullOrEmpty(sanPhamCu.HinhAnh))
                        {
                            var oldPath = Path.Combine(_env.WebRootPath, "images", "sanpham", sanPhamCu.HinhAnh);
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }
                    }
                    else
                    {
                        sanPham.HinhAnh = sanPhamCu?.HinhAnh;
                    }

                    sanPham.TinhTrang = sanPham.SoLuong > 0 ? "Còn hàng" : "Hết hàng";

                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.MaSP)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(m => m.MaSP == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(string id)
        {
            return _context.SanPhams.Any(e => e.MaSP == id);
        }
    }
}