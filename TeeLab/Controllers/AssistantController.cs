using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teelab.Models;
using TeeLab.Models;
using TeeLab.Services;

namespace TeeLab.Controllers
{
    public class AssistantController : Controller
    {
        // 1. Đổi ApplicationDbContext thành AppDbContext cho khớp với file bạn gửi
        private readonly AppDbContext _context;
        private readonly GeminiService _geminiService;

        public AssistantController(AppDbContext context, GeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatInput input)
        {
            if (string.IsNullOrEmpty(input.Message)) return BadRequest();

            // 2. Sửa lỗi lấy dữ liệu sản phẩm
            // Dùng SanPhams (có 's'), TenSP và SoTien theo đúng DbContext
            var productsData = await _context.SanPhams
                .Select(p => $"{p.TenSP} (Giá: {p.SoTien}đ)")
                .ToListAsync();

            string context = string.Join(", ", productsData);

            // 3. Gửi sang Gemini lấy câu trả lời
            string botReply = await _geminiService.GetChatResponse(input.Message, context);

            // 4. Lưu lịch sử vào Database
            var history = new ChatHistory
            {
                UserMessage = input.Message,
                BotResponse = botReply,
                Timestamp = DateTime.Now
            };

            // CHỖ NÀY QUAN TRỌNG: 
            // Trong AppDbContext bạn đặt là ChatHistory (số ít), nên ở đây phải bỏ 'ies'
            _context.ChatHistory.Add(history);

            await _context.SaveChangesAsync();

            return Json(new { reply = botReply });
        }
    }

    public class ChatInput { public string Message { get; set; } = ""; }
}