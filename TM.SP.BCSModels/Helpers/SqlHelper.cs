﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TM.SP.BCSModels.Taxi;

namespace TM.SP.BCSModels.Helpers
{
    public static class SqlHelper
    {
        #region [methods]
        public static string LoadSQLStatement(string statementName)
        {
            string sqlStatement = string.Empty;

            string namespacePart = "TM.SP.BCSModels.SQL";
            string resourceName = namespacePart + "." + statementName;

            using (Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stm != null)
                {
                    sqlStatement = new StreamReader(stm).ReadToEnd();
                }
            }

            return sqlStatement;
        }
        public static void LicenseFillFromReader(License entity, SqlDataReader reader)
        {
            entity.Id                 = (System.Int32)reader["Id"];
            entity.Title              = (reader["Title"] == DBNull.Value) ? null : reader["Title"].ToString();
            entity.RegNumber          = (reader["RegNumber"] == DBNull.Value) ? null : reader["RegNumber"].ToString();
            entity.BlankSeries        = (reader["BlankSeries"] == DBNull.Value) ? null : reader["BlankSeries"].ToString();
            entity.BlankNo            = (reader["BlankNo"] == DBNull.Value) ? null : reader["BlankNo"].ToString();
            entity.OrgName            = (reader["OrgName"] == DBNull.Value) ? null : reader["OrgName"].ToString();
            entity.Ogrn               = (reader["Ogrn"] == DBNull.Value) ? null : reader["Ogrn"].ToString();
            entity.Inn                = (reader["Inn"] == DBNull.Value) ? null : reader["Inn"].ToString();
            entity.Parent             = reader["Parent"] as System.Nullable<System.Int32>;
            entity.RootParent         = reader["RootParent"] as System.Nullable<System.Int32>;
            entity.Status             = reader["Status"] as System.Nullable<System.Int32>;
            entity.Document           = (reader["Document"] == DBNull.Value) ? null : reader["Document"].ToString();
            entity.SignatureBinary    = (reader["SignatureBinary"] == DBNull.Value) ? null : reader["SignatureBinary"] as byte[];
            entity.TaxiId             = reader["TaxiId"] as System.Nullable<System.Int32>;
            entity.Lfb                = (reader["Lfb"] == DBNull.Value) ? null : reader["Lfb"].ToString();
            entity.JuridicalAddress   = (reader["JuridicalAddress"] == DBNull.Value) ? null : reader["JuridicalAddress"].ToString();
            entity.PhoneNumber        = (reader["PhoneNumber"] == DBNull.Value) ? null : reader["PhoneNumber"].ToString();
            entity.AddContactData     = (reader["AddContactData"] == DBNull.Value) ? null : reader["AddContactData"].ToString();
            entity.AccountAbbr        = (reader["AccountAbbr"] == DBNull.Value) ? null : reader["AccountAbbr"].ToString();
            entity.TaxiBrand          = (reader["TaxiBrand"] == DBNull.Value) ? null : reader["TaxiBrand"].ToString();
            entity.TaxiModel          = (reader["TaxiModel"] == DBNull.Value) ? null : reader["TaxiModel"].ToString();
            entity.TaxiStateNumber    = (reader["TaxiStateNumber"] == DBNull.Value) ? null : reader["TaxiStateNumber"].ToString();
            entity.TaxiYear           = reader["TaxiYear"] as System.Nullable<System.Int32>;
            entity.OutputDate         = reader["OutputDate"] as System.Nullable<System.DateTime>;
            entity.CreationDate       = reader["CreationDate"] as System.Nullable<System.DateTime>;
            entity.TillDate           = reader["TillDate"] as System.Nullable<System.DateTime>;
            entity.TillSuspensionDate = reader["TillSuspensionDate"] as System.Nullable<System.DateTime>;
            entity.CancellationReason = (reader["CancellationReason"] == DBNull.Value) ? null : reader["CancellationReason"].ToString();
            entity.SuspensionReason   = (reader["SuspensionReason"] == DBNull.Value) ? null : reader["SuspensionReason"].ToString();
            entity.ChangeReason       = (reader["ChangeReason"] == DBNull.Value) ? null : reader["ChangeReason"].ToString();
            entity.InvalidReason      = (reader["InvalidReason"] == DBNull.Value) ? null : reader["InvalidReason"].ToString();
            entity.ShortName          = (reader["ShortName"] == DBNull.Value) ? null : reader["ShortName"].ToString();
            entity.LastName           = (reader["LastName"] == DBNull.Value) ? null : reader["LastName"].ToString();
            entity.FirstName          = (reader["FirstName"] == DBNull.Value) ? null : reader["FirstName"].ToString();
            entity.SecondName         = (reader["SecondName"] == DBNull.Value) ? null : reader["SecondName"].ToString();
            entity.OgrnDate           = reader["OgrnDate"] as System.Nullable<System.DateTime>;
            entity.Country            = (reader["Country"] == DBNull.Value) ? null : reader["Country"].ToString();
            entity.PostalCode         = (reader["PostalCode"] == DBNull.Value) ? null : reader["PostalCode"].ToString();
            entity.Locality           = (reader["Locality"] == DBNull.Value) ? null : reader["Locality"].ToString();
            entity.Region             = (reader["Region"] == DBNull.Value) ? null : reader["Region"].ToString();
            entity.City               = (reader["City"] == DBNull.Value) ? null : reader["City"].ToString();
            entity.Town               = (reader["Town"] == DBNull.Value) ? null : reader["Town"].ToString();
            entity.Street             = (reader["Street"] == DBNull.Value) ? null : reader["Street"].ToString();
            entity.House              = (reader["House"] == DBNull.Value) ? null : reader["House"].ToString();
            entity.Building           = (reader["Building"] == DBNull.Value) ? null : reader["Building"].ToString();
            entity.Structure          = (reader["Structure"] == DBNull.Value) ? null : reader["Structure"].ToString();
            entity.Facility           = (reader["Facility"] == DBNull.Value) ? null : reader["Facility"].ToString();
            entity.Ownership          = (reader["Ownership"] == DBNull.Value) ? null : reader["Ownership"].ToString();
            entity.Flat               = (reader["Flat"] == DBNull.Value) ? null : reader["Flat"].ToString();
            entity.Fax                = (reader["Fax"] == DBNull.Value) ? null : reader["Fax"].ToString();
            entity.EMail              = (reader["EMail"] == DBNull.Value) ? null : reader["EMail"].ToString();
            entity.TaxiColor          = (reader["TaxiColor"] == DBNull.Value) ? null : reader["TaxiColor"].ToString();
            entity.TaxiNumberColor    = (reader["TaxiNumberColor"] == DBNull.Value) ? null : reader["TaxiNumberColor"].ToString();
            entity.TaxiVin            = (reader["TaxiVin"] == DBNull.Value) ? null : reader["TaxiVin"].ToString();
            entity.ChangeDate         = reader["ChangeDate"] as System.Nullable<System.DateTime>;
            entity.Guid_OD            = (reader["Guid_OD"] == DBNull.Value) ? null : reader["Guid_OD"].ToString();
            entity.Date_OD            = reader["Date_OD"] as System.Nullable<System.DateTime>;
            entity.FromPortal         = reader["FromPortal"] as System.Nullable<System.Boolean>;
            entity.FirmName           = (reader["FirmName"] == DBNull.Value) ? null : reader["FirmName"].ToString();
            entity.Brand              = (reader["Brand"] == DBNull.Value) ? null : reader["Brand"].ToString();
            entity.OgrnNum            = (reader["OgrnNum"] == DBNull.Value) ? null : reader["OgrnNum"].ToString();
            entity.OgrnName           = (reader["OgrnName"] == DBNull.Value) ? null : reader["OgrnName"].ToString();
            entity.GRAddress          = (reader["GRAddress"] == DBNull.Value) ? null : reader["GRAddress"].ToString();
            entity.InnDate            = reader["InnDate"] as System.Nullable<System.DateTime>;
            entity.InnName            = (reader["InnName"] == DBNull.Value) ? null : reader["InnName"].ToString();
            entity.InnNum             = (reader["InnNum"] == DBNull.Value) ? null : reader["InnNum"].ToString();
            entity.Address_Fact       = (reader["Address_Fact"] == DBNull.Value) ? null : reader["Address_Fact"].ToString();
            entity.Country_Fact       = (reader["Country_Fact"] == DBNull.Value) ? null : reader["Country_Fact"].ToString();
            entity.PostalCode_Fact    = (reader["PostalCode_Fact"] == DBNull.Value) ? null : reader["PostalCode_Fact"].ToString();
            entity.Locality_Fact      = (reader["Locality_Fact"] == DBNull.Value) ? null : reader["Locality_Fact"].ToString();
            entity.Region_Fact        = (reader["Region_Fact"] == DBNull.Value) ? null : reader["Region_Fact"].ToString();
            entity.City_Fact          = (reader["City_Fact"] == DBNull.Value) ? null : reader["City_Fact"].ToString();
            entity.Town_Fact          = (reader["Town_Fact"] == DBNull.Value) ? null : reader["Town_Fact"].ToString();
            entity.Street_Fact        = (reader["Street_Fact"] == DBNull.Value) ? null : reader["Street_Fact"].ToString();
            entity.House_Fact         = (reader["House_Fact"] == DBNull.Value) ? null : reader["House_Fact"].ToString();
            entity.Building_Fact      = (reader["Building_Fact"] == DBNull.Value) ? null : reader["Building_Fact"].ToString();
            entity.Structure_Fact     = (reader["Structure_Fact"] == DBNull.Value) ? null : reader["Structure_Fact"].ToString();
            entity.Facility_Fact      = (reader["Facility_Fact"] == DBNull.Value) ? null : reader["Facility_Fact"].ToString();
            entity.Ownership_Fact     = (reader["Ownership_Fact"] == DBNull.Value) ? null : reader["Ownership_Fact"].ToString();
            entity.Flat_Fact          = (reader["Flat_Fact"] == DBNull.Value) ? null : reader["Flat_Fact"].ToString();
            entity.Gps                = reader["Gps"] as System.Nullable<System.Boolean>;
            entity.Taxometr           = reader["Taxometr"] as System.Nullable<System.Boolean>;
            entity.TODate             = reader["TODate"] as System.Nullable<System.DateTime>;
            entity.STSNumber          = (reader["STSNumber"] == DBNull.Value) ? null : reader["STSNumber"].ToString();
            entity.STSDate            = reader["STSDate"] as System.Nullable<System.DateTime>;
            entity.OwnType            = reader["OwnType"] as System.Nullable<System.Int32>;
            entity.OwnNumber          = (reader["OwnNumber"] == DBNull.Value) ? null : reader["OwnNumber"].ToString();
            entity.OwnDate            = reader["OwnDate"] as System.Nullable<System.DateTime>;
            entity.MO                 = reader["MO"] as System.Nullable<System.Boolean>;
            entity.GUID_MO            = (reader["GUID_MO"] == DBNull.Value) ? null : reader["GUID_MO"].ToString();
            entity.DATE_MO            = reader["DATE_MO"] as System.Nullable<System.DateTime>;
            entity.Obsolete           = reader["Obsolete"] as System.Nullable<System.Boolean>;
            entity.DisableGibddSend   = reader["DisableGibddSend"] as System.Nullable<System.Boolean>;
        }
        public static void NewLicenseParams(License newEntity, SqlParameterCollection parameters)
        {
            parameters.AddWithValue("@RegNumber", (newEntity.RegNumber == null) ? (object)DBNull.Value : newEntity.RegNumber);
            parameters.AddWithValue("@BlankSeries", (newEntity.BlankSeries == null) ? (object)DBNull.Value : newEntity.BlankSeries);
            parameters.AddWithValue("@BlankNo", (newEntity.BlankNo == null) ? (object)DBNull.Value : newEntity.BlankNo);
            parameters.AddWithValue("@OrgName", (newEntity.OrgName == null) ? (object)DBNull.Value : newEntity.OrgName);
            parameters.AddWithValue("@Ogrn", (newEntity.Ogrn == null) ? (object)DBNull.Value : newEntity.Ogrn);
            parameters.AddWithValue("@Inn", (newEntity.Inn == null) ? (object)DBNull.Value : newEntity.Inn);
            parameters.AddWithValue("@Parent", (newEntity.Parent == null) ? (object)DBNull.Value : newEntity.Parent);
            parameters.AddWithValue("@RootParent", (newEntity.RootParent == null) ? (object)DBNull.Value : newEntity.RootParent);
            parameters.AddWithValue("@Status", (newEntity.Status == null) ? (object)DBNull.Value : newEntity.Status);
            parameters.AddWithValue("@Document", (newEntity.Document == null) ? (object)DBNull.Value : newEntity.Document);

            var signatureParam = parameters.Add("@SignatureBinary", System.Data.SqlDbType.VarBinary, -1);
            signatureParam.Value = (newEntity.SignatureBinary == null) ? (object)DBNull.Value : newEntity.SignatureBinary;

            parameters.AddWithValue("@TaxiId", (newEntity.TaxiId == null) ? (object)DBNull.Value : newEntity.TaxiId);
            parameters.AddWithValue("@Lfb", (newEntity.Lfb == null) ? (object)DBNull.Value : newEntity.Lfb);
            parameters.AddWithValue("@JuridicalAddress", (newEntity.JuridicalAddress == null) ? (object)DBNull.Value : newEntity.JuridicalAddress);
            parameters.AddWithValue("@PhoneNumber", (newEntity.PhoneNumber == null) ? (object)DBNull.Value : newEntity.PhoneNumber);
            parameters.AddWithValue("@AddContactData", (newEntity.AddContactData == null) ? (object)DBNull.Value : newEntity.AddContactData);
            parameters.AddWithValue("@AccountAbbr", (newEntity.AccountAbbr == null) ? (object)DBNull.Value : newEntity.AccountAbbr);
            parameters.AddWithValue("@TaxiBrand", (newEntity.TaxiBrand == null) ? (object)DBNull.Value : newEntity.TaxiBrand);
            parameters.AddWithValue("@TaxiModel", (newEntity.TaxiModel == null) ? (object)DBNull.Value : newEntity.TaxiModel);
            parameters.AddWithValue("@TaxiStateNumber", (newEntity.TaxiStateNumber == null) ? (object)DBNull.Value : newEntity.TaxiStateNumber);
            parameters.AddWithValue("@TaxiYear", (newEntity.TaxiYear == null) ? (object)DBNull.Value : newEntity.TaxiYear);
            parameters.AddWithValue("@OutputDate", (newEntity.OutputDate == null) ? (object)DBNull.Value : newEntity.OutputDate);
            parameters.AddWithValue("@CreationDate", (newEntity.CreationDate == null) ? (object)DBNull.Value : newEntity.CreationDate);
            parameters.AddWithValue("@TillDate", (newEntity.TillDate == null) ? (object)DBNull.Value : newEntity.TillDate);
            parameters.AddWithValue("@TillSuspensionDate", (newEntity.TillSuspensionDate == null) ? (object)DBNull.Value : newEntity.TillSuspensionDate);
            parameters.AddWithValue("@CancellationReason", (newEntity.CancellationReason == null) ? (object)DBNull.Value : newEntity.CancellationReason);
            parameters.AddWithValue("@SuspensionReason", (newEntity.SuspensionReason == null) ? (object)DBNull.Value : newEntity.SuspensionReason);
            parameters.AddWithValue("@ChangeReason", (newEntity.ChangeReason == null) ? (object)DBNull.Value : newEntity.ChangeReason);
            parameters.AddWithValue("@InvalidReason", (newEntity.InvalidReason == null) ? (object)DBNull.Value : newEntity.InvalidReason);
            parameters.AddWithValue("@ShortName", (newEntity.ShortName == null) ? (object)DBNull.Value : newEntity.ShortName);
            parameters.AddWithValue("@LastName", (newEntity.LastName == null) ? (object)DBNull.Value : newEntity.LastName);
            parameters.AddWithValue("@FirstName", (newEntity.FirstName == null) ? (object)DBNull.Value : newEntity.FirstName);
            parameters.AddWithValue("@SecondName", (newEntity.SecondName == null) ? (object)DBNull.Value : newEntity.SecondName);
            parameters.AddWithValue("@OgrnDate", (newEntity.OgrnDate == null) ? (object)DBNull.Value : newEntity.OgrnDate);
            parameters.AddWithValue("@Country", (newEntity.Country == null) ? (object)DBNull.Value : newEntity.Country);
            parameters.AddWithValue("@PostalCode", (newEntity.PostalCode == null) ? (object)DBNull.Value : newEntity.PostalCode);
            parameters.AddWithValue("@Locality", (newEntity.Locality == null) ? (object)DBNull.Value : newEntity.Locality);
            parameters.AddWithValue("@Region", (newEntity.Region == null) ? (object)DBNull.Value : newEntity.Region);
            parameters.AddWithValue("@City", (newEntity.City == null) ? (object)DBNull.Value : newEntity.City);
            parameters.AddWithValue("@Town", (newEntity.Town == null) ? (object)DBNull.Value : newEntity.Town);
            parameters.AddWithValue("@Street", (newEntity.Street == null) ? (object)DBNull.Value : newEntity.Street);
            parameters.AddWithValue("@House", (newEntity.House == null) ? (object)DBNull.Value : newEntity.House);
            parameters.AddWithValue("@Building", (newEntity.Building == null) ? (object)DBNull.Value : newEntity.Building);
            parameters.AddWithValue("@Structure", (newEntity.Structure == null) ? (object)DBNull.Value : newEntity.Structure);
            parameters.AddWithValue("@Facility", (newEntity.Facility == null) ? (object)DBNull.Value : newEntity.Facility);
            parameters.AddWithValue("@Ownership", (newEntity.Ownership == null) ? (object)DBNull.Value : newEntity.Ownership);
            parameters.AddWithValue("@Flat", (newEntity.Flat == null) ? (object)DBNull.Value : newEntity.Flat);
            parameters.AddWithValue("@Fax", (newEntity.Fax == null) ? (object)DBNull.Value : newEntity.Fax);
            parameters.AddWithValue("@EMail", (newEntity.EMail == null) ? (object)DBNull.Value : newEntity.EMail);
            parameters.AddWithValue("@TaxiColor", (newEntity.TaxiColor == null) ? (object)DBNull.Value : newEntity.TaxiColor);
            parameters.AddWithValue("@TaxiNumberColor", (newEntity.TaxiNumberColor == null) ? (object)DBNull.Value : newEntity.TaxiNumberColor);
            parameters.AddWithValue("@TaxiVin", (newEntity.TaxiVin == null) ? (object)DBNull.Value : newEntity.TaxiVin);
            parameters.AddWithValue("@ChangeDate", (newEntity.ChangeDate == null) ? (object)DBNull.Value : newEntity.ChangeDate);
            parameters.AddWithValue("@Guid_OD", (newEntity.Guid_OD == null) ? (object)DBNull.Value : newEntity.Guid_OD);
            parameters.AddWithValue("@Date_OD", (newEntity.Date_OD == null) ? (object)DBNull.Value : newEntity.Date_OD);
            parameters.AddWithValue("@FromPortal", (newEntity.FromPortal == null) ? (object)DBNull.Value : newEntity.FromPortal);
            parameters.AddWithValue("@FirmName", (newEntity.FirmName == null) ? (object)DBNull.Value : newEntity.FirmName);
            parameters.AddWithValue("@Brand", (newEntity.Brand == null) ? (object)DBNull.Value : newEntity.Brand);
            parameters.AddWithValue("@OgrnNum", (newEntity.OgrnNum == null) ? (object)DBNull.Value : newEntity.OgrnNum);
            parameters.AddWithValue("@OgrnName", (newEntity.OgrnName == null) ? (object)DBNull.Value : newEntity.OgrnName);
            parameters.AddWithValue("@GRAddress", (newEntity.GRAddress == null) ? (object)DBNull.Value : newEntity.GRAddress);
            parameters.AddWithValue("@InnDate", (newEntity.InnDate == null) ? (object)DBNull.Value : newEntity.InnDate);
            parameters.AddWithValue("@InnName", (newEntity.InnName == null) ? (object)DBNull.Value : newEntity.InnName);
            parameters.AddWithValue("@InnNum", (newEntity.InnNum == null) ? (object)DBNull.Value : newEntity.InnNum);
            parameters.AddWithValue("@Address_Fact", (newEntity.Address_Fact == null) ? (object)DBNull.Value : newEntity.Address_Fact);
            parameters.AddWithValue("@Country_Fact", (newEntity.Country_Fact == null) ? (object)DBNull.Value : newEntity.Country_Fact);
            parameters.AddWithValue("@PostalCode_Fact", (newEntity.PostalCode_Fact == null) ? (object)DBNull.Value : newEntity.PostalCode_Fact);
            parameters.AddWithValue("@Locality_Fact", (newEntity.Locality_Fact == null) ? (object)DBNull.Value : newEntity.Locality_Fact);
            parameters.AddWithValue("@Region_Fact", (newEntity.Region_Fact == null) ? (object)DBNull.Value : newEntity.Region_Fact);
            parameters.AddWithValue("@City_Fact", (newEntity.City_Fact == null) ? (object)DBNull.Value : newEntity.City_Fact);
            parameters.AddWithValue("@Town_Fact", (newEntity.Town_Fact == null) ? (object)DBNull.Value : newEntity.Town_Fact);
            parameters.AddWithValue("@Street_Fact", (newEntity.Street_Fact == null) ? (object)DBNull.Value : newEntity.Street_Fact);
            parameters.AddWithValue("@House_Fact", (newEntity.House_Fact == null) ? (object)DBNull.Value : newEntity.House_Fact);
            parameters.AddWithValue("@Building_Fact", (newEntity.Building_Fact == null) ? (object)DBNull.Value : newEntity.Building_Fact);
            parameters.AddWithValue("@Structure_Fact", (newEntity.Structure_Fact == null) ? (object)DBNull.Value : newEntity.Structure_Fact);
            parameters.AddWithValue("@Facility_Fact", (newEntity.Facility_Fact == null) ? (object)DBNull.Value : newEntity.Facility_Fact);
            parameters.AddWithValue("@Ownership_Fact", (newEntity.Ownership_Fact == null) ? (object)DBNull.Value : newEntity.Ownership_Fact);
            parameters.AddWithValue("@Flat_Fact", (newEntity.Flat_Fact == null) ? (object)DBNull.Value : newEntity.Flat_Fact);
            parameters.AddWithValue("@Gps", (newEntity.Gps == null) ? (object)DBNull.Value : newEntity.Gps);
            parameters.AddWithValue("@Taxometr", (newEntity.Taxometr == null) ? (object)DBNull.Value : newEntity.Taxometr);
            parameters.AddWithValue("@TODate", (newEntity.TODate == null) ? (object)DBNull.Value : newEntity.TODate);
            parameters.AddWithValue("@STSNumber", (newEntity.STSNumber == null) ? (object)DBNull.Value : newEntity.STSNumber);
            parameters.AddWithValue("@STSDate", (newEntity.STSDate == null) ? (object)DBNull.Value : newEntity.STSDate);
            parameters.AddWithValue("@OwnType", (newEntity.OwnType == null) ? (object)DBNull.Value : newEntity.OwnType);
            parameters.AddWithValue("@OwnNumber", (newEntity.OwnNumber == null) ? (object)DBNull.Value : newEntity.OwnNumber);
            parameters.AddWithValue("@OwnDate", (newEntity.OwnDate == null) ? (object)DBNull.Value : newEntity.OwnDate);
            parameters.AddWithValue("@MO", (newEntity.MO == null) ? (object)DBNull.Value : newEntity.MO);
            parameters.AddWithValue("@GUID_MO", (newEntity.GUID_MO == null) ? (object)DBNull.Value : newEntity.GUID_MO);
            parameters.AddWithValue("@DATE_MO", (newEntity.DATE_MO == null) ? (object)DBNull.Value : newEntity.DATE_MO);
            parameters.AddWithValue("@Obsolete", (newEntity.Obsolete == null) ? (object)DBNull.Value : newEntity.Obsolete);
            parameters.AddWithValue("@DisableGibddSend", (newEntity.DisableGibddSend == null) ? (object)DBNull.Value : newEntity.DisableGibddSend);
        }
        public static void UpdateLicenseParams(License updateLicense, SqlParameterCollection parameters)
        {
            parameters.AddWithValue("@RegNumber", (updateLicense.RegNumber == null) ? (object)DBNull.Value : updateLicense.RegNumber);
            parameters.AddWithValue("@BlankSeries", (updateLicense.BlankSeries == null) ? (object)DBNull.Value : updateLicense.BlankSeries);
            parameters.AddWithValue("@BlankNo", (updateLicense.BlankNo == null) ? (object)DBNull.Value : updateLicense.BlankNo);
            parameters.AddWithValue("@OrgName", (updateLicense.OrgName == null) ? (object)DBNull.Value : updateLicense.OrgName);
            parameters.AddWithValue("@Ogrn", (updateLicense.Ogrn == null) ? (object)DBNull.Value : updateLicense.Ogrn);
            parameters.AddWithValue("@Inn", (updateLicense.Inn == null) ? (object)DBNull.Value : updateLicense.Inn);
            parameters.AddWithValue("@Parent", (updateLicense.Parent == null) ? (object)DBNull.Value : updateLicense.Parent);
            parameters.AddWithValue("@RootParent", (updateLicense.RootParent == null) ? (object)DBNull.Value : updateLicense.RootParent);
            parameters.AddWithValue("@Status", (updateLicense.Status == null) ? (object)DBNull.Value : updateLicense.Status);
            parameters.AddWithValue("@Document", (updateLicense.Document == null) ? (object)DBNull.Value : updateLicense.Document);

            var signatureParam = parameters.Add("@SignatureBinary", System.Data.SqlDbType.VarBinary, -1);
            signatureParam.Value = (updateLicense.SignatureBinary == null) ? (object)DBNull.Value : updateLicense.SignatureBinary;

            parameters.AddWithValue("@TaxiId", (updateLicense.TaxiId == null) ? (object)DBNull.Value : updateLicense.TaxiId);
            parameters.AddWithValue("@Lfb", (updateLicense.Lfb == null) ? (object)DBNull.Value : updateLicense.Lfb);
            parameters.AddWithValue("@JuridicalAddress", (updateLicense.JuridicalAddress == null) ? (object)DBNull.Value : updateLicense.JuridicalAddress);
            parameters.AddWithValue("@PhoneNumber", (updateLicense.PhoneNumber == null) ? (object)DBNull.Value : updateLicense.PhoneNumber);
            parameters.AddWithValue("@AddContactData", (updateLicense.AddContactData == null) ? (object)DBNull.Value : updateLicense.AddContactData);
            parameters.AddWithValue("@AccountAbbr", (updateLicense.AccountAbbr == null) ? (object)DBNull.Value : updateLicense.AccountAbbr);
            parameters.AddWithValue("@TaxiBrand", (updateLicense.TaxiBrand == null) ? (object)DBNull.Value : updateLicense.TaxiBrand);
            parameters.AddWithValue("@TaxiModel", (updateLicense.TaxiModel == null) ? (object)DBNull.Value : updateLicense.TaxiModel);
            parameters.AddWithValue("@TaxiStateNumber", (updateLicense.TaxiStateNumber == null) ? (object)DBNull.Value : updateLicense.TaxiStateNumber);
            parameters.AddWithValue("@TaxiYear", (updateLicense.TaxiYear == null) ? (object)DBNull.Value : updateLicense.TaxiYear);
            parameters.AddWithValue("@OutputDate", (updateLicense.OutputDate == null) ? (object)DBNull.Value : updateLicense.OutputDate);
            parameters.AddWithValue("@CreationDate", (updateLicense.CreationDate == null) ? (object)DBNull.Value : updateLicense.CreationDate);
            parameters.AddWithValue("@TillDate", (updateLicense.TillDate == null) ? (object)DBNull.Value : updateLicense.TillDate);
            parameters.AddWithValue("@TillSuspensionDate", (updateLicense.TillSuspensionDate == null) ? (object)DBNull.Value : updateLicense.TillSuspensionDate);
            parameters.AddWithValue("@CancellationReason", (updateLicense.CancellationReason == null) ? (object)DBNull.Value : updateLicense.CancellationReason);
            parameters.AddWithValue("@SuspensionReason", (updateLicense.SuspensionReason == null) ? (object)DBNull.Value : updateLicense.SuspensionReason);
            parameters.AddWithValue("@ChangeReason", (updateLicense.ChangeReason == null) ? (object)DBNull.Value : updateLicense.ChangeReason);
            parameters.AddWithValue("@InvalidReason", (updateLicense.InvalidReason == null) ? (object)DBNull.Value : updateLicense.InvalidReason);
            parameters.AddWithValue("@ShortName", (updateLicense.ShortName == null) ? (object)DBNull.Value : updateLicense.ShortName);
            parameters.AddWithValue("@LastName", (updateLicense.LastName == null) ? (object)DBNull.Value : updateLicense.LastName);
            parameters.AddWithValue("@FirstName", (updateLicense.FirstName == null) ? (object)DBNull.Value : updateLicense.FirstName);
            parameters.AddWithValue("@SecondName", (updateLicense.SecondName == null) ? (object)DBNull.Value : updateLicense.SecondName);
            parameters.AddWithValue("@OgrnDate", (updateLicense.OgrnDate == null) ? (object)DBNull.Value : updateLicense.OgrnDate);
            parameters.AddWithValue("@Country", (updateLicense.Country == null) ? (object)DBNull.Value : updateLicense.Country);
            parameters.AddWithValue("@PostalCode", (updateLicense.PostalCode == null) ? (object)DBNull.Value : updateLicense.PostalCode);
            parameters.AddWithValue("@Locality", (updateLicense.Locality == null) ? (object)DBNull.Value : updateLicense.Locality);
            parameters.AddWithValue("@Region", (updateLicense.Region == null) ? (object)DBNull.Value : updateLicense.Region);
            parameters.AddWithValue("@City", (updateLicense.City == null) ? (object)DBNull.Value : updateLicense.City);
            parameters.AddWithValue("@Town", (updateLicense.Town == null) ? (object)DBNull.Value : updateLicense.Town);
            parameters.AddWithValue("@Street", (updateLicense.Street == null) ? (object)DBNull.Value : updateLicense.Street);
            parameters.AddWithValue("@House", (updateLicense.House == null) ? (object)DBNull.Value : updateLicense.House);
            parameters.AddWithValue("@Building", (updateLicense.Building == null) ? (object)DBNull.Value : updateLicense.Building);
            parameters.AddWithValue("@Structure", (updateLicense.Structure == null) ? (object)DBNull.Value : updateLicense.Structure);
            parameters.AddWithValue("@Facility", (updateLicense.Facility == null) ? (object)DBNull.Value : updateLicense.Facility);
            parameters.AddWithValue("@Ownership", (updateLicense.Ownership == null) ? (object)DBNull.Value : updateLicense.Ownership);
            parameters.AddWithValue("@Flat", (updateLicense.Flat == null) ? (object)DBNull.Value : updateLicense.Flat);
            parameters.AddWithValue("@Fax", (updateLicense.Fax == null) ? (object)DBNull.Value : updateLicense.Fax);
            parameters.AddWithValue("@EMail", (updateLicense.EMail == null) ? (object)DBNull.Value : updateLicense.EMail);
            parameters.AddWithValue("@TaxiColor", (updateLicense.TaxiColor == null) ? (object)DBNull.Value : updateLicense.TaxiColor);
            parameters.AddWithValue("@TaxiNumberColor", (updateLicense.TaxiNumberColor == null) ? (object)DBNull.Value : updateLicense.TaxiNumberColor);
            parameters.AddWithValue("@TaxiVin", (updateLicense.TaxiVin == null) ? (object)DBNull.Value : updateLicense.TaxiVin);
            parameters.AddWithValue("@ChangeDate", (updateLicense.ChangeDate == null) ? (object)DBNull.Value : updateLicense.ChangeDate);
            parameters.AddWithValue("@Guid_OD", (updateLicense.Guid_OD == null) ? (object)DBNull.Value : updateLicense.Guid_OD);
            parameters.AddWithValue("@Date_OD", (updateLicense.Date_OD == null) ? (object)DBNull.Value : updateLicense.Date_OD);
            parameters.AddWithValue("@FromPortal", (updateLicense.FromPortal == null) ? (object)DBNull.Value : updateLicense.FromPortal);
            parameters.AddWithValue("@FirmName", (updateLicense.FirmName == null) ? (object)DBNull.Value : updateLicense.FirmName);
            parameters.AddWithValue("@Brand", (updateLicense.Brand == null) ? (object)DBNull.Value : updateLicense.Brand);
            parameters.AddWithValue("@OgrnNum", (updateLicense.OgrnNum == null) ? (object)DBNull.Value : updateLicense.OgrnNum);
            parameters.AddWithValue("@OgrnName", (updateLicense.OgrnName == null) ? (object)DBNull.Value : updateLicense.OgrnName);
            parameters.AddWithValue("@GRAddress", (updateLicense.GRAddress == null) ? (object)DBNull.Value : updateLicense.GRAddress);
            parameters.AddWithValue("@InnDate", (updateLicense.InnDate == null) ? (object)DBNull.Value : updateLicense.InnDate);
            parameters.AddWithValue("@InnName", (updateLicense.InnName == null) ? (object)DBNull.Value : updateLicense.InnName);
            parameters.AddWithValue("@InnNum", (updateLicense.InnNum == null) ? (object)DBNull.Value : updateLicense.InnNum);
            parameters.AddWithValue("@Address_Fact", (updateLicense.Address_Fact == null) ? (object)DBNull.Value : updateLicense.Address_Fact);
            parameters.AddWithValue("@Country_Fact", (updateLicense.Country_Fact == null) ? (object)DBNull.Value : updateLicense.Country_Fact);
            parameters.AddWithValue("@PostalCode_Fact", (updateLicense.PostalCode_Fact == null) ? (object)DBNull.Value : updateLicense.PostalCode_Fact);
            parameters.AddWithValue("@Locality_Fact", (updateLicense.Locality_Fact == null) ? (object)DBNull.Value : updateLicense.Locality_Fact);
            parameters.AddWithValue("@Region_Fact", (updateLicense.Region_Fact == null) ? (object)DBNull.Value : updateLicense.Region_Fact);
            parameters.AddWithValue("@City_Fact", (updateLicense.City_Fact == null) ? (object)DBNull.Value : updateLicense.City_Fact);
            parameters.AddWithValue("@Town_Fact", (updateLicense.Town_Fact == null) ? (object)DBNull.Value : updateLicense.Town_Fact);
            parameters.AddWithValue("@Street_Fact", (updateLicense.Street_Fact == null) ? (object)DBNull.Value : updateLicense.Street_Fact);
            parameters.AddWithValue("@House_Fact", (updateLicense.House_Fact == null) ? (object)DBNull.Value : updateLicense.House_Fact);
            parameters.AddWithValue("@Building_Fact", (updateLicense.Building_Fact == null) ? (object)DBNull.Value : updateLicense.Building_Fact);
            parameters.AddWithValue("@Structure_Fact", (updateLicense.Structure_Fact == null) ? (object)DBNull.Value : updateLicense.Structure_Fact);
            parameters.AddWithValue("@Facility_Fact", (updateLicense.Facility_Fact == null) ? (object)DBNull.Value : updateLicense.Facility_Fact);
            parameters.AddWithValue("@Ownership_Fact", (updateLicense.Ownership_Fact == null) ? (object)DBNull.Value : updateLicense.Ownership_Fact);
            parameters.AddWithValue("@Flat_Fact", (updateLicense.Flat_Fact == null) ? (object)DBNull.Value : updateLicense.Flat_Fact);
            parameters.AddWithValue("@Gps", (updateLicense.Gps == null) ? (object)DBNull.Value : updateLicense.Gps);
            parameters.AddWithValue("@Taxometr", (updateLicense.Taxometr == null) ? (object)DBNull.Value : updateLicense.Taxometr);
            parameters.AddWithValue("@TODate", (updateLicense.TODate == null) ? (object)DBNull.Value : updateLicense.TODate);
            parameters.AddWithValue("@STSNumber", (updateLicense.STSNumber == null) ? (object)DBNull.Value : updateLicense.STSNumber);
            parameters.AddWithValue("@STSDate", (updateLicense.STSDate == null) ? (object)DBNull.Value : updateLicense.STSDate);
            parameters.AddWithValue("@OwnType", (updateLicense.OwnType == null) ? (object)DBNull.Value : updateLicense.OwnType);
            parameters.AddWithValue("@OwnNumber", (updateLicense.OwnNumber == null) ? (object)DBNull.Value : updateLicense.OwnNumber);
            parameters.AddWithValue("@OwnDate", (updateLicense.OwnDate == null) ? (object)DBNull.Value : updateLicense.OwnDate);
            parameters.AddWithValue("@MO", (updateLicense.MO == null) ? (object)DBNull.Value : updateLicense.MO);
            parameters.AddWithValue("@GUID_MO", (updateLicense.GUID_MO == null) ? (object)DBNull.Value : updateLicense.GUID_MO);
            parameters.AddWithValue("@DATE_MO", (updateLicense.DATE_MO == null) ? (object)DBNull.Value : updateLicense.DATE_MO);
            parameters.AddWithValue("@Obsolete", (updateLicense.Obsolete == null) ? (object)DBNull.Value : updateLicense.Obsolete);
            parameters.AddWithValue("@DisableGibddSend", (updateLicense.DisableGibddSend == null) ? (object)DBNull.Value : updateLicense.DisableGibddSend);

            parameters.AddWithValue("@Id", updateLicense.Id);
        }
        #endregion
    }
}
