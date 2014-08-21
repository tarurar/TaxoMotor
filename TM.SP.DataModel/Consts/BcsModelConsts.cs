using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace TM.SP.DataModel
{
    public static class BcsModelConsts
    {
        public static readonly string CV5SystemName = "CoordinateV5";
        public static readonly string CV5EntityNamespace = "TM.SP.BCSModels.CoordinateV5";
        public static readonly string CV5RequestAccountEntityName = "RequestAccount";
        public static readonly string CV5RequestContactEntityName = "RequestContact";

        public static readonly string CV5ListsFeatureId = "{88749623-db7e-4ffc-b1e4-b6c4cf9332b6}";
    }
}
