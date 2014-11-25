using System;
using System.Xml;
using TM.Utils;

namespace TM.Services.CoordinateV5
{
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
            message.ServiceHeader.FromOrgCode           = Consts.TaxoMotorDepCode;
            message.ServiceHeader.ToOrgCode             = Consts.TaxInspectionSysCode;
            message.ServiceHeader.MessageId             = Guid.NewGuid().ToString("D");
            message.ServiceHeader.RequestDateTime       = DateTime.Now;
            // TaskMessage.Data
            message.TaskMessage.Data.DocumentTypeCode   = Consts.EgripDocCode;
            message.TaskMessage.Data.IncludeBinaryView  = true;
            message.TaskMessage.Data.IncludeXmlView     = true;
            message.TaskMessage.Data.ParameterTypeCode  = String.Empty;
            message.TaskMessage.Data.Parameter          = parameter;
            // TaskMessage.Signature
            message.TaskMessage.Signature               = null;
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
            message.TaskMessage.Data.DocumentTypeCode   = Consts.EgripDocCode;

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
            message.ServiceHeader.FromOrgCode           = Consts.TaxoMotorDepCode;
            message.ServiceHeader.ToOrgCode             = Consts.GibddSysCode;
            message.ServiceHeader.MessageId             = Guid.NewGuid().ToString("D");
            message.ServiceHeader.RequestDateTime       = DateTime.Now;
            // TaskMessage.Data
            message.TaskMessage.Data.DocumentTypeCode   = Consts.PtsDocCode;
            message.TaskMessage.Data.IncludeBinaryView  = true;
            message.TaskMessage.Data.IncludeXmlView     = true;
            message.TaskMessage.Data.ParameterTypeCode  = String.Empty;
            message.TaskMessage.Data.Parameter          = parameter;
            // TaskMessage.Signature
            message.TaskMessage.Signature               = null;
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

        public static CoordinateTaskMessage GetPenaltyMessageTemplate(XmlElement parameter)
        {
            var message = GetDefaultMessageTemplate();
            // ServiceHeader
            message.ServiceHeader.FromOrgCode           = Consts.TaxoMotorDepCode;
            message.ServiceHeader.ToOrgCode             = Consts.GibddSysCode;
            message.ServiceHeader.MessageId             = Guid.NewGuid().ToString("D");
            message.ServiceHeader.RequestDateTime       = DateTime.Now;
            // TaskMessage.Data
            message.TaskMessage.Data.DocumentTypeCode   = Consts.PenaltyDocCode;
            message.TaskMessage.Data.IncludeBinaryView  = true;
            message.TaskMessage.Data.IncludeXmlView     = true;
            message.TaskMessage.Data.ParameterTypeCode  = String.Empty;
            message.TaskMessage.Data.Parameter          = parameter;
            // TaskMessage.Signature
            message.TaskMessage.Signature               = null;
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