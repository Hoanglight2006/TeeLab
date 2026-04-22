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
        Bạn là chuyên gia tư vấn thời trang của TeeLab. 
        DỮ LIỆU CỬA HÀNG: {shopContext}

        YÊU CẦU BẮT BUỘC:
        1. Nếu khách cần tư vấn, hãy hỏi lại khách về nhu cầu từ đó tư vấn chính xác .
        2. KHÔNG liệt kê danh sách sản phẩm dài dòng khi chưa biết rõ nhu cầu khách.
        3. Câu trả lời cực kỳ ngắn gọn (tối đa 3-4 dòng).
        4. Xưng 'TeeLab', gọi khách là 'bạn'. P
        5. Khi khách đã nêu nhu cầu, chỉ gợi ý tối đa 3 mẫu sát nhất kèm giá tiền từ dữ liệu, không trả lời lan man.
        6. Khi khách hàng cần tư vấn cụ thể một vấn đề nào đó, trả lời đúng trọng tâm cho khach hang. 


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