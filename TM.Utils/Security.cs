using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.BusinessData.Infrastructure.SecureStore;
using Microsoft.Office.SecureStoreService.Server;

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

        public static string SecureStringToString(SecureString str)
        {
            IntPtr p = Marshal.SecureStringToBSTR(str);
            try
            {
                return Marshal.PtrToStringBSTR(p);
            }
            finally
            {
                Marshal.FreeBSTR(p);
            }

        }

        public static string GetSecureStoreCredentials(string applicationId, SecureStoreCredentialType credentialType)
        {
            var provider = SecureStoreProviderFactory.Create();
            using (var creds = provider.GetCredentials(applicationId))
            using (var cred = creds.FirstOrDefault(item => item.CredentialType == credentialType))
            {
                return cred != null ? SecureStringToString(cred.Credential) : String.Empty;
            }
        }

        public static string GetSecureStoreUserNameCredential(string applicationId)
        {
            var value = GetSecureStoreCredentials(applicationId, SecureStoreCredentialType.UserName);
            if (String.IsNullOrEmpty(value))
            {
                value = GetSecureStoreCredentials(applicationId, SecureStoreCredentialType.WindowsUserName);
            }

            if (String.IsNullOrEmpty(value))
                throw new Exception("Secure store user name credential value is empty");

            return value;
        }

        public static string GetSecureStorePasswordCredential(string applicationId)
        {
            var value = GetSecureStoreCredentials(applicationId, SecureStoreCredentialType.Password);
            if (String.IsNullOrEmpty(value))
            {
                value = GetSecureStoreCredentials(applicationId, SecureStoreCredentialType.WindowsPassword);
            }
            
            return value;
        }
    }
}
