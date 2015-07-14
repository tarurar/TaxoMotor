using Microsoft.SharePoint;
using TM.Utils;

namespace TM.SP.AppPages.Tracker
{
    /// <summary>
    /// Контекст транспортного средства
    /// </summary>
    public class TaxiTrackingContext: BaseTrackingContext
    {
        public TaxiTrackingContext(SPListItem item) : base(item)
        {
        }

        protected override SPListItem GetTaxi()
        {
            return Item;
        }

        protected override SPListItem GetIncomeRequest()
        {
            SPListItem irItem;
            Utility.TryGetListItemFromLookupValue(Item["Tm_IncomeRequestLookup"],
                Item.Fields.GetFieldByInternalName("Tm_IncomeRequestLookup") as SPFieldLookup, out irItem);

            return irItem != null ? new IncomeRequestTrackingContext(irItem).IncomeRequest : base.GetIncomeRequest();
        }
    }
}
