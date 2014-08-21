using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TM.Utils
{
    public static class Reflection
    {
        public static T GetPropertyValue<T>(object sourceObject, string propertyName)
        {
            PropertyInfo property = sourceObject.GetType().GetProperty(propertyName);
            if (property == null)
                throw new Exception(String.Format("There are was an error while trying to get object property named {0}", propertyName));

            return (T)property.GetValue(sourceObject);
        }
    }
}
