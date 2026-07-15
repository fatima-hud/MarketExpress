namespace MarketExpress.Application.Services.AesEncryption
{
    public interface IAesEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);

    }
}
