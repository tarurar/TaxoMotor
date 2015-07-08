using Microsoft.SharePoint;
using TM.Services.CoordinateV5;

namespace TM.SP.AppPages.Communication
{
    public class CoordinateV5EgripMessageBuilder: CoordinateV5EgrulMessageBuilder
    {
        public CoordinateV5EgripMessageBuilder(SPListItem item, int accountId): base(item, accountId) { }
        protected override CoordinateTaskMessage GetMessageTemplate()
        {
            return Helpers.GetEGRIPMessageTemplate(GetTaskParam());
        }
    }
}
