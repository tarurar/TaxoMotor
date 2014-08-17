using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace TM.SP.ListModels
{
    public static class Consts
    {
        public static readonly string BcsCoordinateV5SystemName = "CoordinateV5";
        public static readonly string BcsCoordinateV5EntityNamespace = "TM.SP.BCSModels.CoordinateV5";
        public static readonly string BcsRequestAccountEntityName = "RequestAccount";
        public static readonly string BcsRequestContactEntityName = "RequestContact";

        public static readonly string BcsCoordinateV5ListsFeatureId = "{88749623-db7e-4ffc-b1e4-b6c4cf9332b6}";

    }
}
