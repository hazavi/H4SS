using System.Security.Cryptography;
using System.Text;

namespace H4SS.Codes
{

    public class HashingHandler
    {
        public dynamic MD5Hashing(dynamic valueToHash) =>
            valueToHash is byte[]? MD5.Create().ComputeHash(valueToHash)
            : Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(valueToHash)));
        public dynamic SHAHashing(dynamic valueToHash) =>
            valueToHash is byte[]? SHA256.Create().ComputeHash(valueToHash)
            : Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(valueToHash)));

        #region Bcrypt

        public string BCryptHashing(string valueToHash) =>
            BCrypt.Net.BCrypt.HashPassword(valueToHash);

        public bool BCryptVerifyHashing(string valueToHash, string hashedValue) =>
            BCrypt.Net.BCrypt.Verify(valueToHash, hashedValue);


        public string BCryptHashing2(string valueToHash) =>
            BCrypt.Net.BCrypt.HashPassword(valueToHash, BCrypt.Net.BCrypt.GenerateSalt(), true, BCrypt.Net.HashType.SHA256);

        public bool BCryptVerifyHashing2(string valueToHash, string hashedValue) =>
            BCrypt.Net.BCrypt.Verify(valueToHash, hashedValue, true, BCrypt.Net.HashType.SHA256);

        #endregion

    }
};
