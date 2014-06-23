using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TM.Services.CoordinateV5
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
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
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetRequestsOutMessage))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetRequestsInMessage))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetRequestListOutMessage))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetRequestListInMessage))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public abstract partial class MessageBase
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class SetFilesAndStatusesData
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class RequestStatus
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class Person
    {

        private string lastNameField;

        private string firstNameField;

        private string middleNameField;

        private string jobTitleField;

        private string phoneField;

        private string emailField;

        private string isiIdField;

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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class Department
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class RequestResult
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class ServiceDocument
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class Note
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class File
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
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

        private Address birthAddressField;

        private string jobTitleField;

        private string oMSNumField;

        private System.Nullable<System.DateTime> oMSDateField;

        private string oMSCompanyField;

        private System.Nullable<System.DateTime> oMSValidityPeriodField;

        private string isiIdField;

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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public Address BirthAddress
        {
            get
            {
                return this.birthAddressField;
            }
            set
            {
                this.birthAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string JobTitle
        {
            get
            {
                return this.jobTitleField;
            }
            set
            {
                this.jobTitleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public string OMSNum
        {
            get
            {
                return this.oMSNumField;
            }
            set
            {
                this.oMSNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 20)]
        public System.Nullable<System.DateTime> OMSDate
        {
            get
            {
                return this.oMSDateField;
            }
            set
            {
                this.oMSDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string OMSCompany
        {
            get
            {
                return this.oMSCompanyField;
            }
            set
            {
                this.oMSCompanyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 22)]
        public System.Nullable<System.DateTime> OMSValidityPeriod
        {
            get
            {
                return this.oMSValidityPeriodField;
            }
            set
            {
                this.oMSValidityPeriodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string IsiId
        {
            get
            {
                return this.isiIdField;
            }
            set
            {
                this.isiIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public enum GenderType
    {

        /// <remarks/>
        Male,

        /// <remarks/>
        Female,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class Address
    {

        private string countryField;

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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Locality
        {
            get
            {
                return this.localityField;
            }
            set
            {
                this.localityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Town
        {
            get
            {
                return this.townField;
            }
            set
            {
                this.townField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string House
        {
            get
            {
                return this.houseField;
            }
            set
            {
                this.houseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string Building
        {
            get
            {
                return this.buildingField;
            }
            set
            {
                this.buildingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string Facility
        {
            get
            {
                return this.facilityField;
            }
            set
            {
                this.facilityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string Ownership
        {
            get
            {
                return this.ownershipField;
            }
            set
            {
                this.ownershipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string Flat
        {
            get
            {
                return this.flatField;
            }
            set
            {
                this.flatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public string POBox
        {
            get
            {
                return this.pOBoxField;
            }
            set
            {
                this.pOBoxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string Okato
        {
            get
            {
                return this.okatoField;
            }
            set
            {
                this.okatoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string KladrCode
        {
            get
            {
                return this.kladrCodeField;
            }
            set
            {
                this.kladrCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string KladrStreetCode
        {
            get
            {
                return this.kladrStreetCodeField;
            }
            set
            {
                this.kladrStreetCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public string OMKDistrictCode
        {
            get
            {
                return this.oMKDistrictCodeField;
            }
            set
            {
                this.oMKDistrictCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string OMKRegionCode
        {
            get
            {
                return this.oMKRegionCodeField;
            }
            set
            {
                this.oMKRegionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public string OMKTownCode
        {
            get
            {
                return this.oMKTownCodeField;
            }
            set
            {
                this.oMKTownCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public string OMKStreetCode
        {
            get
            {
                return this.oMKStreetCodeField;
            }
            set
            {
                this.oMKStreetCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string BTIStreetCode
        {
            get
            {
                return this.bTIStreetCodeField;
            }
            set
            {
                this.bTIStreetCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public string BTIBuildingCode
        {
            get
            {
                return this.bTIBuildingCodeField;
            }
            set
            {
                this.bTIBuildingCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public abstract partial class BaseDeclarant
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class RequestAccount : BaseDeclarant
    {

        private string fullNameField;

        private string nameField;

        private string brandNameField;

        private string ogrnField;

        private string ogrnAuthorityField;

        private string ogrnNumField;

        private System.Nullable<System.DateTime> ogrnDateField;

        private string innField;

        private string innAuthorityField;

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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string OgrnAuthority
        {
            get
            {
                return this.ogrnAuthorityField;
            }
            set
            {
                this.ogrnAuthorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string OgrnNum
        {
            get
            {
                return this.ogrnNumField;
            }
            set
            {
                this.ogrnNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 6)]
        public System.Nullable<System.DateTime> OgrnDate
        {
            get
            {
                return this.ogrnDateField;
            }
            set
            {
                this.ogrnDateField = value;
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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string InnAuthority
        {
            get
            {
                return this.innAuthorityField;
            }
            set
            {
                this.innAuthorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string InnNum
        {
            get
            {
                return this.innNumField;
            }
            set
            {
                this.innNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 10)]
        public System.Nullable<System.DateTime> InnDate
        {
            get
            {
                return this.innDateField;
            }
            set
            {
                this.innDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string Kpp
        {
            get
            {
                return this.kppField;
            }
            set
            {
                this.kppField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string Okpo
        {
            get
            {
                return this.okpoField;
            }
            set
            {
                this.okpoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public string OrgFormCode
        {
            get
            {
                return this.orgFormCodeField;
            }
            set
            {
                this.orgFormCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public Address PostalAddress
        {
            get
            {
                return this.postalAddressField;
            }
            set
            {
                this.postalAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public Address FactAddress
        {
            get
            {
                return this.factAddressField;
            }
            set
            {
                this.factAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public RequestContact OrgHead
        {
            get
            {
                return this.orgHeadField;
            }
            set
            {
                this.orgHeadField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public string Okved
        {
            get
            {
                return this.okvedField;
            }
            set
            {
                this.okvedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string Okfs
        {
            get
            {
                return this.okfsField;
            }
            set
            {
                this.okfsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public string BankName
        {
            get
            {
                return this.bankNameField;
            }
            set
            {
                this.bankNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public string BankBik
        {
            get
            {
                return this.bankBikField;
            }
            set
            {
                this.bankBikField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string CorrAccount
        {
            get
            {
                return this.corrAccountField;
            }
            set
            {
                this.corrAccountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public string SetAccount
        {
            get
            {
                return this.setAccountField;
            }
            set
            {
                this.setAccountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public string Fax
        {
            get
            {
                return this.faxField;
            }
            set
            {
                this.faxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public string EMail
        {
            get
            {
                return this.eMailField;
            }
            set
            {
                this.eMailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public string WebSite
        {
            get
            {
                return this.webSiteField;
            }
            set
            {
                this.webSiteField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class ErrorMessageData
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class CoordinateStatusData
    {

        private string requestIdField;

        private System.Nullable<System.DateTime> responseDateField;

        private System.Nullable<System.DateTime> planDateField;

        private System.Nullable<int> statusCodeField;

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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 3)]
        public System.Nullable<int> StatusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        public ServiceDocument[] Documents
        {
            get
            {
                return this.documentsField;
            }
            set
            {
                this.documentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
        public RequestContact[] Contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string Note
        {
            get
            {
                return this.noteField;
            }
            set
            {
                this.noteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public RequestResult Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string ServiceNumber
        {
            get
            {
                return this.serviceNumberField;
            }
            set
            {
                this.serviceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string ReasonCode
        {
            get
            {
                return this.reasonCodeField;
            }
            set
            {
                this.reasonCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class DocumentsRequestData
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class RequestTask
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class CoordinateTaskData
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class Headers
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class RequestService
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class CoordinateData
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class RequestQueryTask
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class ServiceDocumentNoFiles
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class RequestServiceNoFiles
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class RequestInfo
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class ServiceNumberStatusesOnly
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
    public partial class SmallRequestInfo
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
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
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
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
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]

    public partial class CoordinateMessage
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
        public Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/", Order = 0)]
        public CoordinateData Message;

        public CoordinateMessage()
        {
        }

        public CoordinateMessage(Headers ServiceHeader, CoordinateData Message)
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

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
        public Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/", Order = 0)]
        public CoordinateTaskData TaskMessage;

        public CoordinateTaskMessage()
        {
        }

        public CoordinateTaskMessage(Headers ServiceHeader, CoordinateTaskData TaskMessage)
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

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
        public Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/", Order = 0)]
        public CoordinateStatusData StatusMessage;

        public CoordinateStatusMessage()
        {
        }

        public CoordinateStatusMessage(Headers ServiceHeader, CoordinateStatusData StatusMessage)
        {
            this.ServiceHeader = ServiceHeader;
            this.StatusMessage = StatusMessage;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ErrorMessage", WrapperNamespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/", IsWrapped = true)]
    public partial class ErrorMessage
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
        public Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/", Order = 0)]
        public ErrorMessageData Error;

        public ErrorMessage()
        {
        }

        public ErrorMessage(Headers ServiceHeader, ErrorMessageData Error)
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

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
        public Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/", Order = 0)]
        public CoordinateData[] RequestsMessage;

        public SendRequestsMessage()
        {
        }

        public SendRequestsMessage(Headers ServiceHeader, CoordinateData[] RequestsMessage)
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

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
        public Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/", Order = 0)]
        public CoordinateTaskData[] TasksMessage;

        public SendTasksMessage()
        {
        }

        public SendTasksMessage(Headers ServiceHeader, CoordinateTaskData[] TasksMessage)
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

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/")]
        public Headers ServiceHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/", Order = 0)]
        public SetFilesAndStatusesData[] StatusesMessage;

        public SetFilesAndStatusesMessage()
        {
        }

        public SetFilesAndStatusesMessage(Headers ServiceHeader, SetFilesAndStatusesData[] StatusesMessage)
        {
            this.ServiceHeader = ServiceHeader;
            this.StatusesMessage = StatusesMessage;
        }
    }
}