using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.Entities;
using ParcelPriceOptimizer.DAL.IRepositories;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ICustomerInputRepository _customerInputRepository;

        public PaymentService(ICustomerInputRepository customerInputRepository)
        {
            _customerInputRepository = customerInputRepository;
        }
        public async Task<string> CreateStripeSessionAsync(ParcelPaymentViewModel input, string domain, string userId)
        { 
            var customerInput = new CustomerInput 
            { 
                Width = input.Width, 
                Height = input.Height, 
                Depth = input.Depth, 
                Weight = input.Weight, 
                Price = input.Price, 
                SubmittedAt = DateTime.Now,
                UserId = userId,
                IsPaymentCompleted = false
            }; 
            await _customerInputRepository.AddAsync(customerInput); 
            var customerInputId = customerInput.Id;
            var options = new SessionCreateOptions 
            { 
                SuccessUrl = $"{domain}payment-success", 
                CancelUrl = $"{domain}payment-cancelled", 
                LineItems = new List<SessionLineItemOptions>(), 
                Mode = "payment", 
            }; 
            var sessionLineItem = new SessionLineItemOptions 
            { 
                PriceData = new SessionLineItemPriceDataOptions 
                { 
                    UnitAmount = (long)(input.Price * 100), 
                    Currency = "usd", 
                    ProductData = new SessionLineItemPriceDataProductDataOptions 
                    { 
                        Name = "Parcel Payment" 
                    } 
                }, 
                Quantity = 1 
            }; 
            
            options.LineItems.Add(sessionLineItem); 
            var service = new SessionService(); 
            Session session = await service.CreateAsync(options); 
            await UpdateStripePaymentID(customerInputId, session.Id, session.PaymentIntentId, false); 
            return session.Url; 
        }

        public async Task UpdateStripePaymentID(int customerInputId, string sessionId, string paymentIntentId, bool isPaymentSuccessful) 
        { 
            var customerInput = await _customerInputRepository.GetByIdAsync(customerInputId); 
            
            if (customerInput == null) 
            { 
                throw new Exception("Customer Input not found."); 
            } 
            
            customerInput.StripeSessionId = sessionId; 
            customerInput.StripePaymentIntentId = paymentIntentId;
            customerInput.IsPaymentCompleted = isPaymentSuccessful;
            await _customerInputRepository.UpdateAsync(customerInput); 
        }
    }
}