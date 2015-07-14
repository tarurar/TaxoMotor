using System.Globalization;
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
        private readonly SPListItem _item;
        private readonly IRequestAccountData _accountData;

        protected XmlElement GetTaskParam()
        {
            var el = new XElement("ServiceProperties",
                new XAttribute("xmlns", String.Empty),
                new XElement("ogrn", _accountData.Ogrn),
                new XElement("inn", _accountData.Inn));

            var doc = new XmlDocument();
            doc.Load(el.CreateReader());

            return doc.DocumentElement;
        }

        protected virtual CV5.CoordinateTaskMessage GetMessageTemplate()
        {
            return CV5.Helpers.GetEGRULMessageTemplate(GetTaskParam());
        }
        public CoordinateV5EgrulMessageBuilder(SPListItem item, IRequestAccountData accountData)
        {
            _item = item;
            _accountData = accountData;
        }

        public override CV5.CoordinateTaskMessage Build()
        {
            var sNumber = _item.TryGetValue<string>("Tm_SingleNumber");
            if (String.IsNullOrEmpty(sNumber))
            {
                const string snPattern = "{0}-{1}-{2}-{3}/{4}";
                sNumber = String.Format(snPattern, Consts.TaxoMotorDepCode, Consts.TaxoMotorSysCode, "77200101",
                    String.Format("{0:000000}", 1), DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).Right(2));
            }

            var sCode = "77200101";
            if (_item.Fields.ContainsField("Tm_RequestedDocument"))
            {
                // Элемент списка "Подтип госуслуги"
                SPListItem rDocumentItem;
                Utility.TryGetListItemFromLookupValue(_item["Tm_RequestedDocument"],
                    _item.ParentList.Fields.GetFieldByInternalName("Tm_RequestedDocument") as SPFieldLookup,
                    out rDocumentItem);
                sCode = rDocumentItem.TryGetValue<string>("Tm_ServiceCode");
            }

            var message = GetMessageTemplate();
            message.ServiceHeader.ServiceNumber = sNumber;
            message.TaskMessage.Task.Responsible.FirstName = String.Empty;
            message.TaskMessage.Task.Responsible.LastName = _item.Web.CurrentUser.Name;
            message.TaskMessage.Task.ServiceNumber = sNumber;
            message.TaskMessage.Task.ServiceTypeCode = sCode;
            return message;
        }
    }
}
