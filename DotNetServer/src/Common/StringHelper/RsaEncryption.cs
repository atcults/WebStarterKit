using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.StringHelper
{
    public class RsaEncryption
    {
        private const string SKey = "UJYHCX783her*&5@$%#(MJCX**38n*#6835ncv56tvbry(&#MX98cn342cn4*&X#&";

        public static string Encrypt(string sPainText)
        {
            return sPainText.Length == 0 ? (sPainText) : (EncryptString(sPainText, SKey));
        }

        public static string Decrypt(string sEncryptText)
        {
            return sEncryptText.Length == 0 ? (sEncryptText) : (DecryptString(sEncryptText, SKey));
        }

        protected static string EncryptString(string inputText, string password)
        {
            // "Password" string variable is nothing but the key(your secret key) value which is sent from the front end.
            // "InputText" string variable is the actual password sent from the login page.
            // We are now going to create an instance of the
            // Rihndael class.
            var rijndaelCipher = new RijndaelManaged();
            // First we need to turn the input strings into a byte array.
            var plainText = Encoding.Unicode.GetBytes(inputText);
            // We are using Salt to make it harder to guess our key
            // using a dictionary attack.
            var salt = Encoding.ASCII.GetBytes(password.Length.ToString(CultureInfo.InvariantCulture));
            // The (Secret Key) will be generated from the specified
            // password and Salt.
            //PasswordDeriveBytes -- It Derives a key from a password
            var secretKey = new Rfc2898DeriveBytes(password, salt);
            // Create a encryptor from the existing SecretKey bytes.
            // We use 32 bytes for the secret key
            // (the default Rijndael key length is 256 bit = 32 bytes) and
            // then 16 bytes for the IV (initialization vector),
            // (the default Rijndael IV length is 128 bit = 16 bytes)
            var encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(16), secretKey.GetBytes(16));
            // Create a MemoryStream that is going to hold the encrypted bytes
            var memoryStream = new MemoryStream();
            // Create a CryptoStream through which we are going to be processing our data.
            // CryptoStreamMode.Write means that we are going to be writing data
            // to the stream and the output will be written in the MemoryStream
            // we have provided. (always use write mode for encryption)
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            // Start the encryption process.
            cryptoStream.Write(plainText, 0, plainText.Length);
            // Finish encrypting.
            cryptoStream.FlushFinalBlock();
            // Convert our encrypted data from a memoryStream into a byte array.
            var cipherBytes = memoryStream.ToArray();
            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();
            // Convert encrypted data into a base64-encoded string.
            // A common mistake would be to use an Encoding class for that.
            // It does not work, because not all byte values can be
            // represented by characters. We are going to be using Base64 encoding
            // That is designed exactly for what we are trying to do.
            var encryptedData = Convert.ToBase64String(cipherBytes);
            // Return encrypted string.
            return encryptedData;
        }

        protected static string DecryptString(string inputText, string password)
        {
            try
            {
                var rijndaelCipher = new RijndaelManaged();
                var encryptedData = Convert.FromBase64String(inputText);
                var salt = Encoding.ASCII.GetBytes(password.Length.ToString(CultureInfo.InvariantCulture));
                var secretKey = new Rfc2898DeriveBytes(password, salt);
                // Create a decryptor from the existing SecretKey bytes.
                var decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(16), secretKey.GetBytes(16));
                var memoryStream = new MemoryStream(encryptedData);
                // Create a CryptoStream. (always use Read mode for decryption).
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold EncryptedData;
                // DecryptedData is never longer than EncryptedData.
                var plainText = new byte[encryptedData.Length];
                // Start decrypting.
                var decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                memoryStream.Close();
                cryptoStream.Close();
                // Convert decrypted data into a string.
                var decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                // Return decrypted string.
                return decryptedData;
            }
            catch (Exception exception)
            {
                return (exception.Message);
            }
        }
    }
}