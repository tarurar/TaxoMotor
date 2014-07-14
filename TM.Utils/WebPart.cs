using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client.WebParts;
using PersonalizationScope = System.Web.UI.WebControls.WebParts.PersonalizationScope;

namespace TM.Utils
{
    public class WebPart
    {
        public static void RemoveWebPartDuplicatesOnPage(SPWeb web, SPFolder folder, string pageFileName)
        {
            var items = folder.Files;

            // find the right Page Layout
            foreach (var wpm in from SPFile item in items
                                where item.Name.Equals(pageFileName, StringComparison.CurrentCultureIgnoreCase)
                                select item.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                if (wpm.WebParts.Count > 1)
                {
                    var webParts = new List<string>();
                    // iterate through all Web Parts and remove duplicates
                    for (var index = wpm.WebParts.Count - 1; index >= 0; index--)
                    {
                        var sb = new StringBuilder();
                        using (var ms = new MemoryStream())
                        {
                            var xw = new XmlTextWriter(ms, Encoding.UTF8);
                            var wp = wpm.WebParts[index];
                            wpm.ExportWebPart(wp, xw);
                            xw.Flush();

                            string md5Hash = Security.GetMd5Hash(sb.ToString());
                            if (webParts.Contains(md5Hash))
                            {
                                wpm.DeleteWebPart(wp);
                            }
                            else
                            {
                                webParts.Add(md5Hash);
                            }
                        }
                    }
                }
            }
        }
    }
}
