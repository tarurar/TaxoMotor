// <copyright file="CoordinateV5Entities.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-07-11 19:10:47Z</date>
// <auto-generated>
//   Generated with SharePoint Software Factory 4.1
// </auto-generated>
namespace TM.SP.BCSModels.CoordinateV5
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public partial class RequestAccountData
    {
        //TODO: Implement additional properties here.
        public System.Int32 Id { get; set; }
        public System.String Title { get; set; }
        public System.Nullable<System.Int32> DeclarantAccountId { get; set; }
        public System.String DeclarantAccountFullName { get; set; }
        public System.String Ogrn { get; set; }
    }

    /// <summary>
    /// This class contains the properties for Address
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class Address
    {
        //TODO: Implement additional properties here.
        public System.String Title { get; set; }
        public System.String Country { get; set; }
        public System.String PostalCode { get; set; }
        public System.String Locality { get; set; }
        public System.String Region { get; set; }
        public System.String City { get; set; }
        public System.String Town { get; set; }
        public System.String Street { get; set; }
        public System.String House { get; set; }
        public System.String Building { get; set; }
        public System.String Structure { get; set; }
        public System.String Facility { get; set; }
        public System.String Ownership { get; set; }
        public System.String Flat { get; set; }
        public System.String POBox { get; set; }
        public System.String Okato { get; set; }
        public System.String KladrCode { get; set; }
        public System.String KladrStreetCode { get; set; }
        public System.String OMKDistrictCode { get; set; }
        public System.String OMKRegionCode { get; set; }
        public System.String OMKTownCode { get; set; }
        public System.String OMKStreetCode { get; set; }
        public System.String BTIStreetCode { get; set; }
        public System.String BTIBuildingCode { get; set; }
        public System.String Type { get; set; }
        public System.Int32 Id { get; set; }
        public System.String MessageId { get; set; }
    }

    /// <summary>
    /// This class contains the properties for File
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class File
    {
        //TODO: Implement additional properties here.
        public System.String Id { get; set; }
        public System.String Title { get; set; }
        public System.String FileName { get; set; }
        public System.String MimeType { get; set; }
        public System.Nullable<System.Boolean> IsFileInStore { get; set; }
        public System.String FileIdInStore { get; set; }
        public System.String StoreName { get; set; }
        public System.Int32 Id_Auto { get; set; }
        public System.String MessageId { get; set; }
        public System.Int32 ServiceDocument { get; set; }
    }

    /// <summary>
    /// This class contains the properties for Request
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class Request
    {
        //TODO: Implement additional properties here.
        public System.Int32 Id { get; set; }
        public System.String Title { get; set; }
        public System.String MessageId { get; set; }
        public System.Nullable<System.Int32> DeclarantRequestAccount { get; set; }
        public System.Nullable<System.Int32> DeclarantRequestContact { get; set; }
        public System.Nullable<System.Int32> TrusteeRequestContact { get; set; }
        public System.Int32 ServiceProperties { get; set; }
        public System.Int32 Service { get; set; }
        public System.Int32 ServiceHeader { get; set; }
    }

    /// <summary>
    /// This class contains the properties for RequestAccount
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class RequestAccount
    {
        //TODO: Implement additional properties here.
        public System.String Title { get; set; }
        public System.String FullName { get; set; }
        public System.String Name { get; set; }
        public System.String BrandName { get; set; }
        public System.String Ogrn { get; set; }
        public System.String OgrnAuthority { get; set; }
        public System.String OgrnNum { get; set; }
        public System.Nullable<System.DateTime> OgrnDate { get; set; }
        public System.String Inn { get; set; }
        public System.String InnAuthority { get; set; }
        public System.String InnNum { get; set; }
        public System.Nullable<System.DateTime> InnDate { get; set; }
        public System.String Kpp { get; set; }
        public System.String Okpo { get; set; }
        public System.String OrgFormCode { get; set; }
        public System.String Okved { get; set; }
        public System.String Okfs { get; set; }
        public System.String BankName { get; set; }
        public System.String BankBik { get; set; }
        public System.String CorrAccount { get; set; }
        public System.String SetAccount { get; set; }
        public System.String Phone { get; set; }
        public System.String Fax { get; set; }
        public System.String EMail { get; set; }
        public System.String WebSite { get; set; }
        public System.Int32 Id { get; set; }
        public System.String MessageId { get; set; }
        public System.Nullable<System.Int32> PostalAddress { get; set; }
        public System.Nullable<System.Int32> FactAddress { get; set; }
        public System.Nullable<System.Int32> RequestContact { get; set; }
    }

    /// <summary>
    /// This class contains the properties for RequestContact
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class RequestContact
    {
        //TODO: Implement additional properties here.
        public System.String Id { get; set; }
        public System.String Title { get; set; }
        public System.String LastName { get; set; }
        public System.String FirstName { get; set; }
        public System.String MiddleName { get; set; }
        public System.String Gender { get; set; }
        public System.Nullable<System.DateTime> BirthDate { get; set; }
        public System.String Snils { get; set; }
        public System.String Inn { get; set; }
        public System.String MobilePhone { get; set; }
        public System.String WorkPhone { get; set; }
        public System.String HomePhone { get; set; }
        public System.String EMail { get; set; }
        public System.String Nation { get; set; }
        public System.String Citizenship { get; set; }
        public System.String CitizenshipType { get; set; }
        public System.String JobTitle { get; set; }
        public System.String OMSNum { get; set; }
        public System.Nullable<System.DateTime> OMSDate { get; set; }
        public System.String OMSCompany { get; set; }
        public System.String OMSValidityPeriod { get; set; }
        public System.String IsiId { get; set; }
        public System.Int32 Id_Auto { get; set; }
        public System.String MessageId { get; set; }
        public System.Nullable<System.Int32> RegAddress { get; set; }
        public System.Nullable<System.Int32> FactAddress { get; set; }
        public System.Nullable<System.Int32> BirthAddress { get; set; }
    }

    /// <summary>
    /// This class contains the properties for Service
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class Service
    {
        //TODO: Implement additional properties here.
        public System.String Title { get; set; }
        public System.String RegNum { get; set; }
        public System.Nullable<System.DateTime> RegDate { get; set; }
        public System.String ServiceNumber { get; set; }
        public System.String ServiceTypeCode { get; set; }
        public System.Nullable<System.Decimal> ServicePrice { get; set; }
        public System.Nullable<System.DateTime> PrepareTargetDate { get; set; }
        public System.Nullable<System.DateTime> OutputTargetDate { get; set; }
        public System.Nullable<System.Int32> Copies { get; set; }
        public System.Nullable<System.DateTime> PrepareFactDate { get; set; }
        public System.Nullable<System.DateTime> OutputFactDate { get; set; }
        public System.Int32 Id { get; set; }
        public System.String MessageId { get; set; }
    }

    /// <summary>
    /// This class contains the properties for ServiceDocument
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class ServiceDocument
    {
        //TODO: Implement additional properties here.
        public System.String Id { get; set; }
        public System.String Title { get; set; }
        public System.String DocCode { get; set; }
        public System.String DocSubType { get; set; }
        public System.String DocPerson { get; set; }
        public System.String DocSerie { get; set; }
        public System.String DocNumber { get; set; }
        public System.Nullable<System.DateTime> DocDate { get; set; }
        public System.String ValidityPeriod { get; set; }
        public System.String WhoSign { get; set; }
        public System.Nullable<System.Int32> ListCount { get; set; }
        public System.Nullable<System.Int32> CopyCount { get; set; }
        public System.String DivisionCode { get; set; }
        public System.Int32 Id_Auto { get; set; }
        public System.String MessageId { get; set; }
        public System.Int32 Service { get; set; }
    }

    /// <summary>
    /// This class contains the properties for ServiceHeader
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class ServiceHeader
    {
        //TODO: Implement additional properties here.
        public System.String Title { get; set; }
        public System.String FromOrgCode { get; set; }
        public System.String ToOrgCode { get; set; }
        public System.String MessageId { get; set; }
        public System.String RelatesTo { get; set; }
        public System.String ServiceNumber { get; set; }
        public System.DateTime RequestDateTime { get; set; }
        public System.Int32 Id { get; set; }
    }

    /// <summary>
    /// This class contains the properties for ServiceProperties
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class ServiceProperties
    {
        //TODO: Implement additional properties here.
        public System.String Title { get; set; }
        public System.Nullable<System.Int32> delete { get; set; }
        public System.String name { get; set; }
        public System.String other { get; set; }
        public System.Nullable<System.Boolean> pr_pereoformlenie { get; set; }
        public System.Nullable<System.Boolean> pr_pereoformlenie_2 { get; set; }
        public System.Nullable<System.Boolean> pr_pereoformlenie_3 { get; set; }
        public System.Nullable<System.Boolean> pr_pereoformlenie_4 { get; set; }
        public System.Nullable<System.Boolean> pr_pereoformlenie_5 { get; set; }
        public System.Nullable<System.Boolean> pr_pereoformlenie_6 { get; set; }
        public System.Nullable<System.Boolean> pr_pereoformlenie_7 { get; set; }
        public System.Int32 Id { get; set; }
        public System.String MessageId { get; set; }
    }

    /// <summary>
    /// This class contains the properties for taxi_info
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class taxi_info
    {
        //TODO: Implement additional properties here.
        public System.String Title { get; set; }
        public System.String blankno { get; set; }
        public System.String brand { get; set; }
        public System.String brandname { get; set; }
        public System.String color { get; set; }
        public System.String color_2 { get; set; }
        public System.Nullable<System.Boolean> color_number { get; set; }
        public System.Nullable<System.Boolean> color_yellow { get; set; }
        public System.Nullable<System.Int32> condition { get; set; }
        public System.String date_ran { get; set; }
        public System.Nullable<System.Boolean> decision { get; set; }
        public System.String declinereason { get; set; }
        public System.String details { get; set; }
        public System.Nullable<System.Int32> doc { get; set; }
        public System.Nullable<System.Boolean> gps { get; set; }
        public System.String licensedate { get; set; }
        public System.String licensenum { get; set; }
        public System.String lizdetails { get; set; }
        public System.String model { get; set; }
        public System.String name { get; set; }
        public System.String num { get; set; }
        public System.String num2 { get; set; }
        public System.String number_ran { get; set; }
        public System.String owner { get; set; }
        public System.String taxi_info_old { get; set; }
        public System.Nullable<System.Boolean> taxometr { get; set; }
        public System.Nullable<System.DateTime> todate { get; set; }
        public System.Nullable<System.Int32> year { get; set; }
        public System.Int32 Id { get; set; }
        public System.String MessageId { get; set; }
        public System.Int32 ServiceProperties { get; set; }
    }
}

