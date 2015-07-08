using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint;
using TM.Utils;
using CV5 = TM.Services.CoordinateV5;

namespace TM.SP.AppPages.Communication
{
    public class CoordinateV5PenaltyMessageBuilder : CoordinateV5BaseMessageBuilder<CV5.CoordinateTaskMessage>
    {
        private readonly SPListItem _item;
        private const string SnPattern = "{0}-{1}-{2}-{3}/{4}";

        public CoordinateV5PenaltyMessageBuilder(SPListItem item)
        {
            _item = item;
        }

        private static XmlElement GetTaskParam(string stateNumber)
        {
            var el = new XElement("ServiceProperties",
                new XAttribute("xmlns", String.Empty),
                new XElement("regpointnum", stateNumber));

            var doc = new XmlDocument();
            doc.Load(el.CreateReader());

            return doc.DocumentElement;
        }

        public override CV5.CoordinateTaskMessage Build()
        {
            var sn = String.Format(SnPattern, Consts.TaxoMotorDepCode, Consts.TaxoMotorSysCode, "77200101",
                String.Format("{0:000000}", 1), DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).Right(2));

            #region [Building outcome request]

            var taxiStateNumber = _item.TryGetValue<string>("Tm_TaxiStateNumber");
            var message = CV5.Helpers.GetPenaltyMessageTemplate(GetTaskParam(taxiStateNumber));
            message.ServiceHeader.ServiceNumber = sn;
            message.TaskMessage.Task.Responsible.FirstName = String.Empty;
            message.TaskMessage.Task.Responsible.LastName = _item.Web.CurrentUser.Name;
            message.TaskMessage.Task.ServiceNumber = sn;
            message.TaskMessage.Task.ServiceTypeCode = "77200101";
            #endregion

            return message;
        }
    }
}
