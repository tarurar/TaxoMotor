using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NLog;
using System.Reflection;
using System.Threading.Tasks;

namespace TM.Services.CoordinateV52
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, Namespace = Namespace.ServiceNamespace)]
    public class CoordinateV52Service : ICoordinateV52Service
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly string logErrorFmt = "There was an error while executing {0} method implementation";

        public GetRequestListOutMessage GetRequestList(GetRequestListInMessage request)
        {
            GetRequestListOutMessage msg = null;

            try
            {
                msg = ServiceImplementation.GetRequestList(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrorFmt, MethodInfo.GetCurrentMethod().Name), ex);
            }

            return msg;
        }

        public GetRequestsOutMessage GetRequests(GetRequestsInMessage request)
        {
            GetRequestsOutMessage msg = null;

            try
            {
                msg = ServiceImplementation.GetRequests(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrorFmt, MethodInfo.GetCurrentMethod().Name), ex);
            }

            return msg;
        }

        public void SendRequest(CoordinateMessage request)
        {
            try
            {
                ServiceImplementation.SendRequest(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrorFmt, MethodInfo.GetCurrentMethod().Name), ex);
            }
        }

        public void SendTask(CoordinateTaskMessage request)
        {
            try
            {
                ServiceImplementation.SendTask(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrorFmt, MethodInfo.GetCurrentMethod().Name), ex);
            }
        }

        public void SetFilesAndStatus(CoordinateStatusMessage request)
        {
            try
            {
                ServiceImplementation.SetFilesAndStatus(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrorFmt, MethodInfo.GetCurrentMethod().Name), ex);
            }
        }

        public void Acknowledgement(ErrorMessage request)
        {
            try
            {
                ServiceImplementation.Acknowledgement(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrorFmt, MethodInfo.GetCurrentMethod().Name), ex);
            }
        }

        public void SendRequests(SendRequestsMessage request)
        {
            try
            {
                ServiceImplementation.SendRequests(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrorFmt, MethodInfo.GetCurrentMethod().Name), ex);
            }
        }

        public void SendTasks(SendTasksMessage request)
        {
            try
            {
                ServiceImplementation.SendTasks(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrorFmt, MethodInfo.GetCurrentMethod().Name), ex);
            }
        }

        public void SetFilesAndStatuses(SetFilesAndStatusesMessage request)
        {
            try
            {
                ServiceImplementation.SetFilesAndStatuses(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrorFmt, MethodInfo.GetCurrentMethod().Name), ex);
            }
        }
    }
}
