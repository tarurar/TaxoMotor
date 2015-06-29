using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using CryptoPro.Sharpei;
using CryptoPro.Sharpei.Xml;

namespace TM.SP.AppPages.VirtualSigner
{
    public class X509Signer: ISigner
    {
        private X509Certificate2 cert;
        public X509Signer(X509Certificate2 certificate)
        {
            this.cert = certificate;
        }

        public string SignXml(string content)
        {
            var document = new XmlDocument { PreserveWhitespace = true };
            document.LoadXml(content);
            var signedContent = new SignedXml(document) { SigningKey = cert.PrivateKey };
            Reference reference = new Reference { Uri = "", DigestMethod = CPSignedXml.XmlDsigGost3411Url };

            // Добавляем transform на подписываемые данные
            // для удаления вложенной подписи.
            var env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            // Добавляем transform для канонизации.
            XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
            reference.AddTransform(c14);
            signedContent.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert));
            signedContent.KeyInfo = keyInfo;
            signedContent.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410Url;
            signedContent.ComputeSignature();
            // Получаем XML представление подписи и сохраняем его 
            // в отдельном node.
            XmlElement xmlDigitalSignature = signedContent.GetXml();
            // Добавляем node подписи в XML документ.
            document.DocumentElement.AppendChild(document.ImportNode(
                xmlDigitalSignature, true));

            return document.InnerXml;
        }
    }
}
