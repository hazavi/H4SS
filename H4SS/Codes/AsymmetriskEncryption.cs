using System.Security.Cryptography;
using System.Text;

namespace H4SS.Codes
{
    public class AsymmetriskEncryption
    {
        private string _privateKey;
        private string _publicKey;

        public AsymmetriskEncryption()
        {
            if (!Directory.Exists("Keys"))
                Directory.CreateDirectory("Keys");
            if (!File.Exists(@"Keys\privateKey.pem") || !File.Exists(@"Keys\publicKey.pem"))
            {
                using (RSA rsa = RSA.Create(2048))
                {
                    byte[] privateKeyBytes = rsa.ExportRSAPrivateKey();
                    _privateKey = "-----BEGIN PRIVATE KEY-----\n" +
                        Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks) +
                        "\n-----END PRIVATE KEY-----";
                    File.WriteAllText(@"Keys\privateKey.pem", _privateKey);

                    byte[] publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
                    _publicKey = "-----BEGIN PUBLIC KEY-----\n" +
                        Convert.ToBase64String(publicKeyBytes, Base64FormattingOptions.InsertLineBreaks) +
                        "\n-----END PUBLIC KEY-----";
                    File.WriteAllText(@"Keys\publicKey.pem", _publicKey);
                }
            }

            else
            {
                _privateKey = File.ReadAllText(@"Keys\privateKey.pem");
                _publicKey = File.ReadAllText(@"Keys\publicKey.pem");
            }

        }
        public async Task<string> EncryptAsym_webapi(string dataToEncrypt)
        {
            return null;
        }



        public async Task<string> EncryptAsymetrisk(string valueToEncrypt)
        {
            HttpClient client = new();
            var encodedPublicKey = Uri.EscapeDataString(_publicKey);
            var encodedValueToEncrypt = Uri.EscapeDataString(valueToEncrypt);

            var response = await client.GetAsync($"https://localhost:7259/api/encrypt?publicKey={encodedPublicKey}&valueToEncrypt={encodedValueToEncrypt}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                throw new Exception($"Error in encryption: {response.StatusCode} - {errorContent}");
            }
        }


        public async Task<string> DecryptAsymetrisk(string valueToDecrypt)
        {
            HttpClient client = new();
            var encodedPrivateKey = Uri.EscapeDataString(_privateKey);
            var encodedValueToDecrypt = Uri.EscapeDataString(valueToDecrypt);

            var requestUrl = $"https://localhost:7259/api/decrypt?privateKey={encodedPrivateKey}&valueToDecrypt={encodedValueToDecrypt}";
            Console.WriteLine($"Request URL: {requestUrl}");

            var response = await client.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                throw new Exception($"Error in decryption: {response.StatusCode} - {errorContent}");
            }
        }




    }
}