namespace MongoDB_Vault_Demonstration.Application.ServiceInterfaces
{
    public interface ITransitEngineService
    {
        Task<string> EncryptStringAsync(string plain_text);
        Task<string> DecryptStringAsync(string encrypted);
    }
}
