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
            if (User.IsInRole("QuanLy"))
            {
                var danhSachNV = await _context.NhanViens.ToListAsync();
                return View("DanhSachNhanVien", danhSachNV);
            }

            // --- CẬP NHẬT: Lấy thêm Chi tiết Hóa đơn và Sản phẩm ---
            var danhSachDonHang = await _context.ThanhToans
                .Include(t => t.KhachHang)
                .Include(t => t.ChiTietThanhToans)
                    .ThenInclude(ct => ct.SanPham) // Lấy Tên sản phẩm để hiển thị
                .OrderByDescending(t => t.NgayTao)
                .ToListAsync();

            return View(danhSachDonHang);
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatTrangThai(string maTT, string trangThaiMoi)
        {
            // CẬP NHẬT: Phải Include thêm ChiTietThanhToans để lấy thông tin mã áo và số lượng
            var donHang = await _context.ThanhToans
                .Include(t => t.ChiTietThanhToans)
                .FirstOrDefaultAsync(t => t.MaTT == maTT);

            if (donHang != null)
            {
                // --- CHẶN BẢO MẬT TỪ SERVER: Đã giao hoặc Đã hủy thì cấm sửa ---
                if (donHang.TrangThai == "Giao hàng thành công" || donHang.TrangThai == "Đã hủy")
                {
                    TempData["Error"] = "Đơn hàng này đã chốt, không thể thay đổi trạng thái!";
                    return RedirectToAction(nameof(Index));
                }

                // --- TÍNH NĂNG MỚI: HOÀN KHO KHI HỦY ĐƠN ---
                if (trangThaiMoi == "Đã hủy")
                {
                    // Duyệt qua từng món hàng trong đơn bị hủy
                    foreach (var chiTiet in donHang.ChiTietThanhToans)
                    {
                        // Tìm sản phẩm gốc trong kho
                        var sanPham = await _context.SanPhams.FindAsync(chiTiet.MaSP);
                        if (sanPham != null)
                        {
                            // Cộng trả lại số lượng vào kho
                            sanPham.SoLuong += chiTiet.SoLuong;

                            // Cập nhật lại tình trạng nếu sản phẩm trước đó đang báo "Hết hàng"
                            if (sanPham.SoLuong > 0)
                            {
                                sanPham.TinhTrang = "Còn hàng";
                            }
                        }
                    }
                }

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