using System.Net;
using System.Net.Mail;
using System.Text;
using MaxCo.Models.ViewModels;

namespace MaxCoEmailService
{
    public class ProcessOrder : BackgroundService
    {
        private readonly ILogger<ProcessOrder> _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _emailKey;
        public ProcessOrder(ILogger<ProcessOrder> logger, IConfiguration config, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            _serviceProvider = serviceProvider;

            _emailKey = config["MaxCo:EmailPassKey"];
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

        public Task Email(string htmlString, string email)
        {
            using MailMessage message = new();

            message.From = new MailAddress("meepmopo@gmail.com");
            message.To.Add(email);
            message.Subject = "Order Confirmation - MaxCo";
            message.Body = htmlString;
            message.IsBodyHtml = true;

            using SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new NetworkCredential("meepmopo@gmail.com", _emailKey);
            smtp.EnableSsl = true;
            smtp.Send(message);

            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"{nameof(ProcessOrder)} is running");
                    await Task.Delay(5*1000);

                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        //private async Task DoWorkAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation($"{nameof(ProcessOrder)} is working.");

        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        IProcessOrder scopedProcessingService =
        //            scope.ServiceProvider.GetRequiredService<IProcessOrder>();

        //        await scopedProcessingService.DoWorkAsync(stoppingToken);
        //    }
        //}

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(ProcessOrder)} is stopping");

            await base.StopAsync(stoppingToken);
        }
    }
}
