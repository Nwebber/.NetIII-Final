using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public static class StringHelpers
    {
        public static string SHA256Value(this string source)
        {
            string result = "";

            // Create a byte array - cryptography is byte oriented
            byte[] data;

            // Create a .NET hash provider object
            using (SHA256 sha256hash = SHA256.Create())
            {
                // Hash the source
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            // Now to build the result string
            var s = new StringBuilder();

            // Loop through the byte array
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            // Convert the string builder to a string
            result = s.ToString();

            return result;
        }
    }
}
