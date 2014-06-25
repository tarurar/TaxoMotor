﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NLog;
using System.Reflection;

namespace TM.Services.CoordinateV5
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, Namespace = Namespace.ServiceNamespace)]
    public class CoordinateV5Service : ICoordinateV5Service
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly string logErrFmt = "Method name :{0}; Exception Details: {1}";

        public GetRequestListOutMessage GetRequestList(GetRequestListInMessage request)
        {
            GetRequestListOutMessage msg = null;

            try
            {
                msg = ServiceImplementation.GetRequestList(request);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(logErrFmt, MethodBase.GetCurrentMethod().ToString(), ex.Message), ex);
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
                logger.Error(String.Format(logErrFmt, MethodBase.GetCurrentMethod().ToString(), ex.Message), ex);
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
                logger.Error(String.Format(logErrFmt, MethodBase.GetCurrentMethod().ToString(), ex.Message), ex);
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
                logger.Error(String.Format(logErrFmt, MethodBase.GetCurrentMethod().ToString(), ex.Message), ex);
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
                logger.Error(String.Format(logErrFmt, MethodBase.GetCurrentMethod().ToString(), ex.Message), ex);
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
                logger.Error(String.Format(logErrFmt, MethodBase.GetCurrentMethod().ToString(), ex.Message), ex);
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
                logger.Error(String.Format(logErrFmt, MethodBase.GetCurrentMethod().ToString(), ex.Message), ex);
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
                logger.Error(String.Format(logErrFmt, MethodBase.GetCurrentMethod().ToString(), ex.Message), ex);
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
                logger.Error(String.Format(logErrFmt, MethodBase.GetCurrentMethod().ToString(), ex.Message), ex);
            }
        }
    }
}
