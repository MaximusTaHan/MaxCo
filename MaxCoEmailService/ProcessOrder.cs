using System.Net;
using System.Net.Mail;
using System.Text;
using MaxCo.Models.ViewModels;

namespace MaxCoEmailService
{
    public class ProcessOrder : IProcessOrder
    {
        private readonly ILogger<ProcessOrder> _logger;
        public ProcessOrder(ILogger<ProcessOrder> logger)
        {
            _logger = logger;
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

            await Email(body, finalOrder.CustomerEmail);
            return;
        }

        public static Task Email(string htmlString, string email)
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
            smtp.Send(message);

            return Task.CompletedTask;
        }
    }
}
