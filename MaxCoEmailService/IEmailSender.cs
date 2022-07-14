using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxCo.Models.ViewModels;

namespace MaxCoEmailService
{
    public interface IEmailSender
    {
        Task ConfirmationSender(FinalizedOrder finalOrder);
        Task Email(string email, string subject, string htmlMessage);
    }
}

