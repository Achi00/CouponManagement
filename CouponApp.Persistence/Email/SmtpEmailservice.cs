using CouponApp.Application.Interfaces.Sercives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Persistence.Email
{
    public class SmtpEmailservice : IEmailService
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            var email = "eventmanagementsystem82@gmail.com";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(email, "qjktbesaheoozdlp"),
                EnableSsl = true
            };

            var mail = new MailMessage(
                from: email,
                to: to,
                subject: subject,
                body: body
            );

            await client.SendMailAsync(mail);
        }
    }
}
