using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CompleteTakeover.Repository.Helpers
{
    public static class Hasher
    {
        public static byte[] Hash(string s)
        {
            var provider = new SHA256CryptoServiceProvider();
            var bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(s));
            return bytes;
        }
    }
}
