using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace H4SS_API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class EncryptController : Controller
    {
        [HttpGet("encrypt")]
        public IActionResult Encrypt(string publicKey, string valueToEncrypt)
        {
            if (string.IsNullOrEmpty(valueToEncrypt))
            {
                return BadRequest("Value to encrypt cannot be null or empty.");
            }

            try
            {
                // Decode the public key and value to encrypt
                publicKey = Uri.UnescapeDataString(publicKey);
                valueToEncrypt = Uri.UnescapeDataString(valueToEncrypt);

                Console.WriteLine($"Received Public Key: {publicKey}");
                Console.WriteLine($"Value to Encrypt: {valueToEncrypt}");

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
                Console.Error.WriteLine($"Encryption Error: {ex.Message}");
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

            try
            {
                // Log the raw input values
                Console.WriteLine($"Raw Private Key: {privateKey}");
                Console.WriteLine($"Raw Value to Decrypt: {valueToDecrypt}");

                // Decode the private key and value to decrypt
                privateKey = Uri.UnescapeDataString(privateKey);
                valueToDecrypt = Uri.UnescapeDataString(valueToDecrypt);

                Console.WriteLine($"Decoded Private Key: {privateKey}");
                Console.WriteLine($"Decoded Value to Decrypt: {valueToDecrypt}");

                // Clean the private key by removing PEM headers and line breaks
                privateKey = privateKey
                    .Replace("-----BEGIN PRIVATE KEY-----", "")
                    .Replace("-----END PRIVATE KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Trim();

                // Validate Base64 format
                if (!IsBase64String(privateKey))
                {
                    throw new FormatException("The private key is not a valid Base64 string.");
                }

                if (!IsBase64String(valueToDecrypt))
                {
                    throw new FormatException("The value to decrypt is not a valid Base64 string.");
                }

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

                    rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

                    byte[] valueToDecryptAsBytes = Convert.FromBase64String(valueToDecrypt);
                    byte[] decryptValueBytes = rsa.Decrypt(valueToDecryptAsBytes, true);
                    string decryptedDataAsString = Encoding.UTF8.GetString(decryptValueBytes);

                    return Ok(decryptedDataAsString);
                }
            }
            catch (FormatException ex)
            {
                Console.Error.WriteLine($"Decryption Error: Invalid Base64 input - {ex.Message}");
                return BadRequest("Invalid Base64 input for private key or value to decrypt.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Decryption Error: {ex.Message}");
                return StatusCode(500, $"An error occurred during decryption: {ex.Message}");
            }
        }

        // Helper method to validate Base64 strings
        private bool IsBase64String(string input)
        {
            Span<byte> buffer = new Span<byte>(new byte[input.Length]);
            return Convert.TryFromBase64String(input, buffer, out _);
        }




    }
}