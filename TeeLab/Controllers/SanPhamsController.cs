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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Thêm tham số IFormFile hinhAnhSP vào hàm này nhé
        public async Task<IActionResult> Create([Bind("MaSP,TenSP,SoTien,SoLuong")] SanPham sanPham, IFormFile? hinhAnhSP)
        {
            if (ModelState.IsValid)
            {
                // --- XỬ LÝ UPLOAD FILE ẢNH Ở ĐÂY ---
                if (hinhAnhSP != null && hinhAnhSP.Length > 0)
                {
                    // 1. Định nghĩa tên file duy nhất (dùng GUID)
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + hinhAnhSP.FileName;

                    // 2. Định nghĩa đường dẫn lưu file vật lý (wwwroot/images/sanpham)
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "sanpham");

                    // 3. Đảm bảo thư mục tồn tại (nếu chưa có thì tạo mới)
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // 4. Lưu file vật lý xuống ổ đĩa
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnhSP.CopyToAsync(fileStream);
                    }

                    // 5. Lưu cái "Tên file" vào thuộc tính HinhAnh của SanPham để vào DB
                    sanPham.HinhAnh = uniqueFileName;
                }

                // Logic tự động trạng thái cũ giữ nguyên
                if (sanPham.SoLuong > 0) sanPham.TinhTrang = "Còn hàng";
                else sanPham.TinhTrang = "Hết hàng";

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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSP,TenSP,SoTien,SoLuong")] SanPham sanPham, IFormFile? hinhAnhSP)
        {
            if (id != sanPham.MaSP) return NotFound();

            // 1. Loại bỏ kiểm tra validate cho các trường tự động
            ModelState.Remove("TinhTrang");
            ModelState.Remove("HinhAnh");

            if (ModelState.IsValid)
            {
                try
                {
                    // 2. Lấy lại dữ liệu cũ từ DB (không cho EF theo dõi để tránh xung đột)
                    var sanPhamCu = await _context.SanPhams.AsNoTracking().FirstOrDefaultAsync(s => s.MaSP == id);

                    if (hinhAnhSP != null && hinhAnhSP.Length > 0)
                    {
                        // TRƯỜNG HỢP: CÓ UPLOAD ẢNH MỚI
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(hinhAnhSP.FileName);
                        string path = Path.Combine(_env.WebRootPath, "images", "sanpham", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await hinhAnhSP.CopyToAsync(stream);
                        }
                        sanPham.HinhAnh = fileName; // Gán tên ảnh mới

                        // (Tùy chọn) Xóa file ảnh cũ trong thư mục wwwroot để đỡ rác máy
                        if (sanPhamCu != null && !string.IsNullOrEmpty(sanPhamCu.HinhAnh))
                        {
                            var oldPath = Path.Combine(_env.WebRootPath, "images", "sanpham", sanPhamCu.HinhAnh);
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }
                    }
                    else
                    {
                        // TRƯỜNG HỢP: KHÔNG CHỌN ẢNH MỚI -> GIỮ LẠI ẢNH CŨ
                        sanPham.HinhAnh = sanPhamCu?.HinhAnh;
                    }

                    // 3. Cập nhật trạng thái
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
