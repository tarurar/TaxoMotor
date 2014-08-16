using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System.Net;

namespace TestBCSProvision
{
    class Program
    {
        /*private string CreateBCSLookupField(SPWeb targetWeb,
            string lookupFieldName, string groupName,
            string systemInstanceName, string entityNamespace,
            string entityName, string entityFieldName, bool hasActions)
                    {
                        SPBusinessDataField lookupField =
                        targetWeb.Fields.CreateNewField("BusinessData", lookupFieldName) as SPBusinessDataField;
                        lookupField.Group = groupName;
                        lookupField.SystemInstanceName = systemInstanceName;
                        lookupField.EntityNamespace = entityNamespace;
                        lookupField.EntityName = entityName;
                        lookupField.HasActions = hasActions;
                        lookupField.BdcFieldName = entityFieldName;
                        return targetWeb.Fields.Add(lookupField);
                    }*/
        static void Main(string[] args)
        {
            using (var ctx = new ClientContext("http://rgs1.dev.armd.ru"))
            {
                ctx.ExecutingWebRequest +=
                    (sender, e) => e.WebRequestExecutor.RequestHeaders.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                ctx.Credentials = new NetworkCredential("developer", "111222", "spdev");

                var lists = ctx.LoadQuery(ctx.Web.Lists.Where(l => l.RootFolder.Name == "IncomeRequestList"));
                ctx.ExecuteQuery();

                List list = lists.First();

                list.Fields.AddFieldAsXml(
                    "<Field Type=\"BusinessData\" DisplayName=\"serviceheader\" Required=\"FALSE\" ID=\"{2BA64558-25BB-4FD2-AAFE-5B5E841699B3}\" StaticName=\"serviceheader\" Name=\"serviceheader\" SystemInstance=\"CoordinateV5\" EntityNamespace=\"TM.SP.BCSModels.CoordinateV5\" EntityName=\"ServiceHeader\" BdcField=\"Title\" Version=\"1\" />",
                    true, AddFieldOptions.AddToAllContentTypes);
                list.Update();
                ctx.ExecuteQuery();

            }
        }
    }
}
