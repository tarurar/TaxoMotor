using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace TP.SP.DataMigration
{
    public static class MigrationManager
    {
        public static void ProcessAny<ItemType>(int maxCount = 0)
        {
            
        }

        public static void Process<ItemType, TicketType>(int id, string contentType, Func<SPWeb, ItemType, SPListItem> assignFields)
        {
            // TakeItemForMigration method - add parameter
        }
    }
}
