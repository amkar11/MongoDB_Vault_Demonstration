using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB_Vault_Demonstration.Application.RepositoryInterfaces;
using MongoDB_Vault_Demonstration.Application.ServiceInterfaces;
using MongoDB_Vault_Demonstration.Models;

namespace MongoDB_Vault_Demonstration.Infrastructure
{
    public class CarRepository : ICarRepository
    {
        private readonly IMongoCollection<Car> _carsCollection;
        private readonly ITransitEngineService _transitEngineService;

        public CarRepository(IMongoDatabase database, ITransitEngineService transitEngineService)
        {
            _carsCollection = database.GetCollection<Car>("car");
            _transitEngineService = transitEngineService;
        }

        public async Task CreateCarAsync(string model)
        {
           var encrypted_model = await _transitEngineService.EncryptStringAsync(model);
           await _carsCollection.InsertOneAsync(new Car { Model = encrypted_model });
        }

        public async Task<bool> DeleteCarByIdAsync(string id)
        {
           var result = await _carsCollection.DeleteOneAsync(x => x.Id == id);
           return result.DeletedCount > 0;
        }

        public async Task<List<Car>?> GetAllCarsAsync()
        {
            var cars = await _carsCollection.Find(_ => true).ToListAsync();
            List<Car> decrypted_cars = new List<Car>();
            foreach (var car in cars)
            {
                decrypted_cars.Add(new Car { Id = car.Id, Model = await _transitEngineService.DecryptStringAsync(car.Model)});
            }
            return decrypted_cars;
        }

        public async Task<Car?> GetCarByIdAsync(string id)
        {
            var car = await _carsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            var decrypted_car = new Car { Id = car.Id, Model = await _transitEngineService.DecryptStringAsync(car.Model) };
            return decrypted_car;
        }
    }
}
