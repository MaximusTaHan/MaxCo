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
    public class DefaultProcessOrder : IProcessOrder
    {
        private readonly ILogger<DefaultProcessOrder> _logger;

        public DefaultProcessOrder(ILogger<DefaultProcessOrder> logger)
        {
            _logger = logger;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken, FinalizedOrder finalizedOrder)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if(finalizedOrder == null)
                {
                    await Task.Delay(5*1000, cancellationToken);
                    continue;
                }

                string body = string.Empty;
                StringBuilder rows = new(100);

                using (StreamReader reader = new(@"C:\Users\Max\Source\Repos\MaxCoFolder\MaxCoEmailService\HtmlTemplate.html"))
                {
                    body = reader.ReadToEnd();
                }

                foreach (var item in finalizedOrder.OrderProducts)
                {
                    var price = item.ProductPrice * item.Quantity;
                    rows.Append($@"<tr>
                                <td>{item.ProductName}</td>
                                <td>{price}</td>
                                <td>{item.Quantity}</td>
                                </tr>");
                }
                body = body.Replace("{TableRows}", rows.ToString());
                body = body.Replace("{TotalAmount}", finalizedOrder.TotalPrice.ToString());

                await Email(body, finalizedOrder.CustomerEmail);

                _logger.LogInformation("{ServiceName} working, email has been sent", nameof(ProcessOrder));

                await Task.Delay(5 * 1000, cancellationToken);
            }
        }

        public async Task Email(string htmlString, string email)
        {
            using MailMessage message = new();

            message.From = new MailAddress("meepmopo@gmail.com");
            message.To.Add(email);
            message.Subject = "Order Confirmation - MaxCo";
            message.Body = htmlString;
            message.IsBodyHtml = true;

            using SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new NetworkCredential("meepmopo@gmail.com", "euptcokvqggrwkqt");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(message);
        }
    }
}
