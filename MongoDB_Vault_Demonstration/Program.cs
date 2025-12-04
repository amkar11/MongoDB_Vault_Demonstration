
using MongoDB.Driver;
using MongoDB_Vault_Demonstration.Application.RepositoryInterfaces;
using MongoDB_Vault_Demonstration.Application.ServiceInterfaces;
using MongoDB_Vault_Demonstration.Application.Services;
using MongoDB_Vault_Demonstration.Extensions;
using MongoDB_Vault_Demonstration.Infrastructure;

namespace MongoDB_Vault_Demonstration
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            //Logger
            builder.Logging.AddConsole();

            // Add services to the container.
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<ITransitEngineService, TransitEngineService>();

            //MongoDb with Vault
            await builder.Services.AddVaultClient();
            using (var serviceProvider = builder.Services.BuildServiceProvider())
            {
                await builder.Services.AddMongoDbConnectionViaVault(serviceProvider);
            }

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
