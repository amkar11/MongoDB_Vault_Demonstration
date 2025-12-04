using MongoDB_Vault_Demonstration.Models;

namespace MongoDB_Vault_Demonstration.Application.RepositoryInterfaces
{
    public interface ICarRepository
    {
        Task<List<Car>?> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(string id);
        Task CreateCarAsync(string model);
        Task<bool> DeleteCarByIdAsync(string id);
    }
}
