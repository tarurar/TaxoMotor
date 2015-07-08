using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint;
using System;
using TM.Utils;
using CV5 = TM.Services.CoordinateV5;
using BcsCoordinateV5Model = TM.SP.BCSModels.CoordinateV5;

namespace TM.SP.AppPages.Communication
{
    public class CoordinateV5EgrulMessageBuilder: CoordinateV5BaseMessageBuilder<CV5.CoordinateTaskMessage>
    {
        private readonly SPListItem _requestItem;
        private readonly BcsCoordinateV5Model.RequestAccount _account;

        protected XmlElement GetTaskParam()
        {
            var el = new XElement("ServiceProperties",
                new XAttribute("xmlns", String.Empty),
                new XElement("ogrn", _account.Ogrn),
                new XElement("inn", _account.Inn));

            var doc = new XmlDocument();
            doc.Load(el.CreateReader());

            return doc.DocumentElement;
        }

        protected virtual CV5.CoordinateTaskMessage GetMessageTemplate()
        {
            return CV5.Helpers.GetEGRULMessageTemplate(GetTaskParam());
        }
        public CoordinateV5EgrulMessageBuilder(SPListItem item, int accountId)
        {
            _requestItem = item;
            _account = IncomeRequestHelper.ReadRequestAccountItem(accountId);
        }

        public override CV5.CoordinateTaskMessage Build()
        {
            var sNumber = _requestItem.TryGetValue<string>("Tm_SingleNumber");
            // Элемент списка "Подтип госуслуги"
            SPListItem rDocumentItem;
            Utility.TryGetListItemFromLookupValue(_requestItem["Tm_RequestedDocument"],
                _requestItem.ParentList.Fields.GetFieldByInternalName("Tm_RequestedDocument") as SPFieldLookup,
                out rDocumentItem);
            var sCode = rDocumentItem.TryGetValue<string>("Tm_ServiceCode");

            var message = GetMessageTemplate();
            message.ServiceHeader.ServiceNumber = sNumber;
            message.TaskMessage.Task.Responsible.FirstName = String.Empty;
            message.TaskMessage.Task.Responsible.LastName = _requestItem.Web.CurrentUser.Name;
            message.TaskMessage.Task.ServiceNumber = sNumber;
            message.TaskMessage.Task.ServiceTypeCode = sCode;
            return message;
        }
    }
}
