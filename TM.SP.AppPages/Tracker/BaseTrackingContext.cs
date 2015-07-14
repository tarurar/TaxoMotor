using Microsoft.SharePoint;
using System;

namespace TM.SP.AppPages.Tracker
{
    /// <summary>
    /// Базовый класс реализации контекста логгирования
    /// </summary>
    public class BaseTrackingContext: ITrackingContext<SPListItem>
    {
        protected SPListItem Item;
        public BaseTrackingContext(SPListItem item)
        {
            Item = item;
        }

        public SPListItem IncomeRequest
        {
            get { return GetIncomeRequest(); }
        }

        public SPListItem Taxi
        {
            get { return GetTaxi(); }
        }

        public SPListItem License
        {
            get { return GetLicense(); }
        }

        public SPWeb Web
        {
            get { return Item.Web; }
        }

        protected virtual SPListItem GetIncomeRequest()
        {
            return null;
        }

        protected virtual SPListItem GetTaxi()
        {
            return null;
        }

        protected virtual SPListItem GetLicense()
        {
            return null;
        }
    }
}
