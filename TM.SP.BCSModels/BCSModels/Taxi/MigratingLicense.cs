﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.BCSModels.Taxi
{
    public partial class MigratingLicense
    {
        public System.Nullable<System.Int32> TicketId { get; set; }
        public System.Nullable<System.Int32> Status { get; set; }
        public System.String ErrorInfo { get; set; }
        public System.String StackInfo { get; set; }
        public System.Nullable<System.Int32> LicenseId { get; set; }
    }
}
