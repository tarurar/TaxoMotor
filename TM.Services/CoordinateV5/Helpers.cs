using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace TM.Services.CoordinateV5
{
    public static class Consts
    {
        #region [E-government Informational System Codes]
        public static readonly string ASGUFSysCode           = "1131";
        public static readonly string TaxoMotorSysCode       = "1131";
        public static readonly string TaxInspectionSysCode   = "705";
        public static readonly string GIBDDSysCode           = "775";
        #endregion
        #region [E-government Service Codes]
        public static readonly string BaseRegistrServiceCode = "БР2";
        #endregion
        #region [Government Department Names]
        public static readonly string TaxoMotorDepName       = "ДТиРДТИ";
        #endregion
        #region [Government Department Codes]
        public static readonly string TaxoMotorDepCode       = "2009";
        #endregion
        #region [Document Codes]
        public static readonly string EGRULDocCode              = "4020";
        public static readonly string EGRIPDocCode              = "7830";
        public static readonly string PTSDocCode                = "10615";
        #endregion
    }
    public static class Helpers
    {
        #region [methods]
        /// <summary>
        /// Getting default CoordinateTaskMessage message with initialized internal data structures
        /// </summary>
        /// <returns></returns>
        public static CoordinateTaskMessage GetDefaultMessageTemplate()
        {
            return new CoordinateTaskMessage() 
            {
                ServiceHeader = new Headers(),
                TaskMessage = new CoordinateTaskData()
                {
                    Data = new DocumentsRequestData(),
                    Task = new RequestTask()
                    {
                        Department = new Department(),
                        Responsible = new Person()
                    }
                }
            };
        }

        /// <summary>
        /// Getting CoordinateTaskMessage object for EGRUL request
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static CoordinateTaskMessage GetEGRULMessageTemplate(XmlElement parameter)
        {
            var message = GetDefaultMessageTemplate();
            // ServiceHeader
            message.ServiceHeader.FromOrgCode           = Consts.TaxoMotorSysCode;
            message.ServiceHeader.ToOrgCode             = Consts.TaxInspectionSysCode;
            message.ServiceHeader.MessageId             = Guid.NewGuid().ToString("D");
            message.ServiceHeader.RequestDateTime       = DateTime.Now;
            // TaskMessage.Data
            message.TaskMessage.Data.DocumentTypeCode   = Consts.EGRULDocCode;
            message.TaskMessage.Data.IncludeBinaryView  = true;
            message.TaskMessage.Data.IncludeXmlView     = true;
            message.TaskMessage.Data.ParameterTypeCode  = String.Empty;
            message.TaskMessage.Data.Parameter          = parameter;
            // TaskMessage.Signature
            message.TaskMessage.Signature               = String.Empty;
            // TaskMessage.Task
            message.TaskMessage.Task.Code               = Consts.BaseRegistrServiceCode;
            message.TaskMessage.Task.Department.Name    = Consts.TaxoMotorDepName;
            message.TaskMessage.Task.Department.Code    = Consts.TaxoMotorDepCode;
            message.TaskMessage.Task.Department.RegDate = null;
            message.TaskMessage.Task.RequestId          = Guid.NewGuid().ToString("D");
            message.TaskMessage.Task.Subject            = String.Empty;
            message.TaskMessage.Task.ValidityPeriod     = null;

            return message;
        }

        /// <summary>
        /// Getting CoordinateTaskMessage object for EGRIP request
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static CoordinateTaskMessage GetEGRIPMessageTemplate(XmlElement parameter)
        {
            var message = GetEGRULMessageTemplate(parameter);
            // TaskMessage.Data
            message.TaskMessage.Data.DocumentTypeCode   = Consts.EGRIPDocCode;

            return message;
        }

        /// <summary>
        /// Getting CoordinateTaskMessage object for PTS request
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static CoordinateTaskMessage GetPTSMessageTemplate(XmlElement parameter)
        {
            var message = GetDefaultMessageTemplate();
            // ServiceHeader
            message.ServiceHeader.FromOrgCode           = Consts.TaxoMotorSysCode;
            message.ServiceHeader.ToOrgCode             = Consts.GIBDDSysCode;
            message.ServiceHeader.MessageId             = Guid.NewGuid().ToString("D");
            message.ServiceHeader.RequestDateTime       = DateTime.Now;
            // TaskMessage.Data
            message.TaskMessage.Data.DocumentTypeCode   = Consts.PTSDocCode;
            message.TaskMessage.Data.IncludeBinaryView  = true;
            message.TaskMessage.Data.IncludeXmlView     = true;
            message.TaskMessage.Data.ParameterTypeCode  = String.Empty;
            message.TaskMessage.Data.Parameter          = parameter;
            // TaskMessage.Signature
            message.TaskMessage.Signature               = String.Empty;
            // TaskMessage.Task
            message.TaskMessage.Task.Code               = Consts.BaseRegistrServiceCode;
            message.TaskMessage.Task.Department.Name    = Consts.TaxoMotorDepName;
            message.TaskMessage.Task.Department.Code    = Consts.TaxoMotorDepCode;
            message.TaskMessage.Task.Department.RegDate = null;
            message.TaskMessage.Task.RequestId          = Guid.NewGuid().ToString("D");
            message.TaskMessage.Task.Subject            = String.Empty;
            message.TaskMessage.Task.ValidityPeriod     = null;

            return message;
        }

        #endregion
    }
}