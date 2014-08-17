using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using NLog.Targets;

namespace TM.SP.ListModels
{
    public static class Utils
    {
        public static Guid GetWebId(ClientContext context)
        {
            var web = context.Web;
            context.Load(web, w => w.Id);
            context.ExecuteQuery();

            return web.Id;
        }
        public static IEnumerable<List> GetWebLists(ClientContext context)
        {
            var retVal =
                context.LoadQuery(
                    context.Web.Lists.Include(l => l.Id, l => l.RootFolder, l => l.RootFolder.Name,
                        l => l.RootFolder.ContentTypeOrder, l => l.Fields, l => l.ContentTypes));
            context.ExecuteQuery();

            return retVal;
        }

        public static List GetWebList(ClientContext context, string listName)
        {
            var listCollection =
                context.LoadQuery(
                    context.Web.Lists.Where(l => l.RootFolder.Name == listName)
                        .Include(l => l.Id, l => l.RootFolder, l => l.RootFolder.Name,
                            l => l.RootFolder.ContentTypeOrder, l => l.Fields, l => l.ContentTypes));
            context.ExecuteQuery();

            return listCollection.FirstOrDefault();
        }

        public static void MakeContentTypeDefault(ClientContext context, string listName, string contentTypeName)
        {
            List targetList = GetWebList(context, listName);
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

        public static bool CheckFeatureActvation(ClientContext context, Guid featureId)
        {
            var featureCollection = context.LoadQuery(context.Web.Features.Include(f => f.DefinitionId));
            context.ExecuteQuery();

            return featureCollection.Any(f => f.DefinitionId.Equals(featureId));
        }
    }
}
