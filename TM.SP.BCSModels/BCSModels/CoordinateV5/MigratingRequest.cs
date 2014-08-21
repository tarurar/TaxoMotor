using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.BCSModels.CoordinateV5
{
    public enum MigratingStatus
    {
        Undefined = 0,
        Reserved,
        Processing,
        Processed,
        Error
    }

    public partial class MigratingRequest
    {
        public System.Nullable<System.Int32> TicketId { get; set; }
        public System.Nullable<System.Int32> Status { get; set; }
        public System.String ErrorInfo {get; set;}
        public System.String StackInfo {get; set;}
        public System.Nullable<System.Int32> RequestId { get; set; }
    }
}
