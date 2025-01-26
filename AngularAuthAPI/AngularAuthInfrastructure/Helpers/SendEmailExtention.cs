using System.Net;
using System.Net.Mail;

namespace AngularAuthInfrastructure.Helpers;


//GMAIL STMP
//ارسال ایمیل با استفاده از سرور اس. جیمیل

public static class SendEmailExtention
{
    public async static Task<bool> SendAsync(string to, string subject, string body)
    {
        //قبل از استفاده بایستی این پارامتر ها رو از سرور اس. دریافت کنید
        string senderEmail = "";
        string password = "";

        MailMessage message = new MailMessage(senderEmail, to)
        {
            Body = body,
            Subject = subject,
            IsBodyHtml = true
        };

        NetworkCredential mailAuthentication = new NetworkCredential(senderEmail, password);
        SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = mailAuthentication
        };

        try
        {
            await mailClient.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
    }
}
