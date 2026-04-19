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
                .SelectMany(t => t.ChiTietThanhToans!)
                .GroupBy(ct => new { ct.MaSP, TenSP = ct.SanPham != null ? ct.SanPham.TenSP : "Sản phẩm đã xóa" })
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

            return View();
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
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> OrderDetails(string id)
        {
            if (id == null) return NotFound();

            // 1. Tìm hóa đơn và nạp kèm chi tiết sản phẩm
            var donHang = await _context.ThanhToans
                .Include(t => t.ChiTietThanhToans)!
                .ThenInclude(ct => ct.SanPham) // Nạp bảng SanPham để lấy giá gốc nếu cần
                .FirstOrDefaultAsync(m => m.MaTT == id);

            if (donHang == null) return NotFound();

            // 2. LOGIC FIX LỖI SỐ 0: Kiểm tra từng dòng chi tiết
            foreach (var item in donHang.ChiTietThanhToans!)
            {
                // Nếu DonGia trong database đang là 0, thì lấy giá từ bảng SanPham bù vào
                if (item.Gia == 0 && item.SanPham != null)
                {
                    item.Gia = (int)item.SanPham.SoTien; // Giả sử cột giá trong SanPham là GiaBan
                }
            }

            return View(donHang);
        }
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> CustomerHistory(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null) return NotFound();

            // Lấy lịch sử đơn hàng dựa trên cột Id (Khóa ngoại trỏ về khách hàng)
            var lichSuDon = await _context.ThanhToans
                .Where(t => t.Id == id)
                .OrderByDescending(t => t.NgayTao)
                .ToListAsync();

            ViewBag.KhachHang = khachHang;
            return View(lichSuDon);
        }
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> ManegerCustomers(string searchString, string filterRank)
        {
            var query = _context.KhachHangs.AsQueryable();

            // Lọc tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(k => k.Hoten!.Contains(searchString) ||
                                         k.Sdt!.Contains(searchString) ||
                                         k.TenDangNhap!.Contains(searchString));
            }

            // Lọc hạng
            if (!string.IsNullOrEmpty(filterRank))
            {
                query = query.Where(k => k.HangThanhVien == filterRank);
            }

            var listKhachHang = await query.ToListAsync();

            // TÍNH TOÁN: Tạo một Dictionary lưu [MaKH - TongTien]
            // Chỉ tính những đơn hàng có trạng thái "Giao hàng thành công"
            var tongChiTieu = await _context.ThanhToans
                .Where(t => t.TrangThai == "Giao hàng thành công")
                .GroupBy(t => t.Id)
                .Select(g => new { MaKH = g.Key, TongTien = g.Sum(t => t.TongTien) })
                .ToDictionaryAsync(x => x.MaKH, x => x.TongTien);

            foreach (var kh in listKhachHang)
            {
 
                    // Lấy tổng tiền từ Dictionary, nếu không có đơn nào thành công thì mặc định là 0
                    decimal total = tongChiTieu.ContainsKey(kh.Id) ? tongChiTieu[kh.Id] : 0;

                    if (total >= 5000000) kh.HangThanhVien = "Kim Cương";
                    else if (total >= 2000000) kh.HangThanhVien = "Vàng";
                    else if (total >= 1000000) kh.HangThanhVien = "Bạc";
                    else if (total > 0) kh.HangThanhVien = "Đồng";
                    else kh.HangThanhVien = "Mới";
                _context.Update(kh);
            }
            await _context.SaveChangesAsync(); // Lưu tất cả thay đổi hạng vào DB

            ViewBag.TongChiTieu = tongChiTieu;
            ViewBag.SearchString = searchString;
            ViewBag.FilterRank = filterRank;

            return View(listKhachHang);
        }
        [HttpPost]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> ToggleLock(int id)
        {
            var kh = await _context.KhachHangs.FindAsync(id);
            if (kh != null)
            {
                kh.IsLocked = !kh.IsLocked; // Đảo trạng thái: Đang mở -> Khóa, đang khóa -> Mở
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật trạng thái thành công!";
            }
            return RedirectToAction(nameof(ManegerCustomers));
        }
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> AdminEdit(int? id)
        {
            if (id == null) return NotFound();
            var kh = await _context.KhachHangs.FindAsync(id);
            if (kh == null) return NotFound();
            return View(kh);
        }

        [HttpPost]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> AdminEdit(int id, KhachHang kh)
        {
            if (id != kh.Id) return NotFound();

            // Loại bỏ kiểm tra ModelState nếu ông chỉ muốn cập nhật một vài trường
            // hoặc đảm bảo các trường bắt buộc không bị null
            try
            {
                var khInDb = await _context.KhachHangs.FindAsync(id);
                if (khInDb == null) return NotFound();

                // Cập nhật các thông tin từ Form truyền về
                khInDb.Hoten = kh.Hoten;
                khInDb.Sdt = kh.Sdt;
                khInDb.Email = kh.Email;
                khInDb.Diachi = kh.Diachi;
                khInDb.HangThanhVien = kh.HangThanhVien; // ĐÂY LÀ DÒNG LƯU HẠNG ÔNG VỪA CHỌN
                khInDb.IsLocked = kh.IsLocked;

                _context.Update(khInDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManegerCustomers));
            }
            catch (Exception)
            {
                return View(kh);
            }
        }
    }
}
