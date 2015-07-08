using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using TM.Utils;
using CV52 = TM.Services.CoordinateV52;

namespace TM.SP.AppPages.Communication
{
    public class CoordinateV52StatusMessageBuilder : CoordinateV52BaseMessageBuilder<CV52.CoordinateStatusMessage>
    {
        private readonly SPListItem _item;
        private readonly List<int> _attachIdList;
        private readonly SPList _attachList;

        public CoordinateV52StatusMessageBuilder(SPListItem item, List<int> attachIdList)
        {
            var web = item.Web;

            _item = item;
            _attachIdList = attachIdList;
            _attachList = web.GetListOrBreak("AttachLib");
        }

        private CV52.ServiceDocument GetAttachments()
        {
            CV52.ServiceDocument retVal = null;

            if (_attachIdList != null && _attachIdList.Count > 0)
            {
                retVal = new CV52.ServiceDocument
                {
                    DocCode = "10004",
                    DocDate = DateTime.Now,
                    DocNumber = "БН",
                    DocFiles = (from attachId in _attachIdList
                        select _attachList.GetItemById(attachId)
                        into spItem
                        let content = spItem.File.OpenBinary()
                        select new CV52.File
                        {
                            FileContent = content,
                            FileName = spItem.File.Name,
                        }).ToArray()
                };
            }

            return retVal;
        }

        public override CV52.CoordinateStatusMessage Build()
        {
            var sNumber   = _item.TryGetValue<string>("Tm_SingleNumber");
            var stCode =
                Utility.GetStatusServiceCodeFromLookup(_item.TryGetLookupValue("_x0421__x043e__x0441__x0442__x04"));

            var attachs = GetAttachments();
            string[] denyReasonList = null;
            if (stCode == 1030 || stCode == 1080)
            {
                denyReasonList = IncomeRequestHelper.GetDenyReasons(_item)
                    .Select(x => x.TryGetValue<string>("Tm_ServiceCodeSend"))
                    .ToArray();
            }

            var message = new CV52.CoordinateStatusMessage
            {
                ServiceHeader = new CV52.Headers
                {
                    FromOrgCode     = Consts.TaxoMotorDepCode,
                    ToOrgCode       = Consts.AsgufSysCode,
                    MessageId       = Guid.NewGuid().ToString("D"),
                    RequestDateTime = DateTime.Now,
                    ServiceNumber   = sNumber
                },
                StatusMessage = new CV52.CoordinateStatusData
                {
                    ServiceNumber   = sNumber,
                    StatusCode      = stCode,
                    Documents       = attachs != null ? new [] { attachs } : null,
                    Result          = IncomeRequestHelper.GetResultObjectForCoordinateV52StatusMessage(stCode, denyReasonList),
                    StatusDate      = DateTime.Now
                }
            };

            return message;
        }
    }
}
