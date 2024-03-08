namespace SnipperAPI.interfaces
{
    public interface IEncrypt
    {
        string Decrypt(string content);
        string Encrypt(string content);
    }
}