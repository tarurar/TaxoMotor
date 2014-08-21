using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;

namespace TM.SP.DataModel
{
    public static class ListHelpers
    {
        #region methods

        public static void MakeContentTypeDefault(ClientContext context, string listName, string contentTypeName)
        {
            List targetList = WebHelpers.GetWebList(context, listName);
            if (targetList == null)
                throw new Exception(String.Format("List {0} not found", listName));

            List<ContentTypeId> allContentTypes = new List<ContentTypeId>();
            foreach (ContentType ct in targetList.ContentTypes)
            {
                if (ct.Name.Equals(contentTypeName, StringComparison.OrdinalIgnoreCase))
                {
                    allContentTypes.Add(ct.Id);
                }
            }

            if (allContentTypes.Count == 0)
                throw new Exception(String.Format("There is no content type {0} linked to list {1}", contentTypeName,
                    listName));

            targetList.RootFolder.UniqueContentTypeOrder = allContentTypes;
            targetList.RootFolder.Update();
            targetList.Update();
            context.ExecuteQuery();
        }

        public static List GetList(IEnumerable<List> listCollection, string listName, bool breakIfNull = true)
        {
            var retVal = listCollection.SingleOrDefault(l => l.RootFolder.Name == listName);
            if (retVal == null && breakIfNull)
                throw new Exception(String.Format("List {0} not found", listName));

            return retVal;
        }

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

        #endregion
    }
}
