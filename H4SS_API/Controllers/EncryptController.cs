using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace H4SS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncryptController : Controller
    {
        [HttpGet("encrypt")]
        public IActionResult Encrypt(string publicKey, string valueToEncrypt)
        {
            if (string.IsNullOrEmpty(valueToEncrypt))
            {
                return BadRequest("Value to encrypt cannot be null or empty.");
            }
            publicKey = publicKey.Replace(" ", "+");
            valueToEncrypt = valueToEncrypt.Replace(" ", "+");

            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    publicKey = publicKey
                        .Replace("-----BEGIN PUBLIC KEY-----", "")
                        .Replace("-----END PUBLIC KEY-----", "")
                        .Replace("\n", "").Replace("\r", "").Trim();

                    byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

                    rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

                    byte[] valueToEncryptAsBytes = Encoding.UTF8.GetBytes(valueToEncrypt);
                    byte[] encryptValueBytes = rsa.Encrypt(valueToEncryptAsBytes, true);
                    string encryptedValue = Convert.ToBase64String(encryptValueBytes);

                    return Ok(encryptedValue);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during encryption: {ex.Message}");
            }
        }

        [HttpGet("decrypt")]
        public IActionResult Decrypt(string privateKey, string valueToDecrypt)
        {
            if (string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(valueToDecrypt))
            {
                return BadRequest("Private key and value to decrypt cannot be null or empty.");
            }

            privateKey = privateKey.Replace(" ", "+");
            valueToDecrypt = valueToDecrypt.Replace(" ", "+");

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                privateKey = privateKey
                    .Replace("-----BEGIN PRIVATE KEY-----", "")
                    .Replace("-----END PRIVATE KEY-----", "")
                    .Replace("\n", "").Replace("\r", "").Trim();

                byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

                byte[] valueToDecryptAsBytes = Convert.FromBase64String(valueToDecrypt);
                byte[] decryptValueBytes = rsa.Decrypt(valueToDecryptAsBytes, true);
                string decryptedDataAsString = System.Text.Encoding.UTF8.GetString(decryptValueBytes);

                return Ok(decryptedDataAsString);
            }
        }
    }
}
