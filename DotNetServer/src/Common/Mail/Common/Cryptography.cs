using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Common.Mail.Common
{
    /// <summary>
    /// 
    /// </summary>
    internal class Cryptography
    {
        /// <summary>
        /// Follow MD5 digest to convert a string. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToMd5DigestString(String text)
        {
            var sb = new StringBuilder(64);

            var bb = Encoding.UTF8.GetBytes(text);
            MD5 md5 = new MD5CryptoServiceProvider();
            bb = md5.ComputeHash(bb);
            for (var i = 0; i < bb.Length; i++)
            {
                sb.Append(bb[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        /// <summary>
        /// Cram-MD 5 strings in the conversion. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String ToCramMd5String(String text, String key)
        {
            var sb = new StringBuilder(128);
            var md5 = new HMACMD5(Encoding.UTF8.GetBytes(key));
            // 64 decoding the Base challenge code and password for the key HMAC-MD 5 calculates the hash value. 
            var bb = md5.ComputeHash(Convert.FromBase64String(text));
            // Calculated HMAC-MD5 hash value of the byte hexadecimal notation to convert to a string.

            for (var i = 0; i < bb.Length; i++)
            {
                sb.Append(bb[i].ToString("x02"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generates a cryptographic signature for a given message
        /// </summary>
        /// <param name="message">The message to sign</param>
        /// <param name="signingCertificate">The certificate to sign the message with</param>
        /// <param name="encryptionCertificate">An optional encryption certificate to include along with the signature</param>
        /// <returns>The signature for the specified message</returns>
        internal static byte[] GetSignature(string message, X509Certificate2 signingCertificate, X509Certificate2 encryptionCertificate)
        {
            var messageBytes = Encoding.ASCII.GetBytes(message);

            var signedCms = new SignedCms(new ContentInfo(messageBytes), true);

            var cmsSigner = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, signingCertificate)
                {
                    IncludeOption = X509IncludeOption.WholeChain
                };

            if (encryptionCertificate != null)
            {
                cmsSigner.Certificates.Add(encryptionCertificate);
            }

            var signingTime = new Pkcs9SigningTime();
            cmsSigner.SignedAttributes.Add(signingTime);

            signedCms.ComputeSignature(cmsSigner, false);

            return signedCms.Encode();
        }

        /// <summary>
        /// Encrypts a message
        /// </summary>
        /// <param name="message">The message to encrypt</param>
        /// <param name="encryptionCertificates">A list of certificates to encrypt the message with</param>
        /// <returns>The encrypted message</returns>
        internal static byte[] EncryptMessage(string message, X509Certificate2Collection encryptionCertificates)
        {
            var messageBytes = Encoding.ASCII.GetBytes(message);

            var envelopedCms = new EnvelopedCms(new ContentInfo(messageBytes));

            var recipients = new CmsRecipientCollection(SubjectIdentifierType.IssuerAndSerialNumber, encryptionCertificates);

            envelopedCms.Encrypt(recipients);

            return envelopedCms.Encode();
        }
    }
}
