using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxCo.Models.ViewModels;

namespace MaxCoEmailService
{
    public interface IProcessOrder
    {
        Task ConfirmationSender(FinalizedOrder finalOrder);
    }
}
