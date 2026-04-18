using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace TeeLab.Services
{
    public class GeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GeminiService(IConfiguration configuration)
        {
            _apiKey = configuration["Gemini:ApiKey"] ?? throw new ArgumentNullException("ApiKey missing");
            _httpClient = new HttpClient();
        }

        public async Task<string> GetChatResponse(string userMessage, string shopContext)
        {
            // GIỮ NGUYÊN: Model chuẩn theo tài khoản của Mạnh Hà
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3-flash-preview:generateContent?key={_apiKey}";

            // NÂNG CẤP: Chỉ sửa phần nội dung nhắc nhở để Bot thông minh hơn
            var prompt = $@"
# IDENTITY
Bạn là Chuyên gia Tư vấn Thời trang (AI Stylist) cấp cao tại TeeLab.
Phong cách: Tinh tế, am hiểu thời trang Streetwear, phản hồi cực kỳ súc tích nhưng đầy đủ thông tin hữu ích.

# KNOWLEDGE BASE
Dữ liệu sản phẩm hiện có: {shopContext}

# WORKFLOW & LOGIC (QUY TRÌNH LÀM VIỆC)
Bước 1: khi khách hàng nói lời xin chào: ""Chào anh/chị. Cám ơn bạn đã quan tâm TeeLab. Anh/chị có cần bên em tư vấn gì không ạ?""

Bước 2: Phân tích Ý định của khách (User Intent):
- NẾU HỎI SIZE: Tuyệt đối không đưa ra size ngay. Hãy hỏi: ""Để TeeLab tư vấn size chính xác nhất, bạn cho mình xin Chiều cao (cm), Cân nặng (kg) và mẫu bạn đang nhắm tới nhé!""
- NẾU HỎI GỢI Ý/PHỐI ĐỒ: Hãy hỏi 2 câu: ""Bạn dự định mặc đi đâu (đi học, dạo phố, party)?"" và ""Bạn thích form rộng thoải mái hay vừa vặn?""
- NẾU HỎI VỀ CHẤT LIỆU/GIÁ: Trả lời thẳng thắn, chính xác dựa trên DỮ LIỆU CỬA HÀNG.

Bước 3: Đưa ra đề xuất (Khi đã đủ thông tin):
- Chỉ gợi ý tối đa 2 mẫu sát nhất. Mỗi mẫu trình bày theo dạng: [Tên Sản Phẩm] - [Giá].
- Kèm 1 câu nhận xét chuyên môn: ""Mẫu này phối với quần Jean cực cháy cho các buổi dạo phố.""

# STRICT RULES (QUY TẮC CỨNG)
1. Tuyệt đối KHÔNG trả lời lan man, không giới thiệu thừa thãi về công ty.
2. Nếu khách hỏi sản phẩm KHÔNG có trong dữ liệu: Trả lời: ""Tiếc quá, mẫu này TeeLab hiện hết hàng rồi ạ. Bạn tham khảo mẫu [Tên Sản Phẩm Tương Tự] này cũng cực chất nhé!""
3. Giới hạn phản hồi: Tối đa 3-4 dòng văn bản (không tính danh sách sản phẩm).
4. Sử dụng icon chuyên nghiệp: ✨, 👕, 🔥, 📍.

# USER MESSAGE
{userMessage}";
            var requestBody = new
            {
                contents = new[]
                {
            new {
                role = "user",
                parts = new[] {
                    new { text = prompt }
                }
            }
        },
                generationConfig = new
                {
                    temperature = 0.5, // Giảm xuống 0.5 để bot trả lời súc tích và chính xác hơn
                    maxOutputTokens = 800,
                    topP = 0.95
                }
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(requestBody, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return $"Lỗi API: {responseString}";

                using var doc = JsonDocument.Parse(responseString);
                var reply = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return reply ?? "TeeLab đang bận một chút, bạn đợi tí nhé!";
            }
            catch (Exception ex) { return "Lỗi kết nối: " + ex.Message; }
        }
    }
}