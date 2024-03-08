namespace SnipperAPI.interfaces
{
    public interface IEncrypt
    {
        string Decrypt(byte[] content);
        byte[] Encrypt(string content);
    }
}