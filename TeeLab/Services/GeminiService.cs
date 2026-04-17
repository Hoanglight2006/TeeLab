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

        KHÁCH HỎI: {userMessage}";

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