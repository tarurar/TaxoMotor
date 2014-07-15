using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.BusinessData.MetadataModel.Collections;

namespace TM.Utils
{
    public class BCS
    {
        public static string GetLobSystemProperty(ILobSystemInstance lob, string propertyName)
        {
            INamedPropertyDictionary props = lob.GetProperties();

            if (props.ContainsKey(propertyName))
            {
                return props[propertyName].ToString();
            }
            else
            {
                throw new ArgumentException("Can't find LOB system property", propertyName);
            }
        }
    }
}
