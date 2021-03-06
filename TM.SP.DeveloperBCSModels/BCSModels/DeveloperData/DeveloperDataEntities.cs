// <copyright file="DeveloperDataEntities.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-07-11 17:37:37Z</date>
// <auto-generated>
//   Generated with SharePoint Software Factory 4.1
// </auto-generated>
namespace TM.SP.BCSModels.DeveloperData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class contains the properties for DEBUG_DATA
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class DEBUG_DATA
    {
        //TODO: Implement additional properties here.
        public System.Int32 Id { get; set; }
        public System.Nullable<System.DateTime> DATE { get; set; }
        public System.String DATA_STR1 { get; set; }
        public System.String DATA_STR2 { get; set; }
        public System.String DATA_STR3 { get; set; }
        public System.String DATA_STR4 { get; set; }
        public System.String DATA_STR5 { get; set; }
        public System.Nullable<System.Int32> DATA_INT1 { get; set; }
        public System.Nullable<System.Int32> DATA_INT2 { get; set; }
        public System.Nullable<System.Int32> DATA_INT3 { get; set; }
        public System.Nullable<System.Int32> DATA_INT4 { get; set; }
        public System.Nullable<System.Int32> DATA_INT5 { get; set; }
    }

    /// <summary>
    /// This class contains the properties for ErrorData
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class ErrorData
    {
        //TODO: Implement additional properties here.
        public System.Int32 Id { get; set; }
        public System.Nullable<System.DateTime> Date { get; set; }
        public System.Nullable<System.Int32> Number { get; set; }
        public System.Nullable<System.Int32> Severity { get; set; }
        public System.Nullable<System.Int32> State { get; set; }
        public System.String Procedure { get; set; }
        public System.Nullable<System.Int32> Line { get; set; }
        public System.String Message { get; set; }
        public System.String SystemContext { get; set; }
        public System.String UserContext { get; set; }
    }
}

