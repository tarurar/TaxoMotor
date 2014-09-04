using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace TM.Utils
{
    public static class Config
    {
        private static readonly string configurationListUrl = "Lists/ConfigurationList";
        public static SPListItem GetConfigItem(SPWeb web, string keyValue)
        {
            SPList confList = null;

            try
            {
                confList = web.GetList(configurationListUrl);        
            }
            catch (Exception)
            {
                throw new Exception(String.Format("There is no configuration list (expected url = {0})", configurationListUrl));
            }

            SPListItem confItem = (from item in confList.Items.OfType<SPListItem>()
                                  where item.Title.Equals(keyValue, StringComparison.InvariantCultureIgnoreCase)
                                  select item).FirstOrDefault();

            if (confItem == null)
                throw new Exception(String.Format("Configuration item with the key {0} was not found", keyValue));

            return confItem;
        }

        public static object GetConfigValue(SPListItem configItem)
        {
            return configItem["Tm_ConfigurationValue"];
        }
    }
}
