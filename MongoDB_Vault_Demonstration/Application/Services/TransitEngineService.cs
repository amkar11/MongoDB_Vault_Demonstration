using MongoDB_Vault_Demonstration.Application.ServiceInterfaces;
using VaultSharp;
using VaultSharp.V1.SecretsEngines.Transit;
using EncryptRequestOptions = VaultSharp.V1.SecretsEngines.Transit.EncryptRequestOptions;

namespace MongoDB_Vault_Demonstration.Application.Services
{
    public class TransitEngineService : ITransitEngineService
    {
        private readonly IVaultClient _vaultClient;
        private readonly ILogger<TransitEngineService> _logger;

        public TransitEngineService(IServiceProvider serviceProvider, ILogger<TransitEngineService> logger)
        {
            _vaultClient = serviceProvider.GetRequiredService<IVaultClient>();
            _logger = logger;
        }

        public async Task<string> DecryptStringAsync(string encrypted)
        {
            var decryptRequestOptions = new DecryptRequestOptions { CipherText = encrypted };
            var decryptResponse = await _vaultClient.V1.Secrets.Transit.DecryptAsync(
                keyName: "api-key",
                decryptRequestOptions: decryptRequestOptions,
                mountPoint: "transit");
            string base64Decrypted = decryptResponse.Data.Base64EncodedPlainText;
            string decrypted = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Decrypted));
            _logger.LogInformation($"Decrypted string: {decrypted}");
            return decrypted;
        }

        public async Task<string> EncryptStringAsync(string plain_text)
        {
            string base64PlainText = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plain_text));
            var encrpytRequestOptions = new EncryptRequestOptions { Base64EncodedPlainText = base64PlainText};
            var encryptResponse = await _vaultClient.V1.Secrets.Transit.EncryptAsync(
                keyName: "api-key",
                encryptRequestOptions: encrpytRequestOptions,
                mountPoint: "transit");
            string encrypted = encryptResponse.Data.CipherText;
            _logger.LogInformation($"Encrypted string: {encrypted}");
            return encrypted;
        }
    }
}
