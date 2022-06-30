using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MaxCoEmailService
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using MailMessage message = new();

            message.From = new MailAddress("meepmopo@gmail.com");
            message.To.Add(email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = false;

            using SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new NetworkCredential("meepmopo@gmail.com", "euptcokvqggrwkqt");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(message);
        }
    }
}
