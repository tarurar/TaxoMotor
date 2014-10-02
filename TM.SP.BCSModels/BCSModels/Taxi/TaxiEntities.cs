// <copyright file="TaxiEntities.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-09-30 12:37:24Z</date>
// <auto-generated>
//   Generated with SharePoint Software Factory 4.1
// </auto-generated>
namespace TM.SP.BCSModels.Taxi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class contains the properties for License
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class License
    {
        //TODO: Implement additional properties here.
        public System.Int32 Id { get; set; }
        public System.String Title { get; set; }
        public System.String RegNumber { get; set; }
        public System.String BlankSeries { get; set; }
        public System.String BlankNo { get; set; }
        public System.String OrgName { get; set; }
        public System.String Ogrn { get; set; }
        public System.String Inn { get; set; }
        public System.Nullable<System.Int32> Parent { get; set; }
        public System.Nullable<System.Int32> Status { get; set; }
        public System.String Document { get; set; }
        public System.String Signature { get; set; }
        public System.Nullable<System.Int32> TaxiId { get; set; }
        public System.String Lfb { get; set; }
        public System.String JuridicalAddress { get; set; }
        public System.String PhoneNumber { get; set; }
        public System.String AddContactData { get; set; }
        public System.String AccountAbbr { get; set; }
        public System.String TaxiBrand { get; set; }
        public System.String TaxiModel { get; set; }
        public System.String TaxiStateNumber { get; set; }
        public System.Nullable<System.Int32> TaxiYear { get; set; }
        public System.Nullable<System.DateTime> OutputDate { get; set; }
        public System.Nullable<System.DateTime> CreationDate { get; set; }
        public System.Nullable<System.DateTime> TillDate { get; set; }
        public System.Nullable<System.DateTime> TillSuspensionDate { get; set; }
        public System.String CancellationReason { get; set; }
        public System.String SuspensionReason { get; set; }
        public System.String ChangeReason { get; set; }
        public System.String InvalidReason { get; set; }
    }
}

