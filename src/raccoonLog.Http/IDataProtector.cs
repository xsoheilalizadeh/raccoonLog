using Microsoft.Extensions.Primitives;
using System;
using System.Security.Cryptography;
using System.Text;

namespace raccoonLog.Http
{
    public interface IDataProtector
    {
        string Protect(string value);
    }

    public class DataProtector : IDataProtector
    {
        public string Protect(string value) => "protected-data";
    }
}
