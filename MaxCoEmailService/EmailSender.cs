using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MaxCo.Models.ViewModels;

namespace MaxCoEmailService
{
    public class EmailSender : IEmailSender
    {

        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task EmailAsync(string email, string subject, string htmlMessage)
        {
            using MailMessage message = new();

            message.From = new MailAddress("meepmopo@gmail.com");
            message.To.Add(email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            using SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new NetworkCredential("meepmopo@gmail.com", _configuration["MaxCo:EmailPassKey"]);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(message);

        }

        public async Task ConfirmationSender(FinalizedOrder finalOrder)
        {
            string body = string.Empty;
            StringBuilder rows = new(100);

            using (StreamReader reader = new(@"C:\Users\Max\Source\Repos\MaxCoFolder\MaxCoEmailService\HtmlTemplate.html"))
            {
                body = reader.ReadToEnd();
            }

            foreach (var item in finalOrder.OrderProducts)
            {
                var price = item.ProductPrice * item.Quantity;
                rows.Append($@"<tr>
                                <td>{item.ProductName}</td>
                                <td>{price}</td>
                                <td>{item.Quantity}</td>
                                </tr>");
            }
            body = body.Replace("{TableRows}", rows.ToString());
            body = body.Replace("{TotalAmount}", finalOrder.TotalPrice.ToString());

            await EmailAsync(finalOrder.CustomerEmail, "test", body);
            return;
        }
    }
}
