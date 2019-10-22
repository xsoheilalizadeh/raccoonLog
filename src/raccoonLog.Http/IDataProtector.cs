using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace raccoonLog.Http
{
    public interface IDataProtector
    {
        string Protect(StringValues value, ProtectType protectType);
    }

    public class DataProtector : IDataProtector
    {
        public string Protect(StringValues value, ProtectType protectType)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            if (protectType == ProtectType.Truncate)
            {
                ReadOnlySpan<char> valueSpan = value.ToString();

                return new string(valueSpan.Slice(0, valueSpan.Length / 2));
            }
            else
            {
                using var derivedBytes = new Rfc2898DeriveBytes(value.ToString(),
                    saltSize: 16,
                    iterations: 50000,
                    HashAlgorithmName.SHA256);

                var key = derivedBytes.GetBytes(16);

                return Encoding.UTF8.GetString(key);
            }
        }
    }
}
