using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TeeLab.Models;
using Teelab.Models;

namespace TeeLab.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class QuanLiesController : Controller
    {
        private readonly AppDbContext _context;

        public QuanLiesController(AppDbContext context)
        {
            _context = context;
        }

        // --- CẬP NHẬT: Thêm tham số lọc thời gian và Tính Top 5 Sản phẩm ---
        public async Task<IActionResult> Index(DateTime? tuNgay, DateTime? denNgay)
        {
            // 1. Tạo query gốc lấy các đơn hàng
            var queryTatCaDon = _context.ThanhToans.AsQueryable();
            var queryDonThanhCong = _context.ThanhToans.Where(t => t.TrangThai == "Giao hàng thành công");

            // 2. Lọc theo thời gian (nếu có chọn)
            if (tuNgay.HasValue)
            {
                queryTatCaDon = queryTatCaDon.Where(t => t.NgayTao >= tuNgay.Value);
                queryDonThanhCong = queryDonThanhCong.Where(t => t.NgayTao >= tuNgay.Value);
            }
            if (denNgay.HasValue)
            {
                // Cộng thêm 1 ngày để bao gồm trọn vẹn ngày kết thúc
                var toDate = denNgay.Value.AddDays(1);
                queryTatCaDon = queryTatCaDon.Where(t => t.NgayTao < toDate);
                queryDonThanhCong = queryDonThanhCong.Where(t => t.NgayTao < toDate);
            }

            // 3. Tính toán các con số tổng quan
            var doanhThu = await queryDonThanhCong.SumAsync(t => t.TongTien);

            // CẬP NHẬT: Chỉ đếm những đơn hàng KHÔNG bị hủy
            var tongDon = await queryTatCaDon.Where(t => t.TrangThai != "Đã hủy").CountAsync();

            var spHetHang = await _context.SanPhams.CountAsync(s => s.SoLuong < 5);

            // 4. Lấy TOP 5 SẢN PHẨM BÁN CHẠY NHẤT (Chỉ tính đơn thành công)
            var topSanPham = await queryDonThanhCong
                .SelectMany(t => t.ChiTietThanhToans)
                .GroupBy(ct => new { ct.MaSP, ct.SanPham.TenSP })
                .Select(g => new
                {
                    TenSP = g.Key.TenSP ?? "Sản phẩm đã xóa",
                    TongSoLuong = g.Sum(ct => ct.SoLuong)
                })
                .OrderByDescending(x => x.TongSoLuong)
                .Take(5)
                .ToListAsync();

            // Gửi dữ liệu thống kê ra View
            ViewBag.DoanhThu = doanhThu;
            ViewBag.TongDon = tongDon;
            ViewBag.SpHetHang = spHetHang;

            // Gửi ngày tháng ra View để giữ lại trên Form
            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");

            // Gửi dữ liệu cho Chart.js vẽ biểu đồ
            ViewBag.ChartLabels = topSanPham.Select(x => x.TenSP).ToArray();
            ViewBag.ChartData = topSanPham.Select(x => x.TongSoLuong).ToArray();

            return View(await _context.QuanLys.ToListAsync());
        }

        // GET: QuanLies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuanLies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Hoten,Diachi,Ngaysinh,Sdt")] QuanLy quanLy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quanLy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quanLy);
        }

        // GET: QuanLies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quanLy = await _context.QuanLys.FindAsync(id);
            if (quanLy == null)
            {
                return NotFound();
            }
            return View(quanLy);
        }

        // POST: QuanLies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hoten,Diachi,Ngaysinh,Sdt")] QuanLy quanLy)
        {
            if (id != quanLy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quanLy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuanLyExists(quanLy.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(quanLy);
        }

        // GET: QuanLies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quanLy = await _context.QuanLys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quanLy == null)
            {
                return NotFound();
            }

            return View(quanLy);
        }

        // POST: QuanLies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quanLy = await _context.QuanLys.FindAsync(id);
            if (quanLy != null)
            {
                _context.QuanLys.Remove(quanLy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuanLyExists(int id)
        {
            return _context.QuanLys.Any(e => e.Id == id);
        }
    }
}
