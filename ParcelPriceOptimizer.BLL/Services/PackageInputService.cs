using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.Entities;
using ParcelPriceOptimizer.DAL.IRepositories;

namespace ParcelPriceOptimizer.BLL.DTO.ViewModels
{
    public class PackageInputService : IPackageInputService
    {
        private readonly ICustomerInputRepository _customerInputRepository;

        public PackageInputService(ICustomerInputRepository customerInputRepository)
        {
            _customerInputRepository = customerInputRepository;
        }

        public async Task<PackageInputViewModel> CreatePackageInputAsync(PackageInputViewModel input)
        {
            var customerInput = new CustomerInput
            {
                //ParcelId = input.ParcelId,
                //Parcel = input.Parcel,
                //UserId = input.UserId,
                CustomerName = input.CustomerName,
                CustomerEmail = input.CustomerEmail,
                SubmittedAt = input.SubmittedAt,
                Width = input.Width,
                Height = input.Height,
                Depth = input.Depth
            };

            await _customerInputRepository.AddAsync(customerInput);

            return input;
        }

        public async Task<IEnumerable<PackageInputViewModel>> GetAllCustomerInputsAsync()
        {
            var inputs = await _customerInputRepository.GetAllAsync();
            return inputs.Select(input => new PackageInputViewModel
            {
                //ParcelId = input.ParcelId,
                //UserId = input.UserId,
                CustomerName = input.CustomerName,
                CustomerEmail = input.CustomerEmail,
                SubmittedAt = input.SubmittedAt,
                Width = input.Width,
                Height = input.Height,
                Depth = input.Depth
            });
        }

        public async Task<PackageInputViewModel> GetCustomerInputByIdAsync(int id)
        {
            var input = await _customerInputRepository.GetByIdAsync(id);
            if (input == null)
                return null;

            return new PackageInputViewModel
            {
                //ParcelId = input.ParcelId,
                //UserId = input.UserId,
                CustomerName = input.CustomerName,
                CustomerEmail = input.CustomerEmail,
                SubmittedAt = input.SubmittedAt,
                Width = input.Width,
                Height = input.Height,
                Depth = input.Depth
            };
        }
    }
}