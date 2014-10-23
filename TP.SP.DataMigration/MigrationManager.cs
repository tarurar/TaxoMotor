using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Applications.GroupBoard.WebControls;
using TM.SP.BCSModels;
using TM.Utils;

namespace TP.SP.DataMigration
{
    public class MigrationManager<ItemType, TicketType> where TicketType: MigratingTicket
    {
        private string _lob;
        private string _ns;

        public MigrationManager(string lobName, string lobNamespace)
        {
            this._lob = lobName;
            this._ns = lobNamespace;
        }

        public void Process(int id, string contentType, string getItemMethodName, SPWeb web, Func<SPWeb, ItemType, SPListItem> assignFields)
        {
            // trying to get entity with specified id
            TicketType ticket = BCS.ExecuteBcsMethod<TicketType>(new BcsMethodExecutionInfo()
            {
                lob         = _lob,
                ns          = _ns,
                contentType = contentType,
                methodName  = "TakeItemForMigrationInstance",
                methodType  = MethodInstanceType.Scalar
            }, id);
            if (ticket == null) return;

            try
            {
                // set entity status to designate acting
                ticket.Status = (Int32)MigratingStatus.Processing;
                BCS.ExecuteBcsMethod<TicketType>(new BcsMethodExecutionInfo()
                {
                    lob         = _lob,
                    ns          = _ns,
                    contentType = contentType,
                    methodName  = "UpdateMigrationStatus",
                    methodType  = MethodInstanceType.Updater
                }, ticket);

                // process license itself
                ItemType item = BCS.ExecuteBcsMethod<ItemType>(new BcsMethodExecutionInfo()
                {
                    lob         = _lob,
                    ns          = _ns,
                    contentType = contentType,
                    methodName  = getItemMethodName,
                    methodType  = MethodInstanceType.SpecificFinder
                }, ticket.ItemId);
                SPListItem spItem = assignFields(web, item);

                ticket.Status = (Int32)MigratingStatus.Processed;
                BCS.ExecuteBcsMethod<TicketType>(new BcsMethodExecutionInfo()
                {
                    lob         = _lob,
                    ns          = _ns,
                    contentType = contentType,
                    methodName  = "FinishMigrationInstance",
                    methodType  = MethodInstanceType.Updater
                }, ticket);
            }
            catch (Exception ex)
            {
                ticket.Status    = (Int32)MigratingStatus.Error;
                ticket.ErrorInfo = ex.Message;
                ticket.StackInfo = ex.StackTrace;
                BCS.ExecuteBcsMethod<TicketType>(new BcsMethodExecutionInfo()
                {
                    lob         = _lob,
                    ns          = _ns,
                    contentType = contentType,
                    methodName  = "FinishMigrationInstance",
                    methodType  = MethodInstanceType.Updater
                }, ticket);

                throw;
            }
        }
    }
}
