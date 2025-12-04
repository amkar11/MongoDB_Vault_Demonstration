using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.SecretsEngines.Database;

namespace MongoDB_Vault_Demonstration.Extensions
{
    public static class Vault
    {
        private static string _secretPath = "/run/secrets/role-id-secret";
        private static string _file = string.Empty;
        private static string _roleId = string.Empty;
        private static string _secretId = string.Empty;
        private static ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole())
            .CreateLogger(string.Empty);

        public static async Task<IServiceCollection> AddVaultClient(this IServiceCollection services)
        {
            if (File.Exists(_secretPath))
            {
                string file = await File.ReadAllTextAsync(_secretPath);
                string[] id_and_secret = file.Split('/');
                _roleId = id_and_secret[0];
                _secretId = id_and_secret[1];
                _logger.LogInformation($"Role id {_roleId} and {_secretId} are read correctly");
            }
            else
            {
                throw new InvalidOperationException($"Contents of file: {_file}");
            }

            IAuthMethodInfo authMethod = new AppRoleAuthMethodInfo(_roleId, _secretId);

            var vaultClientSettings = new VaultClientSettings("https://vault-tls:8300", authMethod);

            IVaultClient vaultClient = new VaultClient(vaultClientSettings);

            services.AddSingleton<IVaultClient>(vaultClient);

            return services;
        }
    }
}
