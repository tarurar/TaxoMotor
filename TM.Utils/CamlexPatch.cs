using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TM.Utils
{
    public abstract class CamlexPatch
    {
        public static readonly string StorageTZAttr = "StorageTZ";
        public static readonly string TypeAttr = "Type";
        public static readonly string ValueTag = "Value";

        public static string PatchTimeZone(string camlQuery)
        {
            if (String.IsNullOrEmpty(camlQuery))
                throw new ArgumentNullException();

            XDocument camlDoc = XDocument.Parse(camlQuery);

            var dates = from el in camlDoc.Descendants(ValueTag)
                        where el.Attribute(TypeAttr).Value == "DateTime"
                        select el;

            foreach (var date in dates)
            {
                var exAttr = date.Attribute(StorageTZAttr);
                var value = true.ToString().ToUpper();

                if (exAttr != null)
                {
                    exAttr.SetValue(value);
                }
                else 
                {
                    date.Add(new XAttribute(StorageTZAttr, value));
                }
            }

            return camlDoc.ToString();
        }
    }
}
