using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TM.Services.CoordinateV52
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class GetRequestListInMessage : MessageBase
    {

        private System.DateTime fromDateField;

        private System.DateTime toDateField;

        private string serviceCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public System.DateTime FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
                this.RaisePropertyChanged("FromDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
                this.RaisePropertyChanged("ToDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ServiceCode
        {
            get
            {
                return this.serviceCodeField;
            }
            set
            {
                this.serviceCodeField = value;
                this.RaisePropertyChanged("ServiceCode");
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetRequestsOutMessage))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetRequestsInMessage))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetRequestListOutMessage))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetRequestListInMessage))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public abstract partial class MessageBase : object, System.ComponentModel.INotifyPropertyChanged
    {

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class SetFilesAndStatusesData : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string serviceNumberField;

        private string requestIdField;

        private RequestStatus[] statusesField;

        private RequestResult resultField;

        private ServiceDocument[] documentsField;

        private RequestContact[] contactsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ServiceNumber
        {
            get
            {
                return this.serviceNumberField;
            }
            set
            {
                this.serviceNumberField = value;
                this.RaisePropertyChanged("ServiceNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string RequestId
        {
            get
            {
                return this.requestIdField;
            }
            set
            {
                this.requestIdField = value;
                this.RaisePropertyChanged("RequestId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        public RequestStatus[] Statuses
        {
            get
            {
                return this.statusesField;
            }
            set
            {
                this.statusesField = value;
                this.RaisePropertyChanged("Statuses");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public RequestResult Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        public ServiceDocument[] Documents
        {
            get
            {
                return this.documentsField;
            }
            set
            {
                this.documentsField = value;
                this.RaisePropertyChanged("Documents");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        public RequestContact[] Contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
                this.RaisePropertyChanged("Contacts");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class RequestStatus : object, System.ComponentModel.INotifyPropertyChanged
    {

        private int statusCodeField;

        private System.DateTime statusDateField;

        private string reasonField;

        private System.Nullable<System.DateTime> validityPeriodField;

        private Person responsibleField;

        private Department departmentField;

        private string reasonCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int StatusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
                this.RaisePropertyChanged("StatusCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime StatusDate
        {
            get
            {
                return this.statusDateField;
            }
            set
            {
                this.statusDateField = value;
                this.RaisePropertyChanged("StatusDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
                this.RaisePropertyChanged("Reason");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 3)]
        public System.Nullable<System.DateTime> ValidityPeriod
        {
            get
            {
                return this.validityPeriodField;
            }
            set
            {
                this.validityPeriodField = value;
                this.RaisePropertyChanged("ValidityPeriod");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public Person Responsible
        {
            get
            {
                return this.responsibleField;
            }
            set
            {
                this.responsibleField = value;
                this.RaisePropertyChanged("Responsible");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public Department Department
        {
            get
            {
                return this.departmentField;
            }
            set
            {
                this.departmentField = value;
                this.RaisePropertyChanged("Department");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ReasonCode
        {
            get
            {
                return this.reasonCodeField;
            }
            set
            {
                this.reasonCodeField = value;
                this.RaisePropertyChanged("ReasonCode");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class Person : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string lastNameField;

        private string firstNameField;

        private string middleNameField;

        private string jobTitleField;

        private string phoneField;

        private string emailField;

        private string isiIdField;

        private string ssoIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
                this.RaisePropertyChanged("LastName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
                this.RaisePropertyChanged("FirstName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
                this.RaisePropertyChanged("MiddleName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string JobTitle
        {
            get
            {
                return this.jobTitleField;
            }
            set
            {
                this.jobTitleField = value;
                this.RaisePropertyChanged("JobTitle");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
                this.RaisePropertyChanged("Phone");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
                this.RaisePropertyChanged("Email");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string IsiId
        {
            get
            {
                return this.isiIdField;
            }
            set
            {
                this.isiIdField = value;
                this.RaisePropertyChanged("IsiId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string SsoId
        {
            get
            {
                return this.ssoIdField;
            }
            set
            {
                this.ssoIdField = value;
                this.RaisePropertyChanged("SsoId");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class Department : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string nameField;

        private string codeField;

        private string innField;

        private string ogrnField;

        private System.Nullable<System.DateTime> regDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
                this.RaisePropertyChanged("Name");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
                this.RaisePropertyChanged("Code");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Inn
        {
            get
            {
                return this.innField;
            }
            set
            {
                this.innField = value;
                this.RaisePropertyChanged("Inn");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Ogrn
        {
            get
            {
                return this.ogrnField;
            }
            set
            {
                this.ogrnField = value;
                this.RaisePropertyChanged("Ogrn");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public System.Nullable<System.DateTime> RegDate
        {
            get
            {
                return this.regDateField;
            }
            set
            {
                this.regDateField = value;
                this.RaisePropertyChanged("RegDate");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class RequestResult : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string resultCodeField;

        private string[] declineReasonCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ResultCode
        {
            get
            {
                return this.resultCodeField;
            }
            set
            {
                this.resultCodeField = value;
                this.RaisePropertyChanged("ResultCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        public string[] DeclineReasonCode
        {
            get
            {
                return this.declineReasonCodeField;
            }
            set
            {
                this.declineReasonCodeField = value;
                this.RaisePropertyChanged("DeclineReasonCode");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class ServiceDocument : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string idField;

        private string docCodeField;

        private string docSubTypeField;

        private string docPersonField;

        private string docSerieField;

        private string docNumberField;

        private System.Nullable<System.DateTime> docDateField;

        private System.Nullable<System.DateTime> validityPeriodField;

        private string whoSignField;

        private System.Nullable<int> listCountField;

        private System.Nullable<int> copyCountField;

        private string divisionCodeField;

        private byte[] signatureField;

        private Note[] docNotesField;

        private File[] docFilesField;

        private System.Xml.XmlElement customAttributesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("Id");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string DocCode
        {
            get
            {
                return this.docCodeField;
            }
            set
            {
                this.docCodeField = value;
                this.RaisePropertyChanged("DocCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string DocSubType
        {
            get
            {
                return this.docSubTypeField;
            }
            set
            {
                this.docSubTypeField = value;
                this.RaisePropertyChanged("DocSubType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string DocPerson
        {
            get
            {
                return this.docPersonField;
            }
            set
            {
                this.docPersonField = value;
                this.RaisePropertyChanged("DocPerson");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string DocSerie
        {
            get
            {
                return this.docSerieField;
            }
            set
            {
                this.docSerieField = value;
                this.RaisePropertyChanged("DocSerie");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string DocNumber
        {
            get
            {
                return this.docNumberField;
            }
            set
            {
                this.docNumberField = value;
                this.RaisePropertyChanged("DocNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 6)]
        public System.Nullable<System.DateTime> DocDate
        {
            get
            {
                return this.docDateField;
            }
            set
            {
                this.docDateField = value;
                this.RaisePropertyChanged("DocDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 7)]
        public System.Nullable<System.DateTime> ValidityPeriod
        {
            get
            {
                return this.validityPeriodField;
            }
            set
            {
                this.validityPeriodField = value;
                this.RaisePropertyChanged("ValidityPeriod");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string WhoSign
        {
            get
            {
                return this.whoSignField;
            }
            set
            {
                this.whoSignField = value;
                this.RaisePropertyChanged("WhoSign");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 9)]
        public System.Nullable<int> ListCount
        {
            get
            {
                return this.listCountField;
            }
            set
            {
                this.listCountField = value;
                this.RaisePropertyChanged("ListCount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 10)]
        public System.Nullable<int> CopyCount
        {
            get
            {
                return this.copyCountField;
            }
            set
            {
                this.copyCountField = value;
                this.RaisePropertyChanged("CopyCount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string DivisionCode
        {
            get
            {
                return this.divisionCodeField;
            }
            set
            {
                this.divisionCodeField = value;
                this.RaisePropertyChanged("DivisionCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 12)]
        public byte[] Signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
                this.RaisePropertyChanged("Signature");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 13)]
        public Note[] DocNotes
        {
            get
            {
                return this.docNotesField;
            }
            set
            {
                this.docNotesField = value;
                this.RaisePropertyChanged("DocNotes");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 14)]
        public File[] DocFiles
        {
            get
            {
                return this.docFilesField;
            }
            set
            {
                this.docFilesField = value;
                this.RaisePropertyChanged("DocFiles");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public System.Xml.XmlElement CustomAttributes
        {
            get
            {
                return this.customAttributesField;
            }
            set
            {
                this.customAttributesField = value;
                this.RaisePropertyChanged("CustomAttributes");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class Note : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string subjectField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
                this.RaisePropertyChanged("Subject");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
                this.RaisePropertyChanged("Text");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class File : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string idField;

        private string fileNameField;

        private string mimeTypeField;

        private byte[] fileContentField;

        private System.Nullable<bool> isFileInStoreField;

        private bool isFileInStoreFieldSpecified;

        private string fileIdInStoreField;

        private string storeNameField;

        private byte[] fileChecksumField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("Id");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string FileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
                this.RaisePropertyChanged("FileName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string MimeType
        {
            get
            {
                return this.mimeTypeField;
            }
            set
            {
                this.mimeTypeField = value;
                this.RaisePropertyChanged("MimeType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 3)]
        public byte[] FileContent
        {
            get
            {
                return this.fileContentField;
            }
            set
            {
                this.fileContentField = value;
                this.RaisePropertyChanged("FileContent");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public System.Nullable<bool> IsFileInStore
        {
            get
            {
                return this.isFileInStoreField;
            }
            set
            {
                this.isFileInStoreField = value;
                this.RaisePropertyChanged("IsFileInStore");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsFileInStoreSpecified
        {
            get
            {
                return this.isFileInStoreFieldSpecified;
            }
            set
            {
                this.isFileInStoreFieldSpecified = value;
                this.RaisePropertyChanged("IsFileInStoreSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string FileIdInStore
        {
            get
            {
                return this.fileIdInStoreField;
            }
            set
            {
                this.fileIdInStoreField = value;
                this.RaisePropertyChanged("FileIdInStore");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string StoreName
        {
            get
            {
                return this.storeNameField;
            }
            set
            {
                this.storeNameField = value;
                this.RaisePropertyChanged("StoreName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 7)]
        public byte[] FileChecksum
        {
            get
            {
                return this.fileChecksumField;
            }
            set
            {
                this.fileChecksumField = value;
                this.RaisePropertyChanged("FileChecksum");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class RequestContact : BaseDeclarant
    {

        private string idField;

        private string lastNameField;

        private string firstNameField;

        private string middleNameField;

        private System.Nullable<GenderType> genderField;

        private System.Nullable<System.DateTime> birthDateField;

        private string snilsField;

        private string innField;

        private Address regAddressField;

        private Address factAddressField;

        private string mobilePhoneField;

        private string workPhoneField;

        private string homePhoneField;

        private string eMailField;

        private string nationField;

        private string citizenshipField;

        private System.Nullable<CitizenshipType> citizenshipTypeField;

        private string citizenshipCodeField;

        private Address birthAddressField;

        private string jobTitleField;

        private string oMSNumField;

        private System.Nullable<System.DateTime> oMSDateField;

        private string oMSCompanyField;

        private System.Nullable<System.DateTime> oMSValidityPeriodField;

        private string isiIdField;

        private string ssoIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("Id");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
                this.RaisePropertyChanged("LastName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
                this.RaisePropertyChanged("FirstName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
                this.RaisePropertyChanged("MiddleName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public System.Nullable<GenderType> Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
                this.RaisePropertyChanged("Gender");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 5)]
        public System.Nullable<System.DateTime> BirthDate
        {
            get
            {
                return this.birthDateField;
            }
            set
            {
                this.birthDateField = value;
                this.RaisePropertyChanged("BirthDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Snils
        {
            get
            {
                return this.snilsField;
            }
            set
            {
                this.snilsField = value;
                this.RaisePropertyChanged("Snils");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string Inn
        {
            get
            {
                return this.innField;
            }
            set
            {
                this.innField = value;
                this.RaisePropertyChanged("Inn");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public Address RegAddress
        {
            get
            {
                return this.regAddressField;
            }
            set
            {
                this.regAddressField = value;
                this.RaisePropertyChanged("RegAddress");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public Address FactAddress
        {
            get
            {
                return this.factAddressField;
            }
            set
            {
                this.factAddressField = value;
                this.RaisePropertyChanged("FactAddress");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string MobilePhone
        {
            get
            {
                return this.mobilePhoneField;
            }
            set
            {
                this.mobilePhoneField = value;
                this.RaisePropertyChanged("MobilePhone");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string WorkPhone
        {
            get
            {
                return this.workPhoneField;
            }
            set
            {
                this.workPhoneField = value;
                this.RaisePropertyChanged("WorkPhone");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string HomePhone
        {
            get
            {
                return this.homePhoneField;
            }
            set
            {
                this.homePhoneField = value;
                this.RaisePropertyChanged("HomePhone");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public string EMail
        {
            get
            {
                return this.eMailField;
            }
            set
            {
                this.eMailField = value;
                this.RaisePropertyChanged("EMail");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string Nation
        {
            get
            {
                return this.nationField;
            }
            set
            {
                this.nationField = value;
                this.RaisePropertyChanged("Nation");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string Citizenship
        {
            get
            {
                return this.citizenshipField;
            }
            set
            {
                this.citizenshipField = value;
                this.RaisePropertyChanged("Citizenship");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 16)]
        public System.Nullable<CitizenshipType> CitizenshipType
        {
            get
            {
                return this.citizenshipTypeField;
            }
            set
            {
                this.citizenshipTypeField = value;
                this.RaisePropertyChanged("CitizenshipType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public string CitizenshipCode
        {
            get
            {
                return this.citizenshipCodeField;
            }
            set
            {
                this.citizenshipCodeField = value;
                this.RaisePropertyChanged("CitizenshipCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public Address BirthAddress
        {
            get
            {
                return this.birthAddressField;
            }
            set
            {
                this.birthAddressField = value;
                this.RaisePropertyChanged("BirthAddress");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public string JobTitle
        {
            get
            {
                return this.jobTitleField;
            }
            set
            {
                this.jobTitleField = value;
                this.RaisePropertyChanged("JobTitle");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public string OMSNum
        {
            get
            {
                return this.oMSNumField;
            }
            set
            {
                this.oMSNumField = value;
                this.RaisePropertyChanged("OMSNum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 21)]
        public System.Nullable<System.DateTime> OMSDate
        {
            get
            {
                return this.oMSDateField;
            }
            set
            {
                this.oMSDateField = value;
                this.RaisePropertyChanged("OMSDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public string OMSCompany
        {
            get
            {
                return this.oMSCompanyField;
            }
            set
            {
                this.oMSCompanyField = value;
                this.RaisePropertyChanged("OMSCompany");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 23)]
        public System.Nullable<System.DateTime> OMSValidityPeriod
        {
            get
            {
                return this.oMSValidityPeriodField;
            }
            set
            {
                this.oMSValidityPeriodField = value;
                this.RaisePropertyChanged("OMSValidityPeriod");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public string IsiId
        {
            get
            {
                return this.isiIdField;
            }
            set
            {
                this.isiIdField = value;
                this.RaisePropertyChanged("IsiId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public string SsoId
        {
            get
            {
                return this.ssoIdField;
            }
            set
            {
                this.ssoIdField = value;
                this.RaisePropertyChanged("SsoId");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    
    public enum GenderType
    {

        /// <remarks/>
        Male,

        /// <remarks/>
        Female,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class Address : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string countryField;

        private string countryCodeField;

        private string postalCodeField;

        private string localityField;

        private string regionField;

        private string cityField;

        private string townField;

        private string streetField;

        private string houseField;

        private string buildingField;

        private string structureField;

        private string facilityField;

        private string ownershipField;

        private string flatField;

        private string pOBoxField;

        private string okatoField;

        private string kladrCodeField;

        private string kladrStreetCodeField;

        private string oMKDistrictCodeField;

        private string oMKRegionCodeField;

        private string oMKTownCodeField;

        private string oMKStreetCodeField;

        private string bTIStreetCodeField;

        private string bTIBuildingCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
                this.RaisePropertyChanged("Country");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
                this.RaisePropertyChanged("CountryCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
                this.RaisePropertyChanged("PostalCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Locality
        {
            get
            {
                return this.localityField;
            }
            set
            {
                this.localityField = value;
                this.RaisePropertyChanged("Locality");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
                this.RaisePropertyChanged("Region");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
                this.RaisePropertyChanged("City");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Town
        {
            get
            {
                return this.townField;
            }
            set
            {
                this.townField = value;
                this.RaisePropertyChanged("Town");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
                this.RaisePropertyChanged("Street");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string House
        {
            get
            {
                return this.houseField;
            }
            set
            {
                this.houseField = value;
                this.RaisePropertyChanged("House");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string Building
        {
            get
            {
                return this.buildingField;
            }
            set
            {
                this.buildingField = value;
                this.RaisePropertyChanged("Building");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
                this.RaisePropertyChanged("Structure");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string Facility
        {
            get
            {
                return this.facilityField;
            }
            set
            {
                this.facilityField = value;
                this.RaisePropertyChanged("Facility");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string Ownership
        {
            get
            {
                return this.ownershipField;
            }
            set
            {
                this.ownershipField = value;
                this.RaisePropertyChanged("Ownership");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public string Flat
        {
            get
            {
                return this.flatField;
            }
            set
            {
                this.flatField = value;
                this.RaisePropertyChanged("Flat");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string POBox
        {
            get
            {
                return this.pOBoxField;
            }
            set
            {
                this.pOBoxField = value;
                this.RaisePropertyChanged("POBox");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string Okato
        {
            get
            {
                return this.okatoField;
            }
            set
            {
                this.okatoField = value;
                this.RaisePropertyChanged("Okato");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string KladrCode
        {
            get
            {
                return this.kladrCodeField;
            }
            set
            {
                this.kladrCodeField = value;
                this.RaisePropertyChanged("KladrCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public string KladrStreetCode
        {
            get
            {
                return this.kladrStreetCodeField;
            }
            set
            {
                this.kladrStreetCodeField = value;
                this.RaisePropertyChanged("KladrStreetCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string OMKDistrictCode
        {
            get
            {
                return this.oMKDistrictCodeField;
            }
            set
            {
                this.oMKDistrictCodeField = value;
                this.RaisePropertyChanged("OMKDistrictCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public string OMKRegionCode
        {
            get
            {
                return this.oMKRegionCodeField;
            }
            set
            {
                this.oMKRegionCodeField = value;
                this.RaisePropertyChanged("OMKRegionCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public string OMKTownCode
        {
            get
            {
                return this.oMKTownCodeField;
            }
            set
            {
                this.oMKTownCodeField = value;
                this.RaisePropertyChanged("OMKTownCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string OMKStreetCode
        {
            get
            {
                return this.oMKStreetCodeField;
            }
            set
            {
                this.oMKStreetCodeField = value;
                this.RaisePropertyChanged("OMKStreetCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public string BTIStreetCode
        {
            get
            {
                return this.bTIStreetCodeField;
            }
            set
            {
                this.bTIStreetCodeField = value;
                this.RaisePropertyChanged("BTIStreetCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string BTIBuildingCode
        {
            get
            {
                return this.bTIBuildingCodeField;
            }
            set
            {
                this.bTIBuildingCodeField = value;
                this.RaisePropertyChanged("BTIBuildingCode");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    
    public enum CitizenshipType
    {

        /// <remarks/>
        RF,

        /// <remarks/>
        Foreign,

        /// <remarks/>
        None,

        /// <remarks/>
        Both,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RequestAccount))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RequestContact))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public abstract partial class BaseDeclarant : object, System.ComponentModel.INotifyPropertyChanged
    {

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class RequestAccount : BaseDeclarant
    {

        private string fullNameField;

        private string nameField;

        private string brandNameField;

        private string brandField;

        private string ogrnField;

        private string ogrnAuthorityField;

        private string ogrnAuthorityAddressField;

        private string ogrnNumField;

        private System.Nullable<System.DateTime> ogrnDateField;

        private string innField;

        private string innAuthorityField;

        private string innAuthorityAddressField;

        private string innNumField;

        private System.Nullable<System.DateTime> innDateField;

        private string kppField;

        private string okpoField;

        private string orgFormCodeField;

        private Address postalAddressField;

        private Address factAddressField;

        private RequestContact orgHeadField;

        private string okvedField;

        private string okfsField;

        private string bankNameField;

        private string bankBikField;

        private string corrAccountField;

        private string setAccountField;

        private string phoneField;

        private string faxField;

        private string eMailField;

        private string webSiteField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string FullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
                this.RaisePropertyChanged("FullName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
                this.RaisePropertyChanged("Name");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string BrandName
        {
            get
            {
                return this.brandNameField;
            }
            set
            {
                this.brandNameField = value;
                this.RaisePropertyChanged("BrandName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Brand
        {
            get
            {
                return this.brandField;
            }
            set
            {
                this.brandField = value;
                this.RaisePropertyChanged("Brand");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Ogrn
        {
            get
            {
                return this.ogrnField;
            }
            set
            {
                this.ogrnField = value;
                this.RaisePropertyChanged("Ogrn");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string OgrnAuthority
        {
            get
            {
                return this.ogrnAuthorityField;
            }
            set
            {
                this.ogrnAuthorityField = value;
                this.RaisePropertyChanged("OgrnAuthority");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string OgrnAuthorityAddress
        {
            get
            {
                return this.ogrnAuthorityAddressField;
            }
            set
            {
                this.ogrnAuthorityAddressField = value;
                this.RaisePropertyChanged("OgrnAuthorityAddress");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string OgrnNum
        {
            get
            {
                return this.ogrnNumField;
            }
            set
            {
                this.ogrnNumField = value;
                this.RaisePropertyChanged("OgrnNum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 8)]
        public System.Nullable<System.DateTime> OgrnDate
        {
            get
            {
                return this.ogrnDateField;
            }
            set
            {
                this.ogrnDateField = value;
                this.RaisePropertyChanged("OgrnDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string Inn
        {
            get
            {
                return this.innField;
            }
            set
            {
                this.innField = value;
                this.RaisePropertyChanged("Inn");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string InnAuthority
        {
            get
            {
                return this.innAuthorityField;
            }
            set
            {
                this.innAuthorityField = value;
                this.RaisePropertyChanged("InnAuthority");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string InnAuthorityAddress
        {
            get
            {
                return this.innAuthorityAddressField;
            }
            set
            {
                this.innAuthorityAddressField = value;
                this.RaisePropertyChanged("InnAuthorityAddress");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string InnNum
        {
            get
            {
                return this.innNumField;
            }
            set
            {
                this.innNumField = value;
                this.RaisePropertyChanged("InnNum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 13)]
        public System.Nullable<System.DateTime> InnDate
        {
            get
            {
                return this.innDateField;
            }
            set
            {
                this.innDateField = value;
                this.RaisePropertyChanged("InnDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string Kpp
        {
            get
            {
                return this.kppField;
            }
            set
            {
                this.kppField = value;
                this.RaisePropertyChanged("Kpp");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string Okpo
        {
            get
            {
                return this.okpoField;
            }
            set
            {
                this.okpoField = value;
                this.RaisePropertyChanged("Okpo");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string OrgFormCode
        {
            get
            {
                return this.orgFormCodeField;
            }
            set
            {
                this.orgFormCodeField = value;
                this.RaisePropertyChanged("OrgFormCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public Address PostalAddress
        {
            get
            {
                return this.postalAddressField;
            }
            set
            {
                this.postalAddressField = value;
                this.RaisePropertyChanged("PostalAddress");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public Address FactAddress
        {
            get
            {
                return this.factAddressField;
            }
            set
            {
                this.factAddressField = value;
                this.RaisePropertyChanged("FactAddress");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public RequestContact OrgHead
        {
            get
            {
                return this.orgHeadField;
            }
            set
            {
                this.orgHeadField = value;
                this.RaisePropertyChanged("OrgHead");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public string Okved
        {
            get
            {
                return this.okvedField;
            }
            set
            {
                this.okvedField = value;
                this.RaisePropertyChanged("Okved");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string Okfs
        {
            get
            {
                return this.okfsField;
            }
            set
            {
                this.okfsField = value;
                this.RaisePropertyChanged("Okfs");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public string BankName
        {
            get
            {
                return this.bankNameField;
            }
            set
            {
                this.bankNameField = value;
                this.RaisePropertyChanged("BankName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string BankBik
        {
            get
            {
                return this.bankBikField;
            }
            set
            {
                this.bankBikField = value;
                this.RaisePropertyChanged("BankBik");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public string CorrAccount
        {
            get
            {
                return this.corrAccountField;
            }
            set
            {
                this.corrAccountField = value;
                this.RaisePropertyChanged("CorrAccount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public string SetAccount
        {
            get
            {
                return this.setAccountField;
            }
            set
            {
                this.setAccountField = value;
                this.RaisePropertyChanged("SetAccount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
                this.RaisePropertyChanged("Phone");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public string Fax
        {
            get
            {
                return this.faxField;
            }
            set
            {
                this.faxField = value;
                this.RaisePropertyChanged("Fax");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 28)]
        public string EMail
        {
            get
            {
                return this.eMailField;
            }
            set
            {
                this.eMailField = value;
                this.RaisePropertyChanged("EMail");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 29)]
        public string WebSite
        {
            get
            {
                return this.webSiteField;
            }
            set
            {
                this.webSiteField = value;
                this.RaisePropertyChanged("WebSite");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class ErrorMessageData : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorCodeField;

        private string errorTextField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ErrorText
        {
            get
            {
                return this.errorTextField;
            }
            set
            {
                this.errorTextField = value;
                this.RaisePropertyChanged("ErrorText");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class CoordinateStatusData : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string requestIdField;

        private System.Nullable<System.DateTime> responseDateField;

        private System.Nullable<System.DateTime> planDateField;

        private int statusCodeField;

        private System.DateTime statusDateField;

        private Person responsibleField;

        private ServiceDocument[] documentsField;

        private RequestContact[] contactsField;

        private string noteField;

        private RequestResult resultField;

        private string serviceNumberField;

        private string reasonCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string RequestId
        {
            get
            {
                return this.requestIdField;
            }
            set
            {
                this.requestIdField = value;
                this.RaisePropertyChanged("RequestId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public System.Nullable<System.DateTime> ResponseDate
        {
            get
            {
                return this.responseDateField;
            }
            set
            {
                this.responseDateField = value;
                this.RaisePropertyChanged("ResponseDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 2)]
        public System.Nullable<System.DateTime> PlanDate
        {
            get
            {
                return this.planDateField;
            }
            set
            {
                this.planDateField = value;
                this.RaisePropertyChanged("PlanDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int StatusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
                this.RaisePropertyChanged("StatusCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public System.DateTime StatusDate
        {
            get
            {
                return this.statusDateField;
            }
            set
            {
                this.statusDateField = value;
                this.RaisePropertyChanged("StatusDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public Person Responsible
        {
            get
            {
                return this.responsibleField;
            }
            set
            {
                this.responsibleField = value;
                this.RaisePropertyChanged("Responsible");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
        public ServiceDocument[] Documents
        {
            get
            {
                return this.documentsField;
            }
            set
            {
                this.documentsField = value;
                this.RaisePropertyChanged("Documents");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 7)]
        public RequestContact[] Contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
                this.RaisePropertyChanged("Contacts");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string Note
        {
            get
            {
                return this.noteField;
            }
            set
            {
                this.noteField = value;
                this.RaisePropertyChanged("Note");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public RequestResult Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string ServiceNumber
        {
            get
            {
                return this.serviceNumberField;
            }
            set
            {
                this.serviceNumberField = value;
                this.RaisePropertyChanged("ServiceNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string ReasonCode
        {
            get
            {
                return this.reasonCodeField;
            }
            set
            {
                this.reasonCodeField = value;
                this.RaisePropertyChanged("ReasonCode");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class DocumentsRequestData : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string documentTypeCodeField;

        private string parameterTypeCodeField;

        private System.Xml.XmlElement parameterField;

        private bool includeXmlViewField;

        private bool includeBinaryViewField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string DocumentTypeCode
        {
            get
            {
                return this.documentTypeCodeField;
            }
            set
            {
                this.documentTypeCodeField = value;
                this.RaisePropertyChanged("DocumentTypeCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ParameterTypeCode
        {
            get
            {
                return this.parameterTypeCodeField;
            }
            set
            {
                this.parameterTypeCodeField = value;
                this.RaisePropertyChanged("ParameterTypeCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.Xml.XmlElement Parameter
        {
            get
            {
                return this.parameterField;
            }
            set
            {
                this.parameterField = value;
                this.RaisePropertyChanged("Parameter");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public bool IncludeXmlView
        {
            get
            {
                return this.includeXmlViewField;
            }
            set
            {
                this.includeXmlViewField = value;
                this.RaisePropertyChanged("IncludeXmlView");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public bool IncludeBinaryView
        {
            get
            {
                return this.includeBinaryViewField;
            }
            set
            {
                this.includeBinaryViewField = value;
                this.RaisePropertyChanged("IncludeBinaryView");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class RequestTask : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string requestIdField;

        private string codeField;

        private string subjectField;

        private System.Nullable<System.DateTime> validityPeriodField;

        private Person responsibleField;

        private Department departmentField;

        private string serviceNumberField;

        private string serviceTypeCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string RequestId
        {
            get
            {
                return this.requestIdField;
            }
            set
            {
                this.requestIdField = value;
                this.RaisePropertyChanged("RequestId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
                this.RaisePropertyChanged("Code");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
                this.RaisePropertyChanged("Subject");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 3)]
        public System.Nullable<System.DateTime> ValidityPeriod
        {
            get
            {
                return this.validityPeriodField;
            }
            set
            {
                this.validityPeriodField = value;
                this.RaisePropertyChanged("ValidityPeriod");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public Person Responsible
        {
            get
            {
                return this.responsibleField;
            }
            set
            {
                this.responsibleField = value;
                this.RaisePropertyChanged("Responsible");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public Department Department
        {
            get
            {
                return this.departmentField;
            }
            set
            {
                this.departmentField = value;
                this.RaisePropertyChanged("Department");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ServiceNumber
        {
            get
            {
                return this.serviceNumberField;
            }
            set
            {
                this.serviceNumberField = value;
                this.RaisePropertyChanged("ServiceNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string ServiceTypeCode
        {
            get
            {
                return this.serviceTypeCodeField;
            }
            set
            {
                this.serviceTypeCodeField = value;
                this.RaisePropertyChanged("ServiceTypeCode");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class CoordinateTaskData : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RequestTask taskField;

        private DocumentsRequestData dataField;

        private object signatureField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public RequestTask Task
        {
            get
            {
                return this.taskField;
            }
            set
            {
                this.taskField = value;
                this.RaisePropertyChanged("Task");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DocumentsRequestData Data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
                this.RaisePropertyChanged("Data");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public object Signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
                this.RaisePropertyChanged("Signature");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class Headers : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string fromOrgCodeField;

        private string toOrgCodeField;

        private string messageIdField;

        private string relatesToField;

        private string serviceNumberField;

        private System.DateTime requestDateTimeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string FromOrgCode
        {
            get
            {
                return this.fromOrgCodeField;
            }
            set
            {
                this.fromOrgCodeField = value;
                this.RaisePropertyChanged("FromOrgCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ToOrgCode
        {
            get
            {
                return this.toOrgCodeField;
            }
            set
            {
                this.toOrgCodeField = value;
                this.RaisePropertyChanged("ToOrgCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string MessageId
        {
            get
            {
                return this.messageIdField;
            }
            set
            {
                this.messageIdField = value;
                this.RaisePropertyChanged("MessageId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string RelatesTo
        {
            get
            {
                return this.relatesToField;
            }
            set
            {
                this.relatesToField = value;
                this.RaisePropertyChanged("RelatesTo");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string ServiceNumber
        {
            get
            {
                return this.serviceNumberField;
            }
            set
            {
                this.serviceNumberField = value;
                this.RaisePropertyChanged("ServiceNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public System.DateTime RequestDateTime
        {
            get
            {
                return this.requestDateTimeField;
            }
            set
            {
                this.requestDateTimeField = value;
                this.RaisePropertyChanged("RequestDateTime");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Меняет местами организации и Id сообщений,
        /// MessageId -> RelatesTo, 
        /// Новый MessageId = SequentialGuid.NextString()
        /// </summary>
        /// <returns></returns>
        public Headers Invert()
        {
            return new Headers
            {
                FromOrgCode = ToOrgCode,
                ToOrgCode = FromOrgCode,
                RelatesTo = MessageId,
                MessageId = SequentialGuid.NextString(),
                ServiceNumber = ServiceNumber,
                RequestDateTime = DateTime.Now
            };
        }
    }

    public class SequentialGuid
    {
        private static readonly DateTime Initial = new DateTime(1970, 1, 1);

        public static Guid Next()
        {
            byte[] uid = Guid.NewGuid().ToByteArray();
            byte[] binDate = BitConverter.GetBytes((DateTime.Now - Initial).Ticks);
            byte[] comb = new byte[uid.Length];

            // the first 7 bytes are random - if two combs
            // are generated at the same point in time
            // they are not guaranteed to be sequential.
            // But for every DateTime.Tick there are
            // 72,057,594,037,927,935 unique possibilities so
            // there shouldn't be any collisions
            comb[3] = uid[0];
            comb[2] = uid[1];
            comb[1] = uid[2];
            comb[0] = uid[3];
            comb[5] = uid[4];
            comb[4] = uid[5];
            comb[7] = uid[6];

            // set the first 'nibble of the 7th byte to '1100' so 
            // later we can validate it was generated by us
            comb[6] = (byte)(0xc0 | (0xf & uid[7]));

            // the last 8 bytes are sequential,
            // these will reduce index fragmentation
            // to a degree as long as there are not a large
            // number of Combs generated per millisecond
            comb[9] = binDate[0];
            comb[8] = binDate[1];
            comb[15] = binDate[2];
            comb[14] = binDate[3];
            comb[13] = binDate[4];
            comb[12] = binDate[5];
            comb[11] = binDate[6];
            comb[10] = binDate[7];

            return new Guid(comb);
        }

        public static string NextString()
        {
            return Next().ToString();
        }

        public static implicit operator string(SequentialGuid f)
        {
            return f.ToString();
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class RequestService : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string regNumField;

        private System.Nullable<System.DateTime> regDateField;

        private string serviceNumberField;

        private string serviceTypeCodeField;

        private System.Nullable<decimal> servicePriceField;

        private System.Nullable<System.DateTime> prepareTargetDateField;

        private System.Nullable<System.DateTime> outputTargetDateField;

        private System.Nullable<int> copiesField;

        private Person responsibleField;

        private Department departmentField;

        private ServiceDocument[] documentsField;

        private string[] declineReasonCodesField;

        private Department createdByDepartmentField;

        private System.Nullable<System.DateTime> prepareFactDateField;

        private System.Nullable<System.DateTime> outputFactDateField;

        private System.Nullable<OutputKindType> outputKindField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string RegNum
        {
            get
            {
                return this.regNumField;
            }
            set
            {
                this.regNumField = value;
                this.RaisePropertyChanged("RegNum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public System.Nullable<System.DateTime> RegDate
        {
            get
            {
                return this.regDateField;
            }
            set
            {
                this.regDateField = value;
                this.RaisePropertyChanged("RegDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ServiceNumber
        {
            get
            {
                return this.serviceNumberField;
            }
            set
            {
                this.serviceNumberField = value;
                this.RaisePropertyChanged("ServiceNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ServiceTypeCode
        {
            get
            {
                return this.serviceTypeCodeField;
            }
            set
            {
                this.serviceTypeCodeField = value;
                this.RaisePropertyChanged("ServiceTypeCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public System.Nullable<decimal> ServicePrice
        {
            get
            {
                return this.servicePriceField;
            }
            set
            {
                this.servicePriceField = value;
                this.RaisePropertyChanged("ServicePrice");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 5)]
        public System.Nullable<System.DateTime> PrepareTargetDate
        {
            get
            {
                return this.prepareTargetDateField;
            }
            set
            {
                this.prepareTargetDateField = value;
                this.RaisePropertyChanged("PrepareTargetDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 6)]
        public System.Nullable<System.DateTime> OutputTargetDate
        {
            get
            {
                return this.outputTargetDateField;
            }
            set
            {
                this.outputTargetDateField = value;
                this.RaisePropertyChanged("OutputTargetDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 7)]
        public System.Nullable<int> Copies
        {
            get
            {
                return this.copiesField;
            }
            set
            {
                this.copiesField = value;
                this.RaisePropertyChanged("Copies");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public Person Responsible
        {
            get
            {
                return this.responsibleField;
            }
            set
            {
                this.responsibleField = value;
                this.RaisePropertyChanged("Responsible");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public Department Department
        {
            get
            {
                return this.departmentField;
            }
            set
            {
                this.departmentField = value;
                this.RaisePropertyChanged("Department");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 10)]
        public ServiceDocument[] Documents
        {
            get
            {
                return this.documentsField;
            }
            set
            {
                this.documentsField = value;
                this.RaisePropertyChanged("Documents");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 11)]
        public string[] DeclineReasonCodes
        {
            get
            {
                return this.declineReasonCodesField;
            }
            set
            {
                this.declineReasonCodesField = value;
                this.RaisePropertyChanged("DeclineReasonCodes");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public Department CreatedByDepartment
        {
            get
            {
                return this.createdByDepartmentField;
            }
            set
            {
                this.createdByDepartmentField = value;
                this.RaisePropertyChanged("CreatedByDepartment");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 13)]
        public System.Nullable<System.DateTime> PrepareFactDate
        {
            get
            {
                return this.prepareFactDateField;
            }
            set
            {
                this.prepareFactDateField = value;
                this.RaisePropertyChanged("PrepareFactDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 14)]
        public System.Nullable<System.DateTime> OutputFactDate
        {
            get
            {
                return this.outputFactDateField;
            }
            set
            {
                this.outputFactDateField = value;
                this.RaisePropertyChanged("OutputFactDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 15)]
        public System.Nullable<OutputKindType> OutputKind
        {
            get
            {
                return this.outputKindField;
            }
            set
            {
                this.outputKindField = value;
                this.RaisePropertyChanged("OutputKind");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    
    public enum OutputKindType
    {

        /// <remarks/>
        Personally,

        /// <remarks/>
        Telephone,

        /// <remarks/>
        Portal,

        /// <remarks/>
        EMail,

        /// <remarks/>
        Mail,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class CoordinateData : object, System.ComponentModel.INotifyPropertyChanged
    {

        private BaseDeclarant declarantField;

        private RequestContact trusteeField;

        private RequestService serviceField;

        private System.Xml.XmlElement customAttributesField;

        private RequestContact[] contactsField;

        private byte[] signatureField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BaseDeclarant Declarant
        {
            get
            {
                return this.declarantField;
            }
            set
            {
                this.declarantField = value;
                this.RaisePropertyChanged("Declarant");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public RequestContact Trustee
        {
            get
            {
                return this.trusteeField;
            }
            set
            {
                this.trusteeField = value;
                this.RaisePropertyChanged("Trustee");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public RequestService Service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
                this.RaisePropertyChanged("Service");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public System.Xml.XmlElement CustomAttributes
        {
            get
            {
                return this.customAttributesField;
            }
            set
            {
                this.customAttributesField = value;
                this.RaisePropertyChanged("CustomAttributes");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        public RequestContact[] Contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
                this.RaisePropertyChanged("Contacts");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 5)]
        public byte[] Signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
                this.RaisePropertyChanged("Signature");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class RequestQueryTask : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string requestIdField;

        private string subjectField;

        private System.Nullable<System.DateTime> validityPeriodField;

        private int statusCodeField;

        private Person responsibleField;

        private Department departmentField;

        private string docCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string RequestId
        {
            get
            {
                return this.requestIdField;
            }
            set
            {
                this.requestIdField = value;
                this.RaisePropertyChanged("RequestId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
                this.RaisePropertyChanged("Subject");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 2)]
        public System.Nullable<System.DateTime> ValidityPeriod
        {
            get
            {
                return this.validityPeriodField;
            }
            set
            {
                this.validityPeriodField = value;
                this.RaisePropertyChanged("ValidityPeriod");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int StatusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
                this.RaisePropertyChanged("StatusCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public Person Responsible
        {
            get
            {
                return this.responsibleField;
            }
            set
            {
                this.responsibleField = value;
                this.RaisePropertyChanged("Responsible");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public Department Department
        {
            get
            {
                return this.departmentField;
            }
            set
            {
                this.departmentField = value;
                this.RaisePropertyChanged("Department");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string DocCode
        {
            get
            {
                return this.docCodeField;
            }
            set
            {
                this.docCodeField = value;
                this.RaisePropertyChanged("DocCode");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class ServiceDocumentNoFiles : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string idField;

        private string docCodeField;

        private string docSubTypeField;

        private string docPersonField;

        private string docSerieField;

        private string docNumberField;

        private System.Nullable<System.DateTime> docDateField;

        private System.Nullable<System.DateTime> validityPeriodField;

        private string whoSignField;

        private System.Nullable<int> listCountField;

        private System.Nullable<int> copyCountField;

        private string divisionCodeField;

        private byte[] signatureField;

        private System.Xml.XmlElement customAttributesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("Id");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string DocCode
        {
            get
            {
                return this.docCodeField;
            }
            set
            {
                this.docCodeField = value;
                this.RaisePropertyChanged("DocCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string DocSubType
        {
            get
            {
                return this.docSubTypeField;
            }
            set
            {
                this.docSubTypeField = value;
                this.RaisePropertyChanged("DocSubType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string DocPerson
        {
            get
            {
                return this.docPersonField;
            }
            set
            {
                this.docPersonField = value;
                this.RaisePropertyChanged("DocPerson");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string DocSerie
        {
            get
            {
                return this.docSerieField;
            }
            set
            {
                this.docSerieField = value;
                this.RaisePropertyChanged("DocSerie");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string DocNumber
        {
            get
            {
                return this.docNumberField;
            }
            set
            {
                this.docNumberField = value;
                this.RaisePropertyChanged("DocNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 6)]
        public System.Nullable<System.DateTime> DocDate
        {
            get
            {
                return this.docDateField;
            }
            set
            {
                this.docDateField = value;
                this.RaisePropertyChanged("DocDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 7)]
        public System.Nullable<System.DateTime> ValidityPeriod
        {
            get
            {
                return this.validityPeriodField;
            }
            set
            {
                this.validityPeriodField = value;
                this.RaisePropertyChanged("ValidityPeriod");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string WhoSign
        {
            get
            {
                return this.whoSignField;
            }
            set
            {
                this.whoSignField = value;
                this.RaisePropertyChanged("WhoSign");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 9)]
        public System.Nullable<int> ListCount
        {
            get
            {
                return this.listCountField;
            }
            set
            {
                this.listCountField = value;
                this.RaisePropertyChanged("ListCount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 10)]
        public System.Nullable<int> CopyCount
        {
            get
            {
                return this.copyCountField;
            }
            set
            {
                this.copyCountField = value;
                this.RaisePropertyChanged("CopyCount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string DivisionCode
        {
            get
            {
                return this.divisionCodeField;
            }
            set
            {
                this.divisionCodeField = value;
                this.RaisePropertyChanged("DivisionCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 12)]
        public byte[] Signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
                this.RaisePropertyChanged("Signature");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public System.Xml.XmlElement CustomAttributes
        {
            get
            {
                return this.customAttributesField;
            }
            set
            {
                this.customAttributesField = value;
                this.RaisePropertyChanged("CustomAttributes");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class RequestServiceNoFiles : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string regNumField;

        private System.Nullable<System.DateTime> regDateField;

        private string serviceNumberField;

        private string serviceTypeCodeField;

        private System.Nullable<decimal> servicePriceField;

        private System.Nullable<System.DateTime> prepareTargetDateField;

        private System.Nullable<System.DateTime> outputTargetDateField;

        private System.Nullable<int> copiesField;

        private Person responsibleField;

        private Department departmentField;

        private ServiceDocumentNoFiles[] documentsField;

        private string[] declineReasonCodesField;

        private Department createdByDepartmentField;

        private System.Nullable<System.DateTime> prepareFactDateField;

        private System.Nullable<System.DateTime> outputFactDateField;

        private System.Nullable<OutputKindType> outputKindField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string RegNum
        {
            get
            {
                return this.regNumField;
            }
            set
            {
                this.regNumField = value;
                this.RaisePropertyChanged("RegNum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public System.Nullable<System.DateTime> RegDate
        {
            get
            {
                return this.regDateField;
            }
            set
            {
                this.regDateField = value;
                this.RaisePropertyChanged("RegDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ServiceNumber
        {
            get
            {
                return this.serviceNumberField;
            }
            set
            {
                this.serviceNumberField = value;
                this.RaisePropertyChanged("ServiceNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ServiceTypeCode
        {
            get
            {
                return this.serviceTypeCodeField;
            }
            set
            {
                this.serviceTypeCodeField = value;
                this.RaisePropertyChanged("ServiceTypeCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public System.Nullable<decimal> ServicePrice
        {
            get
            {
                return this.servicePriceField;
            }
            set
            {
                this.servicePriceField = value;
                this.RaisePropertyChanged("ServicePrice");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 5)]
        public System.Nullable<System.DateTime> PrepareTargetDate
        {
            get
            {
                return this.prepareTargetDateField;
            }
            set
            {
                this.prepareTargetDateField = value;
                this.RaisePropertyChanged("PrepareTargetDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 6)]
        public System.Nullable<System.DateTime> OutputTargetDate
        {
            get
            {
                return this.outputTargetDateField;
            }
            set
            {
                this.outputTargetDateField = value;
                this.RaisePropertyChanged("OutputTargetDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 7)]
        public System.Nullable<int> Copies
        {
            get
            {
                return this.copiesField;
            }
            set
            {
                this.copiesField = value;
                this.RaisePropertyChanged("Copies");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public Person Responsible
        {
            get
            {
                return this.responsibleField;
            }
            set
            {
                this.responsibleField = value;
                this.RaisePropertyChanged("Responsible");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public Department Department
        {
            get
            {
                return this.departmentField;
            }
            set
            {
                this.departmentField = value;
                this.RaisePropertyChanged("Department");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 10)]
        public ServiceDocumentNoFiles[] Documents
        {
            get
            {
                return this.documentsField;
            }
            set
            {
                this.documentsField = value;
                this.RaisePropertyChanged("Documents");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 11)]
        public string[] DeclineReasonCodes
        {
            get
            {
                return this.declineReasonCodesField;
            }
            set
            {
                this.declineReasonCodesField = value;
                this.RaisePropertyChanged("DeclineReasonCodes");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public Department CreatedByDepartment
        {
            get
            {
                return this.createdByDepartmentField;
            }
            set
            {
                this.createdByDepartmentField = value;
                this.RaisePropertyChanged("CreatedByDepartment");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 13)]
        public System.Nullable<System.DateTime> PrepareFactDate
        {
            get
            {
                return this.prepareFactDateField;
            }
            set
            {
                this.prepareFactDateField = value;
                this.RaisePropertyChanged("PrepareFactDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 14)]
        public System.Nullable<System.DateTime> OutputFactDate
        {
            get
            {
                return this.outputFactDateField;
            }
            set
            {
                this.outputFactDateField = value;
                this.RaisePropertyChanged("OutputFactDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 15)]
        public System.Nullable<OutputKindType> OutputKind
        {
            get
            {
                return this.outputKindField;
            }
            set
            {
                this.outputKindField = value;
                this.RaisePropertyChanged("OutputKind");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class RequestInfo : object, System.ComponentModel.INotifyPropertyChanged
    {

        private BaseDeclarant declarantField;

        private RequestContact trusteeField;

        private RequestServiceNoFiles serviceField;

        private RequestStatus[] statusesField;

        private RequestQueryTask[] tasksField;

        private System.Xml.XmlElement customAttributesField;

        private RequestContact[] contactsField;

        private byte[] signatureField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BaseDeclarant Declarant
        {
            get
            {
                return this.declarantField;
            }
            set
            {
                this.declarantField = value;
                this.RaisePropertyChanged("Declarant");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public RequestContact Trustee
        {
            get
            {
                return this.trusteeField;
            }
            set
            {
                this.trusteeField = value;
                this.RaisePropertyChanged("Trustee");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public RequestServiceNoFiles Service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
                this.RaisePropertyChanged("Service");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Statuses", Order = 3)]
        public RequestStatus[] Statuses
        {
            get
            {
                return this.statusesField;
            }
            set
            {
                this.statusesField = value;
                this.RaisePropertyChanged("Statuses");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Tasks", Order = 4)]
        public RequestQueryTask[] Tasks
        {
            get
            {
                return this.tasksField;
            }
            set
            {
                this.tasksField = value;
                this.RaisePropertyChanged("Tasks");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public System.Xml.XmlElement CustomAttributes
        {
            get
            {
                return this.customAttributesField;
            }
            set
            {
                this.customAttributesField = value;
                this.RaisePropertyChanged("CustomAttributes");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Contacts", Order = 6)]
        public RequestContact[] Contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
                this.RaisePropertyChanged("Contacts");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 7)]
        public byte[] Signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
                this.RaisePropertyChanged("Signature");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class ServiceNumberStatusesOnly : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string serviceNumberField;

        private bool statusesOnlyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ServiceNumber
        {
            get
            {
                return this.serviceNumberField;
            }
            set
            {
                this.serviceNumberField = value;
                this.RaisePropertyChanged("ServiceNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool StatusesOnly
        {
            get
            {
                return this.statusesOnlyField;
            }
            set
            {
                this.statusesOnlyField = value;
                this.RaisePropertyChanged("StatusesOnly");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class SmallRequestInfo : object, System.ComponentModel.INotifyPropertyChanged
    {

        private System.DateTime createdDateField;

        private string serviceCodeField;

        private string serviceNumberField;

        private int statusCodeField;

        private System.DateTime statusDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public System.DateTime CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
                this.RaisePropertyChanged("CreatedDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ServiceCode
        {
            get
            {
                return this.serviceCodeField;
            }
            set
            {
                this.serviceCodeField = value;
                this.RaisePropertyChanged("ServiceCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ServiceNumber
        {
            get
            {
                return this.serviceNumberField;
            }
            set
            {
                this.serviceNumberField = value;
                this.RaisePropertyChanged("ServiceNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int StatusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
                this.RaisePropertyChanged("StatusCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public System.DateTime StatusDate
        {
            get
            {
                return this.statusDateField;
            }
            set
            {
                this.statusDateField = value;
                this.RaisePropertyChanged("StatusDate");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class GetRequestsOutMessage : MessageBase
    {

        private RequestInfo[] requestsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Request")]
        public RequestInfo[] Requests
        {
            get
            {
                return this.requestsField;
            }
            set
            {
                this.requestsField = value;
                this.RaisePropertyChanged("Requests");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class GetRequestsInMessage : MessageBase
    {

        private System.DateTime fromDateField;

        private System.DateTime toDateField;

        private string serviceCodeField;

        private ServiceNumberStatusesOnly[] serviceNumbersField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public System.DateTime FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
                this.RaisePropertyChanged("FromDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
                this.RaisePropertyChanged("ToDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ServiceCode
        {
            get
            {
                return this.serviceCodeField;
            }
            set
            {
                this.serviceCodeField = value;
                this.RaisePropertyChanged("ServiceCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        public ServiceNumberStatusesOnly[] ServiceNumbers
        {
            get
            {
                return this.serviceNumbersField;
            }
            set
            {
                this.serviceNumbersField = value;
                this.RaisePropertyChanged("ServiceNumbers");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    
    public partial class GetRequestListOutMessage : MessageBase
    {

        private SmallRequestInfo[] requestsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Request")]
        public SmallRequestInfo[] Requests
        {
            get
            {
                return this.requestsField;
            }
            set
            {
                this.requestsField = value;
                this.RaisePropertyChanged("Requests");
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CoordinateMessage
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = Namespace.ServiceNamespace)]
        public TM.Services.CoordinateV52.Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = Namespace.ServiceNamespace, Order = 0)]
        public TM.Services.CoordinateV52.CoordinateData Message;

        public CoordinateMessage()
        {
        }

        public CoordinateMessage(TM.Services.CoordinateV52.Headers ServiceHeader, TM.Services.CoordinateV52.CoordinateData Message)
        {
            this.ServiceHeader = ServiceHeader;
            this.Message = Message;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CoordinateTaskMessage
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = Namespace.ServiceNamespace)]
        public TM.Services.CoordinateV52.Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = Namespace.ServiceNamespace, Order = 0)]
        public TM.Services.CoordinateV52.CoordinateTaskData TaskMessage;

        public CoordinateTaskMessage()
        {
        }

        public CoordinateTaskMessage(TM.Services.CoordinateV52.Headers ServiceHeader, TM.Services.CoordinateV52.CoordinateTaskData TaskMessage)
        {
            this.ServiceHeader = ServiceHeader;
            this.TaskMessage = TaskMessage;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CoordinateStatusMessage
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = Namespace.ServiceNamespace)]
        public TM.Services.CoordinateV52.Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = Namespace.ServiceNamespace, Order = 0)]
        public TM.Services.CoordinateV52.CoordinateStatusData StatusMessage;

        public CoordinateStatusMessage()
        {
        }

        public CoordinateStatusMessage(TM.Services.CoordinateV52.Headers ServiceHeader, TM.Services.CoordinateV52.CoordinateStatusData StatusMessage)
        {
            this.ServiceHeader = ServiceHeader;
            this.StatusMessage = StatusMessage;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ErrorMessage", WrapperNamespace = Namespace.ServiceNamespace, IsWrapped = true)]
    public partial class ErrorMessage
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = Namespace.ServiceNamespace)]
        public TM.Services.CoordinateV52.Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = Namespace.ServiceNamespace, Order = 0)]
        public TM.Services.CoordinateV52.ErrorMessageData Error;

        public ErrorMessage()
        {
        }

        public ErrorMessage(TM.Services.CoordinateV52.Headers ServiceHeader, TM.Services.CoordinateV52.ErrorMessageData Error)
        {
            this.ServiceHeader = ServiceHeader;
            this.Error = Error;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SendRequestsMessage
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = Namespace.ServiceNamespace)]
        public TM.Services.CoordinateV52.Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = Namespace.ServiceNamespace, Order = 0)]
        public TM.Services.CoordinateV52.CoordinateData[] RequestsMessage;

        public SendRequestsMessage()
        {
        }

        public SendRequestsMessage(TM.Services.CoordinateV52.Headers ServiceHeader, TM.Services.CoordinateV52.CoordinateData[] RequestsMessage)
        {
            this.ServiceHeader = ServiceHeader;
            this.RequestsMessage = RequestsMessage;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SendTasksMessage
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = Namespace.ServiceNamespace)]
        public TM.Services.CoordinateV52.Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = Namespace.ServiceNamespace, Order = 0)]
        public TM.Services.CoordinateV52.CoordinateTaskData[] TasksMessage;

        public SendTasksMessage()
        {
        }

        public SendTasksMessage(TM.Services.CoordinateV52.Headers ServiceHeader, TM.Services.CoordinateV52.CoordinateTaskData[] TasksMessage)
        {
            this.ServiceHeader = ServiceHeader;
            this.TasksMessage = TasksMessage;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SetFilesAndStatusesMessage
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = Namespace.ServiceNamespace)]
        public TM.Services.CoordinateV52.Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = Namespace.ServiceNamespace, Order = 0)]
        public TM.Services.CoordinateV52.SetFilesAndStatusesData[] StatusesMessage;

        public SetFilesAndStatusesMessage()
        {
        }

        public SetFilesAndStatusesMessage(TM.Services.CoordinateV52.Headers ServiceHeader, TM.Services.CoordinateV52.SetFilesAndStatusesData[] StatusesMessage)
        {
            this.ServiceHeader = ServiceHeader;
            this.StatusesMessage = StatusesMessage;
        }
    }
}