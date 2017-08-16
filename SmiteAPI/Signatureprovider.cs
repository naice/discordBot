using System;
using System.Collections.Generic;
using System.Text;

namespace SmiteAPI
{
    public class SignatureProvider
    {
        private readonly string _devId, _authKey;
        private string _timestamp;

        public SignatureProvider(string devId, string authKey)
        {
            _devId = devId;
            _authKey = authKey;
        }

        public void UpdateTimestamp(string timestamp)
        {
            _timestamp = timestamp;
        }

        public string GetSignature(string methodName)
        {
            return GetMD5Hash(_devId + methodName + _authKey + _timestamp);
        }

        private static string GetMD5Hash(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            var sb = new System.Text.StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

    }
}
