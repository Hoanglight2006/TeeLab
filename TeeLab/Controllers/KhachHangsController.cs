using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeeLab.Models;
using Teelab.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TeeLab.Controllers
{
    // Đã xóa chữ [Authorize] ở đây để phân quyền chi tiết cho từng hàm
    public class KhachHangsController : Controller
    {
        private readonly AppDbContext _context;

        public KhachHangsController(AppDbContext context)
        {
            _context = context;
        }

        // CHỈ KHÁCH HÀNG MỚI ĐƯỢC XEM LỊCH SỬ CỦA HỌ
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim);

            var lichSuDonHang = await _context.ThanhToans
                .Where(t => t.Id == userId)
                .OrderByDescending(t => t.NgayTao)
                .ToListAsync();

            return View(lichSuDonHang);
        }

        // --- MỞ KHÓA: NHÂN VIÊN, QUẢN LÝ VÀ CHÍNH KHÁCH HÀNG ĐƯỢC XEM HỒ SƠ ---
        [Authorize(Roles = "KhachHang,NhanVien,QuanLy")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(m => m.Id == id);
            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // CÁC HÀM CRUD CÒN LẠI (Create, Edit, Delete) CHỈ DÀNH CHO KHÁCH HÀNG
        [Authorize(Roles = "KhachHang")]
        public IActionResult Create() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> Create([Bind("Id,Hoten,Diachi,Ngaysinh,Sdt")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hoten,Diachi,Ngaysinh,Sdt")] KhachHang khachHang)
        {
            if (id != khachHang.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try { _context.Update(khachHang); await _context.SaveChangesAsync(); }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(m => m.Id == id);
            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null) _context.KhachHangs.Remove(khachHang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id) { return _context.KhachHangs.Any(e => e.Id == id); }
    }
}