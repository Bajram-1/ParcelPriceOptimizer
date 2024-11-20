using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.IRepositories;

namespace ParcelPriceOptimizer.BLL.Services
{
    public class CourierValidationService : ICourierValidationService
    {
        private readonly ICourierPriceRulingRepository _repository;

        public CourierValidationService(ICourierPriceRulingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsValidForAnyCompanyAsync(CustomerInputViewModel input)
        {
            var rules = await _repository.GetPricingRulesAsync();

            return rules.Any(rule =>
                input.Volume >= rule.MinVolume && input.Volume <= rule.MaxVolume &&
                input.Weight >= rule.MinWeight && input.Weight <= rule.MaxWeight);
        }

        public async Task<bool> IsValidForNoCompanyAsync(CustomerInputViewModel input)
        {
            var rules = await _repository.GetPricingRulesAsync();

            return !rules.Any(rule =>
                input.Volume >= rule.MinVolume && input.Volume <= rule.MaxVolume &&
                input.Weight >= rule.MinWeight && input.Weight <= rule.MaxWeight);
        }
    }
}