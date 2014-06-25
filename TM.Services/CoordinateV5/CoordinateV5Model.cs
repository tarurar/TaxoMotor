using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TM.Services.CoordinateV5
{
    [MessageContract]
    public abstract class MessageBase
    {
        
    }

    [Serializable]
    [XmlRoot(ElementName = "GetRequestListOutMessage", Namespace = Namespace.ServiceNamespace)]
    public class GetRequestListOutMessage : MessageBase
    {
        [XmlArray("Requests")]
        [XmlArrayItem("Request")]
        public SmallRequestInfo[] Requests { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "GetRequestListInMessage")]
    public class GetRequestListInMessage : MessageBase
    {
        [XmlElement("FromDate", Order = 0)]
        public DateTime FromDate { get; set; }

        [XmlElement("ToDate", Order = 1)]
        public DateTime ToDate { get; set; }

        [XmlElement("ServiceCode", Order = 2)]
        public string ServiceCode { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "GetRequestsOutMessage", Namespace = Namespace.ServiceNamespace)]
    public class GetRequestsOutMessage : MessageBase
    {
        [XmlArray("Requests")]
        [XmlArrayItem("Request")]
        public RequestInfo[] Requests { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "GetRequestsInMessage")]
    public class GetRequestsInMessage : MessageBase
    {
        [XmlElement("FromDate", Order = 0)]
        public DateTime FromDate { get; set; }

        [XmlElement("ToDate", Order = 1)]
        public DateTime ToDate { get; set; }

        [XmlElement("ServiceCode", Order = 2)]
        public string ServiceCode { get; set; }

        [XmlArray("ServiceNumbers", Order = 3)]
        //[XmlArrayItem("ServiceNumber")]
        public ServiceNumberStatusesOnly[] ServiceNumbers { get; set; }
    }

    [MessageContract(IsWrapped = false)]
    public class CoordinateMessage : MessageBase
    {
        [MessageBodyMember]
        public CoordinateData Message { get; set; }

        [MessageHeader]
        public Headers ServiceHeader { get; set; }
    }

    [MessageContract(IsWrapped = false)]
    public class CoordinateTaskMessage : MessageBase
    {
        [MessageBodyMember]
        public CoordinateTaskData TaskMessage { get; set; }

        [MessageHeader]
        public Headers ServiceHeader { get; set; }
    }

    [MessageContract(IsWrapped = false)]
    public class CoordinateStatusMessage : MessageBase
    {
        [MessageBodyMember]
        public CoordinateStatusData StatusMessage { get; set; }

        [MessageHeader]
        public Headers ServiceHeader { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class ErrorMessage
    {
        [MessageHeader]
        public Headers ServiceHeader { get; set; }

        [MessageBodyMember]
        public ErrorMessageData Error { get; set; }
    }

    [MessageContract(IsWrapped = false)]
    public class SendRequestsMessage : MessageBase
    {
        [MessageBodyMember]
        public CoordinateData[] RequestsMessage { get; set; }

        [MessageHeader]
        public Headers ServiceHeader { get; set; }
    }

    [MessageContract(IsWrapped = false)]
    public class SendTasksMessage : MessageBase
    {
        [MessageBodyMember]
        public CoordinateTaskData[] TasksMessage { get; set; }

        [MessageHeader]
        public Headers ServiceHeader { get; set; }
    }

    [MessageContract(IsWrapped = false)]
    public class SetFilesAndStatusesMessage : MessageBase
    {
        [MessageBodyMember]
        public SetFilesAndStatusesData[] StatusesMessage { get; set; }

        [MessageHeader]
        public Headers ServiceHeader { get; set; }
    }

    public class SetFilesAndStatusesData
    {
        public string ServiceNumber { get; set; }

        public string RequestId { get; set; }

        public RequestStatus[] Statuses { get; set; }

        public RequestResult Result { get; set; }

        public ServiceDocument[] Documents { get; set; }

        public RequestContact[] Contacts { get; set; }
    }

    public class RequestContact : BaseDeclarant
    {
        [XmlElement(Order = 0)]
        public string Id { get; set; }

        [XmlElementAttribute(Order = 1)]
        public string LastName { get; set; }

        // Фамилия

        [XmlElementAttribute(Order = 2)]
        public string FirstName { get; set; }

        // Имя

        [XmlElementAttribute(Order = 3)]
        public string MiddleName { get; set; }

        // Отчество

        [XmlElementAttribute(Order = 4)]
        public GenderType? Gender { get; set; }

        [XmlElementAttribute(Order = 5)]
        public DateTime? BirthDate { get; set; }

        // Дата рождения

        [XmlElementAttribute(Order = 6)]
        public string Snils { get; set; }

        // СНИЛС

        [XmlElementAttribute(Order = 7)]
        public string Inn { get; set; }

        // ИНН

        [XmlElementAttribute(Order = 8)]
        public Address RegAddress { get; set; }

        // Адрес

        [XmlElementAttribute(Order = 9)]
        public Address FactAddress { get; set; }

        [XmlElementAttribute(Order = 10)]
        public string MobilePhone { get; set; }

        // Мобильный телефон

        [XmlElementAttribute(Order = 11)]
        public string WorkPhone { get; set; }

        // Рабочий телефон 

        [XmlElementAttribute(Order = 12)]
        public string HomePhone { get; set; }

        // Домашний телефон

        [XmlElementAttribute(Order = 13)]
        public string EMail { get; set; }

        // E-Mail

        [XmlElementAttribute(Order = 14)]
        public string Nation { get; set; }

        // Национальность

        [XmlElementAttribute(Order = 15)]
        public string Citizenship { get; set; }

        // Гражданство

        [XmlElementAttribute(Order = 16)]
        public CitizenshipType? CitizenshipType { get; set; }

        // Тип гражданства

        [XmlElementAttribute(Order = 17)]
        public Address BirthAddress { get; set; }

        // Адрес(место) рождения

        [XmlElementAttribute(Order = 18)]
        public string JobTitle { get; set; }

        // Должность

        [XmlElementAttribute(Order = 19)]
        public string OMSNum { get; set; }

        [XmlElementAttribute(Order = 20)]
        public DateTime? OMSDate { get; set; }

        [XmlElementAttribute(Order = 21)]
        public string OMSCompany { get; set; }

        [XmlElementAttribute(Order = 22)]
        public DateTime? OMSValidityPeriod { get; set; }

        [XmlElementAttribute(Order = 23)]
        public string IsiId { get; set; }
    }

    public class SmallRequestInfo
    {
        [XmlElement(Order = 0)]
        public DateTime CreatedDate { get; set; }
        [XmlElement(Order = 1)]
        public string ServiceCode { get; set; }
        [XmlElement(Order = 2)]
        public string ServiceNumber { get; set; }
        [XmlElement(Order = 3)]
        public int StatusCode { get; set; }
        [XmlElement(Order = 4)]
        public DateTime StatusDate { get; set; }
    }

    public class RequestInfo
    {
        [XmlElement(Order = 0)]
        public BaseDeclarant Declarant { get; set; }

        [XmlElement(Order = 1)]
        public RequestContact Trustee { get; set; }

        [XmlElement(Order = 2)]
        public RequestServiceNoFiles Service { get; set; }

        [XmlElement(Order = 3)]
        public RequestStatus[] Statuses { get; set; }

        [XmlElement(Order = 4)]
        public RequestQueryTask[] Tasks { get; set; }

        [XmlElement(Order = 5)]
        public XmlElement CustomAttributes { get; set; }

        [XmlElement(Order = 6)]
        public RequestContact[] Contacts { get; set; }

        [XmlElement(Order = 7)]
        public byte[] Signature { get; set; }
    }

    public class ServiceNumberStatusesOnly
    {
        [XmlElement(Order = 0)]
        public string ServiceNumber { get; set; }

        [XmlElement(Order = 1)]
        public bool StatusesOnly { get; set; }
    }

    [System.Xml.Serialization.XmlInclude(typeof(RequestContact))]
    [System.Xml.Serialization.XmlInclude(typeof(RequestAccount))]
    [KnownType(typeof(RequestAccount))]
    [KnownType(typeof(RequestContact))]
    public abstract class BaseDeclarant
    {

    }

    public class RequestAccount : BaseDeclarant
    {
        [XmlElement(Order = 0)]
        public string FullName { get; set; }

        [XmlElement(Order = 1)]
        public string Name { get; set; }

        [XmlElement(Order = 2)]
        public string BrandName { get; set; }

        [XmlElement(Order = 3)]
        public string Ogrn { get; set; }

        [XmlElement(Order = 4)]
        public string OgrnAuthority { get; set; }

        [XmlElement(Order = 5)]
        public string OgrnNum { get; set; }

        [XmlElement(Order = 6)]
        public DateTime? OgrnDate { get; set; }

        [XmlElement(Order = 7)]
        public string Inn { get; set; }

        [XmlElement(Order = 8)]
        public string InnAuthority { get; set; }

        [XmlElement(Order = 9)]
        public string InnNum { get; set; }

        [XmlElement(Order = 10)]
        public DateTime? InnDate { get; set; }

        [XmlElement(Order = 11)]
        public string Kpp { get; set; }

        [XmlElement(Order = 12)]
        public string Okpo { get; set; }

        [XmlElement(Order = 13)]
        public string OrgFormCode { get; set; }

        [XmlElement(Order = 14)]
        public Address PostalAddress { get; set; }

        // Почтовый адрес юридического лица
        [XmlElement(Order = 15)]
        public Address FactAddress { get; set; }

        // Местонахождение юридического лица
        [XmlElement(Order = 16)]
        public RequestContact OrgHead { get; set; }

        [XmlElement(Order = 17)]
        public string Okved { get; set; }

        [XmlElement(Order = 18)]
        public string Okfs { get; set; }

        [XmlElement(Order = 19)]
        public string BankName { get; set; }

        [XmlElement(Order = 20)]
        public string BankBik { get; set; }

        [XmlElement(Order = 21)]
        public string CorrAccount { get; set; }

        [XmlElement(Order = 22)]
        public string SetAccount { get; set; }

        [XmlElement(Order = 23)]
        public string Phone { get; set; }

        [XmlElement(Order = 24)]
        public string Fax { get; set; }

        [XmlElement(Order = 25)]
        public string EMail { get; set; }

        [XmlElement(Order = 26)]
        public string WebSite { get; set; }
    }

    [DataContract]
    public class CoordinateData
    {
        #region properties

        [DataMember(IsRequired = true, Order = 0)]
        public BaseDeclarant Declarant { get; set; }

        [DataMember(IsRequired = false, Order = 1)]
        public RequestContact Trustee { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public RequestService Service { get; set; }

        [DataMember(IsRequired = false, Order = 4)]
        public XmlElement CustomAttributes { get; set; }

        [DataMember(IsRequired = false, Order = 5)]
        public RequestContact[] Contacts { get; set; }

        [DataMember(IsRequired = false, Order = 6)]
        public byte[] Signature { get; set; }

        #endregion
    }

    public class Headers
    {
        public string FromOrgCode { get; set; }

        public string ToOrgCode { get; set; }

        public string MessageId { get; set; }

        public string RelatesTo { get; set; }

        public string ServiceNumber { get; set; }

        public DateTime RequestDateTime { get; set; }

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

    [DataContract]
    public class CoordinateTaskData
    {
        [DataMember(IsRequired = true, Order = 0)]
        public RequestTask Task { get; set; }

        [DataMember(IsRequired = false, Order = 1)]
        public DocumentsRequestData Data { get; set; }

        [DataMember(IsRequired = false, Order = 2)]
        public object Signature { get; set; }
    }

    [DataContract]
    public class CoordinateStatusData
    {
        #region properties

        public string RequestId { get; set; }

        public DateTime? ResponseDate { get; set; }

        public DateTime? PlanDate { get; set; }

        public int? StatusCode { get; set; }

        public Person Responsible { get; set; }

        public ServiceDocument[] Documents { get; set; }

        public RequestContact[] Contacts { get; set; }

        public string Note { get; set; }

        public RequestResult Result { get; set; }

        public string ServiceNumber { get; set; }

        public string ReasonCode { get; set; }

        #endregion
    }

    public class RequestStatus
    {
        [XmlElement(Order = 0)]
        public int StatusCode { get; set; }

        [XmlElement(Order = 1)]
        public DateTime StatusDate { get; set; }

        [XmlElement(Order = 2)]
        public string Reason { get; set; }

        [XmlElement(Order = 3)]
        public DateTime? ValidityPeriod { get; set; }

        [XmlElement(Order = 4)]
        public Person Responsible { get; set; }

        [XmlElement(Order = 5)]
        public Department Department { get; set; }

        [XmlElement(Order = 6)]
        public string ReasonCode { get; set; }
    }

    public class Person
    {
        [XmlElement(Order = 0)]
        public string LastName { get; set; }        // Фамилия

        [XmlElement(Order = 1)]
        public string FirstName { get; set; }       // Имя

        [XmlElement(Order = 2)]
        public string MiddleName { get; set; }      // Отчество

        [XmlElement(Order = 3)]
        public string JobTitle { get; set; }      // Должность

        [XmlElement(Order = 4)]
        public string Phone { get; set; }

        [XmlElement(Order = 5)]
        public string Email { get; set; }

        [XmlElement(Order = 6)]
        public string IsiId { get; set; }
    }

    [DataContract]
    public class Department
    {
        [XmlElement(Order = 0)]
        public string Name { get; set; }

        [XmlElement(Order = 1)]
        public string Code { get; set; }

        [XmlElement(Order = 2)]
        public string Inn { get; set; }

        [XmlElement(Order = 3)]
        public string Ogrn { get; set; }

        [XmlElement(Order = 4)]
        public DateTime? RegDate { get; set; }
    }

    public class RequestResult
    {
        public string ResultCode { get; set; }
        public string[] DeclineReasonCode { get; set; }
    }

    public class ServiceDocument
    {
        [XmlElement(Order = 0)]
        public string Id { get; set; }

        [XmlElement(Order = 1)]
        public string DocCode { get; set; }

        [XmlElement(Order = 2)]
        public string DocSubType { get; set; }

        [XmlElement(Order = 3)]
        public string DocPerson { get; set; }

        [XmlElement(Order = 4)]
        public string DocSerie { get; set; }

        [XmlElement(Order = 5)]
        public string DocNumber { get; set; }

        [XmlElement(Order = 6)]
        public DateTime? DocDate { get; set; }

        [XmlElement(Order = 7)]
        public DateTime? ValidityPeriod { get; set; }

        [XmlElement(Order = 8)]
        public string WhoSign { get; set; }

        [XmlElement(Order = 9)]
        public int? ListCount { get; set; }

        [XmlElement(Order = 10)]
        public int? CopyCount { get; set; }

        [XmlElement(Order = 11)]
        public string DivisionCode { get; set; }

        [XmlElement(Order = 12)]
        public byte[] Signature { get; set; }

        [XmlArray(Order = 13)]
        public Note[] DocNotes { get; set; }

        [XmlArray(Order = 14)]
        public File[] DocFiles { get; set; }

        [XmlElement(Order = 15)]
        public System.Xml.XmlElement CustomAttributes { get; set; }
    }

    public class Note
    {
        [XmlElement(Order = 0)]
        public string Subject { get; set; }

        [XmlElement(Order = 1)]
        public string Text { get; set; }
    }

    public class File
    {
        [XmlElement(Order = 0)]
        public string Id { get; set; }
        [XmlElement(Order = 1)]
        public string FileName { get; set; }
        [XmlElement(Order = 2)]
        public string MimeType { get; set; }
        [XmlElement(Order = 3)]
        public byte[] FileContent { get; set; }
    }

    [DataContract]
    public enum GenderType
    {
        [EnumMember]
        Male = 1,
        [EnumMember]
        Female = 2
    }

    public class Address
    {
        [XmlElement(Order = 0)]
        public string Country { get; set; }

        // Государство

        [XmlElement(Order = 1)]
        public string PostalCode { get; set; }

        // Почтовый индекс

        [XmlElement(Order = 2)]
        public string Locality { get; set; }

        // Субъект государства

        [XmlElement(Order = 3)]
        public string Region { get; set; }

        // Район

        [XmlElement(Order = 4)]
        public string City { get; set; }

        // Город

        [XmlElement(Order = 5)]
        public string Town { get; set; }

        // Населенный пункт

        [XmlElement(Order = 6)]
        public string Street { get; set; }

        // Улица

        [XmlElement(Order = 7)]
        public string House { get; set; }

        // Дом, корпус

        [XmlElement(Order = 8)]
        public string Building { get; set; }

        // Корпус

        [XmlElement(Order = 9)]
        public string Structure { get; set; }

        // Строение

        [XmlElement(Order = 10)]
        public string Facility { get; set; }

        // Сооружение

        [XmlElement(Order = 11)]
        public string Ownership { get; set; }

        // Владение

        [XmlElement(Order = 12)]
        public string Flat { get; set; }

        // Квартира

        [XmlElement(Order = 13)]
        public string POBox { get; set; }

        // Абонентский ящик

        [XmlElement(Order = 14)]
        public string Okato { get; set; }

        // ОКАТО

        //[Rkis("new_kladr", IsVocValue = true)]
        [XmlElement(Order = 15)]
        public string KladrCode { get; set; }

        // КЛАДР

        [XmlElement(Order = 16)]
        public string KladrStreetCode { get; set; }

        // Улицы КЛАДР

        [XmlElement(Order = 17)]
        public string OMKDistrictCode { get; set; }

        [XmlElement(Order = 18)]
        public string OMKRegionCode { get; set; }

        [XmlElement(Order = 19)]
        public string OMKTownCode { get; set; }

        [XmlElement(Order = 20)]
        public string OMKStreetCode { get; set; }

        [XmlElement(Order = 21)]
        public string BTIStreetCode { get; set; }

        [XmlElement(Order = 22)]
        //[Rkis("new_street.new_btistreetcode", Callback = true)]
        public string BTIBuildingCode { get; set; }
    }

    [DataContract]
	public enum CitizenshipType
	{
		[EnumMember] RF = 1,
		[EnumMember] Foreign = 2,
		[EnumMember] None = 3,
		[EnumMember] Both = 4
	}

    public class RequestServiceNoFiles
	{
		[XmlElement(Order = 0)]
		public string RegNum { get; set; }

		[XmlElement(Order = 1)]
		public DateTime? RegDate { get; set; }

		[XmlElement(Order = 2)]
		public string ServiceNumber { get; set; }

		[XmlElement(Order = 3)]
		public string ServiceTypeCode { get; set; }

		[XmlElement(Order = 4)]
		public decimal? ServicePrice { get; set; }

		[XmlElement(Order = 5)]
		public DateTime? PrepareTargetDate { get; set; }

		[XmlElement(Order = 6)]
		public DateTime? OutputTargetDate { get; set; }

		[XmlElement(Order = 7)]
		public int? Copies { get; set; }

		[XmlElement(Order = 8)]
		public Person Responsible { get; set; }

		//[Rkis("ownerid.businessunitid", Callback = true)]
		[XmlElement(Order = 9)]
		public Department Department { get; set; }

		[XmlArray(Order = 10)]
		public ServiceDocumentNoFiles[] Documents { get; set; }

        [XmlArray(Order = 11)]
        public string[] DeclineReasonCodes { get; set; }

        [XmlElement(Order = 12)]
        public Department CreatedByDepartment { get; set; }

        [XmlElement(Order = 13)]
        public DateTime? PrepareFactDate { get; set; }

        [XmlElement(Order = 14)]
        public DateTime? OutputFactDate { get; set; }
	}

    public class ServiceDocumentNoFiles
    {
        [XmlElement(Order = 0)]
        public string Id { get; set; }

        [XmlElement(Order = 1)]
        public string DocCode { get; set; }

        [XmlElement(Order = 2)]
        public string DocSubType { get; set; }

        [XmlElement(Order = 3)]
        public string DocPerson { get; set; }

        [XmlElement(Order = 4)]
        public string DocSerie { get; set; }

        [XmlElement(Order = 5)]
        public string DocNumber { get; set; }

        [XmlElement(Order = 6)]
        public DateTime? DocDate { get; set; }

        [XmlElement(Order = 7)]
        public DateTime? ValidityPeriod { get; set; }

        [XmlElement(Order = 8)]
        public string WhoSign { get; set; }

        [XmlElement(Order = 9)]
        public int? ListCount { get; set; }

        [XmlElement(Order = 10)]
        public int? CopyCount { get; set; }

        [XmlElement(Order = 11)]
        public string DivisionCode { get; set; }

        [XmlElement(Order = 12)]
        public byte[] Signature { get; set; }

        [XmlElement(Order = 13)]
        public System.Xml.XmlElement CustomAttributes { get; set; }
    }

    public class RequestQueryTask
    {
        [XmlElement(Order = 0)]
        public string RequestId { get; set; }

        [XmlElement(Order = 4)]
        public string Subject { get; set; }

        [XmlElement(Order = 5)]
        public DateTime? ValidityPeriod { get; set; }

        [XmlElement(Order = 6)]
        public int StatusCode { get; set; }

        [XmlElement(Order = 7)]
        public Person Responsible { get; set; }

        [XmlElement(Order = 8)]
        public Department Department { get; set; }

        [XmlElement(Order = 9)]
        public string DocCode { get; set; }
    }

    public class RequestService
    {
        [XmlElement(Order = 0)]
        public string RegNum { get; set; }

        [XmlElement(Order = 1)]
        public DateTime? RegDate { get; set; }

        [XmlElement(Order = 2)]
        public string ServiceNumber { get; set; }

        [XmlElement(Order = 3)]
        public string ServiceTypeCode { get; set; }

        [XmlElement(Order = 4)]
        public decimal? ServicePrice { get; set; }

        [XmlElement(Order = 5)]
        public DateTime? PrepareTargetDate { get; set; }

        [XmlElement(Order = 6)]
        public DateTime? OutputTargetDate { get; set; }

        [XmlElement(Order = 7)]
        public int? Copies { get; set; }

        [XmlElement(Order = 8)]
        public Person Responsible { get; set; }

        [XmlElement(Order = 9)]
        public Department Department { get; set; }

        [XmlArray(Order = 10)]
        public ServiceDocument[] Documents { get; set; }

        [XmlArray(Order = 11)]
        public string[] DeclineReasonCodes { get; set; }

        [XmlElement(Order = 12)]
        public Department CreatedByDepartment { get; set; }

        [XmlElement(Order = 13)]
        public DateTime? PrepareFactDate { get; set; }

        [XmlElement(Order = 14)]
        public DateTime? OutputFactDate { get; set; }
    }

    public class RequestTask
    {
        [XmlElement(Order = 0)]
        public string RequestId { get; set; }

        [XmlElement(Order = 5)]
        public string Code { get; set; }

        [XmlElement(Order = 6)]
        public string Subject { get; set; }

        [XmlElement(Order = 7)]
        public DateTime? ValidityPeriod { get; set; }

        [XmlElement(Order = 8)]
        public Person Responsible { get; set; }

        [XmlElement(Order = 9)]
        public Department Department { get; set; }

        [XmlElement(Order = 10)]
        public string ServiceNumber { get; set; }

        [XmlElement(Order = 11)]
        public string ServiceTypeCode { get; set; }
    }

    [DataContract]
    public class DocumentsRequestData
    {
        [DataMember(IsRequired = false, Order = 0)]
        public string DocumentTypeCode { get; set; }

        [DataMember(IsRequired = false, Order = 1)]
        public string ParameterTypeCode { get; set; }

        [DataMember(IsRequired = false, Order = 2)]
        public XmlElement Parameter { get; set; }

        [DataMember(IsRequired = false, Order = 3)]
        public bool IncludeXmlView { get; set; }

        [DataMember(IsRequired = false, Order = 4)]
        public bool IncludeBinaryView { get; set; }

    }

    [DataContract(Namespace = Namespace.ServiceNamespace)]
    public class ErrorMessageData
    {
        [DataMember]
        public string ErrorCode { get; set; }
        [DataMember]
        public string ErrorText { get; set; }
    }
}
