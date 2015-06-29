using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace TM.SP.AppPages.VirtualSigner
{
    public static class CertificateHelper
    {
        public static X509Certificate2 GetCryptoProCertificate(string thumbprint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            var found = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            if (found.Count == 0)
            {
                throw new Exception("Сертификат не найден");
                
            }
            if (found.Count > 1)
            {
                throw new Exception("Найдено больше одного сертификата");
            }

            return found[0];
        }
    }
}
