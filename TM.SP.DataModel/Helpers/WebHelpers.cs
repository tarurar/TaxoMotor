using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace TM.SP.DataModel
{
    public static class WebHelpers
    {
        #region methods

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

        public static bool CheckFeatureActivation(ClientContext context, Guid featureId)
        {
            var featureCollection = context.LoadQuery(context.Web.Features.Include(f => f.DefinitionId));
            context.ExecuteQuery();

            return featureCollection.Any(f => f.DefinitionId.Equals(featureId));
        }

        #endregion
    }
}
