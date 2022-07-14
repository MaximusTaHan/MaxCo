using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxCo.Models.ViewModels;

// Implementation for a hosted service to be consumed in a scope
//  --------------------Currently Not Used----------------------
namespace MaxCoEmailService
{
    public interface IProcessOrder
    {
        Task DoWorkAsync(CancellationToken cancellationToken, FinalizedOrder finalizedOrder);
    }
}
