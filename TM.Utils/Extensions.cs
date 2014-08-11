using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data;
using System.ComponentModel;


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

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable t = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                t.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                t.Rows.Add(values);
            }
            return t;
        }
    }
}
