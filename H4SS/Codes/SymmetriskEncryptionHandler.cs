using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;

namespace H4SS.Codes
{
    public class SymmetriskEncryptionHandler
    {
        private readonly IDataProtector _key;

        public SymmetriskEncryptionHandler(IDataProtectionProvider key)
        {
            //_key = key.CreateProtector("Nick");
            _key = key.CreateProtector(new RSACryptoServiceProvider().ToXmlString(false));
        }

        public string Encrypt(string valueToEncrypt) =>
            _key.Protect(valueToEncrypt);

        public string Decrypt(string valueToDecrypt) =>
            _key.Unprotect(valueToDecrypt);
    }
}