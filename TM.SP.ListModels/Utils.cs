using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;

namespace TM.SP.ListModels
{
    public static class Utils
    {
        public static void AddFieldAsXmlToList(List list, XElement fieldDefinition,
            AddFieldOptions options)
        {
            var listContext = list.Context;

            string fieldName = fieldDefinition.Attribute("Name").Value;
            Field field = null;
            try
            {
                field = list.Fields.SingleOrDefault(f => f.InternalName == fieldName);
            }
            catch (Exception ex)
            {
                if (ex is CollectionNotInitializedException || ex is PropertyOrFieldNotInitializedException)
                {
                    listContext.Load(list.Fields);
                    listContext.ExecuteQuery();
                    field = list.Fields.SingleOrDefault(f => f.InternalName == fieldName);
                }
                else throw;
            }
            
            if (field != null)
            {
                field.DeleteObject();
                listContext.ExecuteQuery();
            }

            list.Fields.AddFieldAsXml(fieldDefinition.ToString(), false, options);
            list.Update();
            listContext.ExecuteQuery();
        }
    }
}
