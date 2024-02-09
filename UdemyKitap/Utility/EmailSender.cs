using Microsoft.AspNetCore.Identity.UI.Services;

namespace UdemyKitap.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //email gönderme işlemlerinizi yapın 
            return Task.CompletedTask;
        }
    }
}
