using MongoDB.Driver;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.SecretsEngines.PKI;

namespace MongoDB_Vault_Demonstration.Extensions
{
    public static class MongoDbVault
    {
        public static async Task<IServiceCollection> AddMongoDbConnectionViaVault(this IServiceCollection services,
            IServiceProvider provider)
        {

            var vaultClient = provider.GetService<IVaultClient>();

            var secret = await vaultClient!.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                path: "db-config",
                mountPoint: "kv"
            );

            var username = secret.Data.Data["username"].ToString();
            var password = secret.Data.Data["password"].ToString();

            var mongoClient = new MongoClient($"mongodb://{username}:{password}@mongo:27017/");
            var database = mongoClient.GetDatabase("CarsDb");
            services.AddSingleton<IMongoDatabase>(database);

            return services;
        }
    }
}
