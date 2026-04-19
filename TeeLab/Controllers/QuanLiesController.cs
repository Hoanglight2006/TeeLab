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
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace TeeLab.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class QuanLiesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPdfService _pdfService; // 1. Thêm biến này

        public QuanLiesController(AppDbContext context, IPdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
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
                .Include(t => t.ChiTietThanhToans)
                .ThenInclude(ct => ct.SanPham) // Nạp bảng SanPham để lấy giá gốc nếu cần
                .FirstOrDefaultAsync(m => m.MaTT == id);

            if (donHang == null) return NotFound();

            // 2. LOGIC FIX LỖI SỐ 0: Kiểm tra từng dòng chi tiết
            foreach (var item in donHang.ChiTietThanhToans)
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
                query = query.Where(k => k.Hoten.Contains(searchString) ||
                                         k.Sdt.Contains(searchString) ||
                                         k.TenDangNhap.Contains(searchString));
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

        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> ExportDoanhThuPdf(DateTime? tuNgay, DateTime? denNgay)
        {
            // 1. Lấy dữ liệu lọc các đơn thành công kèm chi tiết sản phẩm
            var query = _context.ThanhToans
                .Include(t => t.ChiTietThanhToans)
                .ThenInclude(ct => ct.SanPham)
                .Where(t => t.TrangThai == "Giao hàng thành công");

            if (tuNgay.HasValue) query = query.Where(t => t.NgayTao >= tuNgay.Value);
            if (denNgay.HasValue) query = query.Where(t => t.NgayTao < denNgay.Value.AddDays(1));

            var listDonHang = await query.OrderByDescending(t => t.NgayTao).ToListAsync();

            // Thống kê tổng hợp sản phẩm đã bán
            var thongKeSanPham = listDonHang
    .SelectMany(t => t.ChiTietThanhToans)
    .GroupBy(ct => new { ct.MaSP, TenSP = ct.SanPham != null ? ct.SanPham.TenSP : "Sản phẩm đã xóa" })
    .Select(g => new
    {
        Ten = g.Key.TenSP,
        SoLuong = g.Sum(x => x.SoLuong),
        // Tính tổng tiền bán được của từng SP (Fix lỗi số 0)
        TongTienBan = g.Sum(x => (x.Gia > 0 ? x.Gia : (int)(x.SanPham?.SoTien ?? 0)) * x.SoLuong)
    })
    .OrderByDescending(x => x.SoLuong)
    .ToList();

            var tongDoanhThu = listDonHang.Sum(t => t.TongTien);
            var tongDonHang = listDonHang.Count;

            // 2. Tạo nội dung HTML chuyên nghiệp
            var html = $@"
    <html>
    <head>
        <style>
            body {{ font-family: 'Arial', sans-serif; color: #333; line-height: 1.5; }}
            .container {{ padding: 20px; }}
            .header {{ text-align: center; border-bottom: 2px solid #444; padding-bottom: 10px; margin-bottom: 20px; }}
            .header h1 {{ margin: 0; color: #d32f2f; text-transform: uppercase; }}
            .info-box {{ margin-bottom: 20px; font-size: 14px; }}
            table {{ width: 100%; border-collapse: collapse; margin-bottom: 20px; }}
            th, td {{ border: 1px solid #ccc; padding: 10px; text-align: left; font-size: 12px; }}
            th {{ background-color: #f8f9fa; font-weight: bold; text-transform: uppercase; }}
            .text-right {{ text-align: right; }}
            .highlight {{ background-color: #fff9c4; font-weight: bold; }}
            .footer {{ margin-top: 50px; width: 100%; }}
            .signature {{ float: right; width: 200px; text-align: center; }}
            .summary-table {{ width: 300px; float: right; }}
            .clear {{ clear: both; }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>Báo Cáo Doanh Thu Chi Tiết</h1>
                <p>Cửa hàng thời trang TeeLab</p>
            </div>

            <div class='info-box'>
                <p><b>Thời gian báo cáo:</b> {tuNgay?.ToString("dd/MM/yyyy") ?? "Tất cả"} - {denNgay?.ToString("dd/MM/yyyy") ?? "Hiện tại"}</p>
                <p><b>Ngày lập báo cáo:</b> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                <p><b>Tổng số đơn hàng thành công:</b> {tongDonHang} đơn</p>
            </div>

            <h3>1. THỐNG KÊ SẢN PHẨM BÁN CHẠY</h3>
            <table>
                <thead>
                    <tr>
                        <th>Tên Sản Phẩm</th>
                        <th class='text-right'>Số Lượng</th>
                        <th class='text-right'>Doanh Thu Thành Phần</th>
                    </tr>
                </thead>
                <tbody>";
            foreach (var sp in thongKeSanPham)
            {
                html += $@"<tr>
                        <td>{sp.Ten}</td>
                        <td class='text-right'>{sp.SoLuong:N0}</td>
                        <td class='text-right'>{sp.TongTienBan:N0} VNĐ</td>
                    </tr>";
            }
            html += $@"</tbody>
            </table>

            <h3>2. DANH SÁCH ĐƠN HÀNG CHI TIẾT</h3>
            <table>
                <thead>
                    <tr>
                        <th>Mã Đơn</th>
                        <th>Ngày Tạo</th>
                        <th>Khách Hàng (ID)</th>
                        <th class='text-right'>Tổng Tiền</th>
                    </tr>
                </thead>
                <tbody>";
            foreach (var item in listDonHang)
            {
                html += $@"<tr>
                        <td><b>{item.MaTT}</b></td>
                        <td>{item.NgayTao:dd/MM/yyyy HH:mm}</td>
                        <td>Khách hàng #{item.Id}</td>
                        <td class='text-right'>{item.TongTien:N0} VNĐ</td>
                    </tr>";
            }
            html += $@"</tbody>
            </table>

            <div class='clear'></div>
            <table class='summary-table'>
                <tr class='highlight'>
                    <td style='border:none'>TỔNG DOANH THU:</td>
                    <td class='text-right' style='border:none; color: #d32f2f; font-size: 16px;'>{tongDoanhThu:N0} VNĐ</td>
                </tr>
            </table>

            <div class='clear'></div>
            <div class='footer'>
                <div class='signature'>
                    <p><i>Ngày .... tháng .... năm ....</i></p>
                    <p><b>Người lập biểu</b></p>
                    <br><br><br>
                    <p>................................</p>
                </div>
            </div>
        </div>
    </body>
    </html>";

            // 3. Xuất file
            var file = _pdfService.CreatePdf(html);
            return File(file, "application/pdf", $"BaoCaoChiTiet_{DateTime.Now:yyyyMMdd}.pdf");
        }
        [Authorize(Roles = "QuanLy")]
public async Task<IActionResult> ExportDoanhThuWord(DateTime? tuNgay, DateTime? denNgay)
{
    // 1. Lấy dữ liệu (Giữ nguyên logic tính toán giống PDF)
    var query = _context.ThanhToans
        .Include(t => t.ChiTietThanhToans)
        .ThenInclude(ct => ct.SanPham)
        .Where(t => t.TrangThai == "Giao hàng thành công");

    if (tuNgay.HasValue) query = query.Where(t => t.NgayTao >= tuNgay.Value);
    if (denNgay.HasValue) query = query.Where(t => t.NgayTao < denNgay.Value.AddDays(1));

    var listDonHang = await query.OrderByDescending(t => t.NgayTao).ToListAsync();

    var thongKeSanPham = listDonHang
        .SelectMany(t => t.ChiTietThanhToans)
        .GroupBy(ct => new { ct.MaSP, TenSP = ct.SanPham != null ? ct.SanPham.TenSP : "Sản phẩm đã xóa" })
        .Select(g => new {
            Ten = g.Key.TenSP,
            SoLuong = g.Sum(x => x.SoLuong),
            TongTienBan = g.Sum(x => (x.Gia > 0 ? x.Gia : (int)(x.SanPham?.SoTien ?? 0)) * x.SoLuong)
        })
        .OrderByDescending(x => x.SoLuong).ToList();

    var tongDoanhThu = listDonHang.Sum(t => t.TongTien);

    // 2. Tạo file Word
    using (var stream = new MemoryStream())
    {
        using (var doc = DocX.Create(stream))
        {
            // --- HEADER (Giống PDF) ---
            var title = doc.InsertParagraph("BÁO CÁO DOANH THU CHI TIẾT").Bold().FontSize(20);
            title.Alignment = Alignment.center;

            doc.InsertParagraph("Cửa hàng thời trang TeeLab").Alignment = Alignment.center;
            doc.InsertParagraph("---------------------------------------").Alignment = Alignment.center;

            // INFO BOX
            doc.InsertParagraph($"Thời gian báo cáo: {(tuNgay?.ToString("dd/MM/yyyy") ?? "Tất cả")} - {(denNgay?.ToString("dd/MM/yyyy") ?? "Hiện tại")}");
            doc.InsertParagraph($"Ngày lập báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm}");
            doc.InsertParagraph($"Tổng số đơn hàng thành công: {listDonHang.Count} đơn");
            doc.InsertParagraph("");

            // --- 1. THỐNG KÊ SẢN PHẨM BÁN CHẠY ---
            doc.InsertParagraph("1. THỐNG KÊ SẢN PHẨM BÁN CHẠY").Bold().FontSize(14);
            var t1 = doc.AddTable(thongKeSanPham.Count + 1, 3);
            t1.Design = TableDesign.TableGrid;
            t1.Alignment = Alignment.center;
            t1.Rows[0].Cells[0].Paragraphs[0].Append("Tên Sản Phẩm").Bold();
            t1.Rows[0].Cells[1].Paragraphs[0].Append("Số Lượng").Bold();
            t1.Rows[0].Cells[2].Paragraphs[0].Append("Số tiền bán được").Bold();

            for (int i = 0; i < thongKeSanPham.Count; i++)
            {
                var sp = thongKeSanPham[i];
                t1.Rows[i + 1].Cells[0].Paragraphs[0].Append(sp.Ten);
                t1.Rows[i + 1].Cells[1].Paragraphs[0].Append(sp.SoLuong.ToString("N0")).Alignment = Alignment.right;
                t1.Rows[i + 1].Cells[2].Paragraphs[0].Append(sp.TongTienBan.ToString("N0") + " VNĐ").Alignment = Alignment.right;
            }
            doc.InsertTable(t1);
            doc.InsertParagraph("");

            // --- 2. DANH SÁCH ĐƠN HÀNG CHI TIẾT ---
            doc.InsertParagraph("2. DANH SÁCH ĐƠN HÀNG CHI TIẾT").Bold().FontSize(14);
            var t2 = doc.AddTable(listDonHang.Count + 1, 4);
            t2.Design = TableDesign.TableGrid;
            t2.Alignment = Alignment.center;
            t2.Rows[0].Cells[0].Paragraphs[0].Append("Mã Đơn").Bold();
            t2.Rows[0].Cells[1].Paragraphs[0].Append("Ngày Tạo").Bold();
            t2.Rows[0].Cells[2].Paragraphs[0].Append("Khách Hàng").Bold();
            t2.Rows[0].Cells[3].Paragraphs[0].Append("Tổng Tiền").Bold();

            for (int i = 0; i < listDonHang.Count; i++)
            {
                var item = listDonHang[i];
                t2.Rows[i + 1].Cells[0].Paragraphs[0].Append(item.MaTT).Bold();
                t2.Rows[i + 1].Cells[1].Paragraphs[0].Append(item.NgayTao.ToString("dd/MM/yyyy HH:mm"));
                t2.Rows[i + 1].Cells[2].Paragraphs[0].Append($"Khách hàng #{item.Id}");
                t2.Rows[i + 1].Cells[3].Paragraphs[0].Append(item.TongTien.ToString("N0") + " VNĐ").Alignment = Alignment.right;
            }
            doc.InsertTable(t2);

            // --- TỔNG DOANH THU (Căn phải, màu đỏ) ---
            doc.InsertParagraph("");
            var pTotal = doc.InsertParagraph($"TỔNG DOANH THU: ");
            pTotal.Append(tongDoanhThu.ToString("N0") + " VNĐ").Bold().FontSize(16);
            pTotal.Alignment = Alignment.right;

            // --- CHỮ KÝ (FOOTER) ---
            doc.InsertParagraph("");
            var signaturePara = doc.InsertParagraph($"Ngày .... tháng .... năm ....").Italic();
            signaturePara.Alignment = Alignment.right;
            var signName = doc.InsertParagraph("Người lập biểu").Bold();
            signName.Alignment = Alignment.right;
            doc.InsertParagraph("\n\n\n................................").Alignment = Alignment.right;

            doc.Save();
        }
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd}.docx");
    }
}
        }
    }