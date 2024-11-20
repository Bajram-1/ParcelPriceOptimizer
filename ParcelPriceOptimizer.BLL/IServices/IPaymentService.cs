using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface IPaymentService
    {
        Task<string> CreateStripeSessionAsync(ParcelPaymentViewModel input, string domain, string userId);
    }
}
