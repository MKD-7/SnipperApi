using SnipperAPI.interfaces;
using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;

public class EncryptionService : IEncrypt
{
    private readonly byte[] _key = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
    private readonly byte[] _IV = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
    //these two will be removed once we a re persisiting the key and iv along with the encrypted data. there is an issue where the iv and key are bring reqreshed between requests which means that the decryption messgae doesnt work.
    public EncryptionService()
	{
    }

    public string Decrypt(byte[] content)
    {
        //to do - fetch byte[] value form database 
        var decrypted = Decrypt(content, _key, _IV);
        return decrypted;
        
    }

    public byte[] Encrypt(string content)
    {
        var encrypted = Encrypt(content, _key, _IV);
        //to do - save byte[] in database
        return encrypted;
        
    }
    private string Decrypt(byte[] content, byte[] Key, byte[] IV)
    {
        if (content == null || content.Length <= 0)
            throw new ArgumentNullException("cipherText");

        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(content))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }

    private byte[] Encrypt(string content, byte[] Key, byte[] IV)
    {
        if (content == null || content.Length <= 0)
            throw new ArgumentNullException("content");
        byte[] encrypted;
        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(content);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        // Return the encrypted bytes from the memory stream.
        return encrypted;

    }
}
