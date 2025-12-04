using MongoDB.Driver;
using MongoDB_Vault_Demonstration.Application.RepositoryInterfaces;
using MongoDB_Vault_Demonstration.Application.ServiceInterfaces;
using MongoDB_Vault_Demonstration.Models;

namespace MongoDB_Vault_Demonstration.Application.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public Task CreateCarAsync(string model)
        {
           return _carRepository.CreateCarAsync(model);
        }

        public async Task DeleteCarByIdAsync(string id)
        {
            var deleted = await _carRepository.DeleteCarByIdAsync(id);
            if (!deleted)
            {
                throw new InvalidOperationException($"There is no car with such id {id} in database");
            }
        }

        public Task<List<Car>?> GetAllCarsAsync()
        {
            return _carRepository.GetAllCarsAsync();
        }

        public async Task<Car> GetCarByIdAsync(string id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
            {
                throw new InvalidOperationException($"There is no car with such id {id} in database");
            }
            return car;
        }
    }
}
