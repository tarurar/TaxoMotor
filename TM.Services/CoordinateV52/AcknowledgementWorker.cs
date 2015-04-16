using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace TM.Services.CoordinateV52
{
    public class AcknowledgementWorker
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Headers _headers;
        private string _bindingName;
        private bool _silentError;

        private AcknowledgementWorker() { }
        public AcknowledgementWorker(Headers h, string bindingName, bool silentError = true)
        {
            this._headers = h;
            this._bindingName = bindingName;
            this._silentError = silentError;
        }

        public void Work()
        {
            try
            {
                CoordinateV52Client client = new CoordinateV52Client(this._bindingName);
                client.Acknowledgement(this._headers, new ErrorMessageData()
                {
                    ErrorCode = "0",
                    ErrorText = String.Empty
                });
            }
            catch (Exception ex)
            {
                logger.Error("Unable to send acknowledgement", ex);
                if (!this._silentError) throw;
            }
        }
    }
}