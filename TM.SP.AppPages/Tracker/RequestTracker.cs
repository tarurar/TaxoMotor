using System;
using System.Globalization;
using Microsoft.SharePoint;
using TM.Utils;

namespace TM.SP.AppPages.Tracker
{
    /// <summary>
    /// Журналирование междведомственных запросов
    /// </summary>
    public class RequestTracker: ITracker
    {
        protected ITrackingContext<SPListItem> Context;
        protected OutcomeRequestTrackingData TrackingData;
        protected SPList TrackList;
        protected SPList RequestTypeList;
        protected SPListItem RequestTypeItem;
        private static SPFieldLookupValue CreateLookup(SPListItem lookupItem)
        {
            return lookupItem != null ? new SPFieldLookupValue(lookupItem.ID, lookupItem.Title) : null;
        }
        private SPListItem GetLicenseRootParentItem()
        {
            if (Context.License != null)
            {
                SPListItem parentItem;
                Utility.TryGetListItemFromLookupValue(Context.License["Tm_LicenseRtParentLicenseLookup"],
                    Context.License.Fields.GetFieldByInternalName("Tm_LicenseRtParentLicenseLookup") as SPFieldLookup,
                    out parentItem);

                return parentItem;
            }

            return null;
        }
        private SPFolder CreateFolder()
        {
            var yearStr  = DateTime.Now.ToString("yyyy");
            var monthstr = DateTime.Now.ToString("MMMM");
            var dayStr   = DateTime.Now.ToString("dd");
            var hourStr  = DateTime.Now.ToString("hh");

            return TrackList.RootFolder.CreateSubFolders(new[] { yearStr, monthstr, dayStr, hourStr });
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Интерфейс контекста, в котором был осуществлен межведоственный запрос</param>
        /// <param name="trackingData">Информация о межведоственном запросе, необходимая для корректного журналирования</param>
        public RequestTracker(ITrackingContext<SPListItem> context, OutcomeRequestTrackingData trackingData)
        {
            Context = context;
            TrackingData = trackingData;

            TrackList = Context.Web.GetListOrBreak("Lists/OutcomeRequestStateList");
            RequestTypeList = Context.Web.GetListOrBreak("Lists/OutcomeRequestTypeBookList");
            RequestTypeItem = RequestTypeList.GetSingleListItemByFieldValue("Tm_ServiceCode",
                ((int) trackingData.Type).ToString(CultureInfo.InvariantCulture));
        }
        public virtual void Track()
        {
            var folder = CreateFolder();
            var trackItem = TrackList.AddItem(folder.ServerRelativeUrl, SPFileSystemObjectType.File);

            trackItem["Title"]                           = RequestTypeItem != null ? RequestTypeItem.Title : "Запрос";
            trackItem["Tm_OutputDate"]                   = DateTime.Now;
            trackItem["Tm_IncomeRequestLookup"]          = CreateLookup(Context.IncomeRequest);
            trackItem["Tm_TaxiLookup"]                   = CreateLookup(Context.Taxi);
            trackItem["Tm_LicenseLookup"]                = CreateLookup(Context.License);
            trackItem["Tm_OutputRequestTypeLookup"]      = CreateLookup(RequestTypeItem);
            trackItem["Tm_AnswerReceived"]               = false;
            trackItem["Tm_MessageId"]                    = TrackingData.Id;
            trackItem["Tm_LicenseRtParentLicenseLookup"] = CreateLookup(GetLicenseRootParentItem());

            trackItem.Update();
        }
    }
}
