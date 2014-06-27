using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace TM.Utils
{
    public static class Extensions
    {
        public static XElement ToXElement<T>(this object obj)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                using (TextWriter textWriter = new StreamWriter(mStream))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));        
                    serializer.Serialize(textWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(mStream.ToArray()));
                }
            }
        }
    }
}
