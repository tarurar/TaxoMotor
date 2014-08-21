using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.SharePoint;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.BusinessData.MetadataModel.Collections;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData.SharedService;
using Microsoft.SharePoint.BusinessData.MetadataModel;
using Microsoft.SharePoint.BusinessData.Infrastructure;

namespace TM.Utils
{
    public class BCS
    {
        public static readonly string LOBRequestSystemName = "CoordinateV5";
        public static readonly string LOBRequestSystemNamespace = "TM.SP.BCSModels.CoordinateV5";
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

        public static IEntity GetEntity(SPServiceContext context, string bdcServiceName, string @namespace, string name)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            BdcService svcInstance = String.IsNullOrEmpty(bdcServiceName) ? SPFarm.Local.Services.GetValue<BdcService>() : SPFarm.Local.Services.GetValue<BdcService>(bdcServiceName);
            if (svcInstance == null)
                throw new Exception("No BDC Service Application found");

            DatabaseBackedMetadataCatalog catalog = svcInstance.GetDatabaseBackedMetadataCatalog(context);
            return catalog.GetEntity(@namespace, name);
        }

        public static object GetDataFromMethod(string lobSystemName, IEntity externalCT, string methodName, MethodInstanceType methodType, ref object[] args)
        {
            var lob     = externalCT.GetLobSystem();
            var lobi    = lob.GetLobSystemInstances()[lobSystemName];
            var mi      = externalCT.GetMethodInstance(methodName, methodType);

            object[] _args = new object[mi.GetMethod().GetParameters().Count];
            if (args.Length > _args.Length)
                throw new Exception("Parameters count is more than method accepts");
            args.CopyTo(_args, 0);

            object retVal = externalCT.Execute(mi, lobi, ref _args);
            args = _args;
            return retVal;
        }

        public static void SetBCSFieldValue(SPListItem item, string bcsFieldInternalName, object bcsLookupObject, string lookupObjectIdentityName = "Id")
        {
            SPField field = item.Fields.GetFieldByInternalName(bcsFieldInternalName);
            if (field == null)
                throw new Exception("BCS field not found by internal name");

            XmlDocument fieldSchema = new XmlDocument();
            fieldSchema.LoadXml(field.SchemaXml);

            string identityField = fieldSchema.FirstChild.Attributes["RelatedFieldWssStaticName"].Value;
            string valueField = fieldSchema.FirstChild.Attributes["BdcField"].Value;
            string identity = EntityInstanceIdEncoder.EncodeEntityInstanceId(new object[] { Reflection.GetPropertyValue<Object>(bcsLookupObject, lookupObjectIdentityName) });

            item[identityField] = identity;
            item[bcsFieldInternalName] = Reflection.GetPropertyValue<Object>(bcsLookupObject, valueField);
        }
    }
}
