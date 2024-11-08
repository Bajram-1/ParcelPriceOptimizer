using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.DAL.IRepositories
{
    public interface IParcelRepository
    {
        Task<IEnumerable<Parcel>> GetAllAsync();
        Task<Parcel> GetByIdAsync(int id);
        Task AddAsync(Parcel parcel);
        Task UpdateAsync(Parcel parcel);
        Task DeleteAsync(int id);
    }
}