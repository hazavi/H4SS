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
            if (!File.Exists(@"Keys\privateKey.pem,") || !File.Exists(@"Keys\publicKey.pem"))
            {
                using (RSA rsa = RSA.Create(2048))
                {
                    byte[] privateKeyBytes = rsa.ExportRSAPrivateKey();
                    _privateKey = "-----BEGIN PRIVATE KEY-----\n" +
                        Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks) +
                        "\n-----END PRIVATE KEY-----";

                    byte[] publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
                    _publicKey = "-----BEGIN PUBLIC KEY-----\n" +
                        Convert.ToBase64String(publicKeyBytes, Base64FormattingOptions.InsertLineBreaks) +
                        "\n-----END PUBLIC KEY-----";

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
            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            //{
            //    string publicKey = _publicKey
            //        .Replace("-----BEGIN PUBLIC KEY-----", "")
            //        .Replace("-----END PUBLIC KEY-----", "")
            //        .Replace("\n", "").Replace("\r", "").Trim();

            //    byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

            //    rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

            //    byte[] valueToEncryptAsBytes = Encoding.UTF8.GetBytes(valueToEncrypt);
            //    byte[] encryptValueBytes = rsa.Encrypt(valueToEncryptAsBytes, true);
            //    return Convert.ToBase64String(encryptValueBytes);
            //}
            HttpClient client = new();
            var value = await client.GetAsync($"https://localhost:7024/encrypt?publicKey={_publicKey}&valueToEncrypt={valueToEncrypt}");
            if (value.IsSuccessStatusCode)
            {
                var result = await value.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new Exception("Error in encryption");
            }

            //string result = call web api(publickey, valueToEncrypt)
        }

        public async Task<string> DecryptAsymetrisk(string valueToDecrypt)
        {
            HttpClient client = new();
            var value = await client.GetAsync($"https://localhost:7024/decrypt?privateKey={_privateKey}&valueToDecrypt={valueToDecrypt}");
            if (value.IsSuccessStatusCode)
            {
                var result = await value.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new Exception("Error in encryption");
            }
        }
    }
}