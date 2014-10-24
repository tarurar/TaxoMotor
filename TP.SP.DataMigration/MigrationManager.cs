using System;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using TM.SP.BCSModels;
using TM.Utils;

namespace TP.SP.DataMigration
{
    public class MigrationManager<TItemType, TTicketType> where TTicketType: MigratingTicket
    {
        private string _lob;
        private string _ns;

        public MigrationManager(string lobName, string lobNamespace)
        {
            _lob = lobName;
            _ns = lobNamespace;
        }

        public SPListItem Process(int id, string contentType, string getItemMethodName, SPWeb web, Func<SPWeb, TItemType, SPListItem> assignFields)
        {
            // trying to get entity with specified id
            var ticket = BCS.ExecuteBcsMethod<TTicketType>(new BcsMethodExecutionInfo
            {
                lob         = _lob,
                ns          = _ns,
                contentType = contentType,
                methodName  = "TakeItemForMigrationInstance",
                methodType  = MethodInstanceType.Scalar
            }, id);
            if (ticket == null) return null;

            try
            {
                // set entity status to designate acting
                ticket.Status = (Int32)MigratingStatus.Processing;
                BCS.ExecuteBcsMethod<TTicketType>(new BcsMethodExecutionInfo
                {
                    lob         = _lob,
                    ns          = _ns,
                    contentType = contentType,
                    methodName  = "UpdateMigrationStatus",
                    methodType  = MethodInstanceType.Updater
                }, ticket);

                // process entity itself
                var item = BCS.ExecuteBcsMethod<TItemType>(new BcsMethodExecutionInfo
                {
                    lob         = _lob,
                    ns          = _ns,
                    contentType = contentType,
                    methodName  = getItemMethodName,
                    methodType  = MethodInstanceType.SpecificFinder
                }, ticket.ItemId);
                var spItem = assignFields(web, item);

                ticket.Status = (Int32)MigratingStatus.Processed;
                BCS.ExecuteBcsMethod<TTicketType>(new BcsMethodExecutionInfo
                {
                    lob         = _lob,
                    ns          = _ns,
                    contentType = contentType,
                    methodName  = "FinishMigrationInstance",
                    methodType  = MethodInstanceType.Updater
                }, ticket);

                return spItem;
            }
            catch (Exception ex)
            {
                ticket.Status    = (Int32)MigratingStatus.Error;
                ticket.ErrorInfo = ex.Message;
                ticket.StackInfo = ex.StackTrace;
                BCS.ExecuteBcsMethod<TTicketType>(new BcsMethodExecutionInfo
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
