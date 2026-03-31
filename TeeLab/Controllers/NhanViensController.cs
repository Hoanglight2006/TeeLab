using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teelab.Models;
using TeeLab.Models;

namespace TeeLab.Controllers
{
    [Authorize(Roles = "NhanVien,QuanLy")]
    public class NhanViensController : Controller
    {
        private readonly AppDbContext _context;

        public NhanViensController(AppDbContext context)
        {
            _context = context;
        }

        // PHÂN LUỒNG THÔNG MINH
        public async Task<IActionResult> Index()
        {
            // 1. Nếu người đang đăng nhập là Admin (Quản lý)
            if (User.IsInRole("QuanLy"))
            {
                // Lấy danh sách nhân viên và ném sang giao diện quản lý nhân sự
                var danhSachNV = await _context.NhanViens.ToListAsync();
                return View("DanhSachNhanVien", danhSachNV);
            }

            // 2. Nếu người đang đăng nhập là Nhân viên
            // Lấy danh sách đơn hàng ném sang giao diện duyệt đơn (Index.cshtml)
            var danhSachDonHang = await _context.ThanhToans
                .Include(t => t.KhachHang)
                .OrderByDescending(t => t.NgayTao)
                .ToListAsync();

            return View(danhSachDonHang);
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatTrangThai(string maTT, string trangThaiMoi)
        {
            var donHang = await _context.ThanhToans.FindAsync(maTT);
            if (donHang != null)
            {
                donHang.TrangThai = trangThaiMoi;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã cập nhật đơn {maTT} thành: {trangThaiMoi}";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var nhanVien = await _context.NhanViens.FirstOrDefaultAsync(m => m.Id == id);
            if (nhanVien == null) return NotFound();
            return View(nhanVien);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // BỔ SUNG TenDangNhap và MatKhau vào Bind
        public async Task<IActionResult> Create([Bind("Id,Hoten,Diachi,Ngaysinh,Sdt,TenDangNhap,MatKhau")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã bị ai dùng chưa (tránh trùng lặp)
                if (_context.Nguois.Any(u => u.TenDangNhap == nhanVien.TenDangNhap))
                {
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập này đã tồn tại trên hệ thống!");
                    return View(nhanVien);
                }

                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null) return NotFound();
            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // BỔ SUNG TenDangNhap và MatKhau vào Bind
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hoten,Diachi,Ngaysinh,Sdt,TenDangNhap,MatKhau")] NhanVien nhanVien)
        {
            if (id != nhanVien.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy user cũ để so sánh, tránh trường hợp đổi tên đăng nhập trùng với người khác
                    var oldUser = await _context.Nguois.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                    if (oldUser != null && oldUser.TenDangNhap != nhanVien.TenDangNhap)
                    {
                        if (_context.Nguois.Any(u => u.TenDangNhap == nhanVien.TenDangNhap))
                        {
                            ModelState.AddModelError("TenDangNhap", "Tên đăng nhập này đã tồn tại!");
                            return View(nhanVien);
                        }
                    }

                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var nhanVien = await _context.NhanViens.FirstOrDefaultAsync(m => m.Id == id);
            if (nhanVien == null) return NotFound();
            return View(nhanVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null) _context.NhanViens.Remove(nhanVien);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
            return _context.NhanViens.Any(e => e.Id == id);
        }
    }
}