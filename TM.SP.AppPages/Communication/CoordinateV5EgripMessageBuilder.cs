using Microsoft.SharePoint;
using TM.Services.CoordinateV5;

namespace TM.SP.AppPages.Communication
{
    public class CoordinateV5EgripMessageBuilder: CoordinateV5EgrulMessageBuilder
    {
        public CoordinateV5EgripMessageBuilder(SPListItem item, IRequestAccountData accountData): base(item, accountData) { }
        protected override CoordinateTaskMessage GetMessageTemplate()
        {
            return Helpers.GetEGRIPMessageTemplate(GetTaskParam());
        }
    }
}
