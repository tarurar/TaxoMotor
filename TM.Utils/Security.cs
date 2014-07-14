using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TM.Utils
{
    public class Security
    {
        public static string GetMd5Hash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                foreach (byte t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString();
            }
        }
    }
}
