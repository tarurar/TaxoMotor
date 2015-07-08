using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint;
using TM.Utils;
using CV5 = TM.Services.CoordinateV5;

namespace TM.SP.AppPages.Communication
{
    public class CoordinateV5PtsMessageBuilder : CoordinateV5BaseMessageBuilder<CV5.CoordinateTaskMessage>
    {
        private readonly SPListItem _item;
        private const string SnPattern = "{0}-{1}-{2}-{3}/{4}";

        private static XmlElement GetTaskParam(string stateNumber)
        {
            var el = new XElement("ServiceProperties",
                new XAttribute("xmlns", String.Empty),
                new XElement("regno", stateNumber));

            var doc = new XmlDocument();
            doc.Load(el.CreateReader());

            return doc.DocumentElement;
        }

        public CoordinateV5PtsMessageBuilder(SPListItem item)
        {
            _item = item;
        }
        public override CV5.CoordinateTaskMessage Build()
        {
            #region [Getting linked items from lists]
            string sNumber = null;
            string sCode = null;

            if (_item.ParentList.RootFolder.Name == "TaxiList")
            {
                // Элемент списка "Обращения"
                SPListItem irItem;
                Utility.TryGetListItemFromLookupValue(_item["Tm_IncomeRequestLookup"],
                    _item.ParentList.Fields.GetFieldByInternalName("Tm_IncomeRequestLookup") as SPFieldLookup,
                    out irItem);

                if (irItem != null)
                {
                    // Элемент списка "Подтип госуслуги"
                    SPListItem rDocumentItem;
                    Utility.TryGetListItemFromLookupValue(irItem["Tm_RequestedDocument"],
                        irItem.ParentList.Fields.GetFieldByInternalName("Tm_RequestedDocument") as SPFieldLookup,
                        out rDocumentItem);

                    sNumber = irItem.TryGetValue<string>("Tm_SingleNumber");
                    if (rDocumentItem != null)
                        sCode = rDocumentItem.TryGetValue<string>("Tm_ServiceCode");
                }
            }

            if (String.IsNullOrEmpty(sNumber))
                sNumber = String.Format(SnPattern, Consts.TaxoMotorDepCode, Consts.TaxoMotorSysCode, "77200101",
                    String.Format("{0:000000}", 1), DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).Right(2));
            if (String.IsNullOrEmpty(sCode))
                sCode = "77200101";
            #endregion

            #region [Building outcome request]

            var taxiStateNumber = _item.TryGetValue<string>("Tm_TaxiStateNumber");
            var message =
                CV5.Helpers.GetPTSMessageTemplate(GetTaskParam(taxiStateNumber));
            message.ServiceHeader.ServiceNumber = sNumber;
            message.TaskMessage.Task.Responsible.FirstName = String.Empty;
            message.TaskMessage.Task.Responsible.LastName = _item.Web.CurrentUser.Name;
            message.TaskMessage.Task.ServiceNumber = sNumber;
            message.TaskMessage.Task.ServiceTypeCode = sCode;

            #endregion

            return message;
        }
    }
}
