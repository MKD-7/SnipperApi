using SnipperAPI.interfaces;
using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;

public class EncryptionService : IEncrypt
{
	public EncryptionService()
	{ }

    public string Decrypt(string content)
    {
        throw new NotImplementedException();
    }

    public string Encrypt(string content)
    {
        using (SHA256 mySHA256 = SHA256.Create())
        {
            byte[] hashValue = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(content));
            return string.Empty;
        }
        
    }
}
