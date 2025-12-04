using MongoDB_Vault_Demonstration.Models;

namespace MongoDB_Vault_Demonstration.Application.ServiceInterfaces
{
    public interface ICarService
    {
        Task<List<Car>?> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(string id);
        Task CreateCarAsync(string model);
        Task DeleteCarByIdAsync(string id);
    }
}
