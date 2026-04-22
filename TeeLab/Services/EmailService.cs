using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeKit.MimeMessage();
        emailMessage.From.Add(new MailboxAddress("TeeLab Support", "dtc245180174@ictu.edu.vn"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };

        using (var client = new SmtpClient())
        {
            // Kết nối tới Server Gmail
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            // ĐĂNG NHẬP: Đây là chỗ quan trọng nhất
            await client.AuthenticateAsync("dtc245180174@ictu.edu.vn", "tnwu owjz asgj skgn");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}