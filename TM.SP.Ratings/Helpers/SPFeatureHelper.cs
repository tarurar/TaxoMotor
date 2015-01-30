using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.Ratings.Helpers
{
    public static class SPFeatureHelper
    {
        public static string GetFeatureLocalizedResource(string resourceName, string featureId, uint lang = 1033)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", featureId, resourceName), string.Empty, lang);
        }
    }
}
