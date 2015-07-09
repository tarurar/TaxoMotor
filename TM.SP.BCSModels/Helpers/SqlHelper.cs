using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using TM.SP.BCSModels.Taxi;

namespace TM.SP.BCSModels.Helpers
{
    public static class SqlHelper
    {
        #region [methods]
// ReSharper disable once InconsistentNaming
        public static string LoadSQLStatement(string statementName)
        {
            string sqlStatement = string.Empty;

            const string namespacePart = "TM.SP.BCSModels.SQL";
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
            entity.Id                      = (Int32)reader["Id"];
            entity.Title                   = (reader["Title"] == DBNull.Value) ? null : reader["Title"].ToString();
            entity.RegNumber               = (reader["RegNumber"] == DBNull.Value) ? null : reader["RegNumber"].ToString();
            entity.BlankSeries             = (reader["BlankSeries"] == DBNull.Value) ? null : reader["BlankSeries"].ToString();
            entity.BlankNo                 = (reader["BlankNo"] == DBNull.Value) ? null : reader["BlankNo"].ToString();
            entity.OrgName                 = (reader["OrgName"] == DBNull.Value) ? null : reader["OrgName"].ToString();
            entity.Ogrn                    = (reader["Ogrn"] == DBNull.Value) ? null : reader["Ogrn"].ToString();
            entity.Inn                     = (reader["Inn"] == DBNull.Value) ? null : reader["Inn"].ToString();
            entity.Parent                  = reader["Parent"] as int?;
            entity.RootParent              = reader["RootParent"] as int?;
            entity.Status                  = reader["Status"] as int?;
            entity.Document                = (reader["Document"] == DBNull.Value) ? null : reader["Document"].ToString();
            entity.SignatureBinary         = (reader["SignatureBinary"] == DBNull.Value) ? null : reader["SignatureBinary"] as byte[];
            entity.TaxiId                  = reader["TaxiId"] as int?;
            entity.Lfb                     = (reader["Lfb"] == DBNull.Value) ? null : reader["Lfb"].ToString();
            entity.JuridicalAddress        = (reader["JuridicalAddress"] == DBNull.Value) ? null : reader["JuridicalAddress"].ToString();
            entity.PhoneNumber             = (reader["PhoneNumber"] == DBNull.Value) ? null : reader["PhoneNumber"].ToString();
            entity.AddContactData          = (reader["AddContactData"] == DBNull.Value) ? null : reader["AddContactData"].ToString();
            entity.AccountAbbr             = (reader["AccountAbbr"] == DBNull.Value) ? null : reader["AccountAbbr"].ToString();
            entity.TaxiBrand               = (reader["TaxiBrand"] == DBNull.Value) ? null : reader["TaxiBrand"].ToString();
            entity.TaxiModel               = (reader["TaxiModel"] == DBNull.Value) ? null : reader["TaxiModel"].ToString();
            entity.TaxiStateNumber         = (reader["TaxiStateNumber"] == DBNull.Value) ? null : reader["TaxiStateNumber"].ToString();
            entity.TaxiYear                = reader["TaxiYear"] as int?;
            entity.OutputDate              = reader["OutputDate"] as DateTime?;
            entity.CreationDate            = reader["CreationDate"] as DateTime?;
            entity.TillDate                = reader["TillDate"] as DateTime?;
            entity.TillSuspensionDate      = reader["TillSuspensionDate"] as DateTime?;
            entity.CancellationReason      = (reader["CancellationReason"] == DBNull.Value) ? null : reader["CancellationReason"].ToString();
            entity.SuspensionReason        = (reader["SuspensionReason"] == DBNull.Value) ? null : reader["SuspensionReason"].ToString();
            entity.ChangeReason            = (reader["ChangeReason"] == DBNull.Value) ? null : reader["ChangeReason"].ToString();
            entity.InvalidReason           = (reader["InvalidReason"] == DBNull.Value) ? null : reader["InvalidReason"].ToString();
            entity.ShortName               = (reader["ShortName"] == DBNull.Value) ? null : reader["ShortName"].ToString();
            entity.LastName                = (reader["LastName"] == DBNull.Value) ? null : reader["LastName"].ToString();
            entity.FirstName               = (reader["FirstName"] == DBNull.Value) ? null : reader["FirstName"].ToString();
            entity.SecondName              = (reader["SecondName"] == DBNull.Value) ? null : reader["SecondName"].ToString();
            entity.OgrnDate                = reader["OgrnDate"] as DateTime?;
            entity.Country                 = (reader["Country"] == DBNull.Value) ? null : reader["Country"].ToString();
            entity.PostalCode              = (reader["PostalCode"] == DBNull.Value) ? null : reader["PostalCode"].ToString();
            entity.Locality                = (reader["Locality"] == DBNull.Value) ? null : reader["Locality"].ToString();
            entity.Region                  = (reader["Region"] == DBNull.Value) ? null : reader["Region"].ToString();
            entity.City                    = (reader["City"] == DBNull.Value) ? null : reader["City"].ToString();
            entity.Town                    = (reader["Town"] == DBNull.Value) ? null : reader["Town"].ToString();
            entity.Street                  = (reader["Street"] == DBNull.Value) ? null : reader["Street"].ToString();
            entity.House                   = (reader["House"] == DBNull.Value) ? null : reader["House"].ToString();
            entity.Building                = (reader["Building"] == DBNull.Value) ? null : reader["Building"].ToString();
            entity.Structure               = (reader["Structure"] == DBNull.Value) ? null : reader["Structure"].ToString();
            entity.Facility                = (reader["Facility"] == DBNull.Value) ? null : reader["Facility"].ToString();
            entity.Ownership               = (reader["Ownership"] == DBNull.Value) ? null : reader["Ownership"].ToString();
            entity.Flat                    = (reader["Flat"] == DBNull.Value) ? null : reader["Flat"].ToString();
            entity.Fax                     = (reader["Fax"] == DBNull.Value) ? null : reader["Fax"].ToString();
            entity.EMail                   = (reader["EMail"] == DBNull.Value) ? null : reader["EMail"].ToString();
            entity.TaxiColor               = (reader["TaxiColor"] == DBNull.Value) ? null : reader["TaxiColor"].ToString();
            entity.TaxiNumberColor         = (reader["TaxiNumberColor"] == DBNull.Value) ? null : reader["TaxiNumberColor"].ToString();
            entity.TaxiVin                 = (reader["TaxiVin"] == DBNull.Value) ? null : reader["TaxiVin"].ToString();
            entity.ChangeDate              = reader["ChangeDate"] as DateTime?;
            entity.Guid_OD                 = (reader["Guid_OD"] == DBNull.Value) ? null : reader["Guid_OD"].ToString();
            entity.Date_OD                 = reader["Date_OD"] as DateTime?;
            entity.FromPortal              = reader["FromPortal"] as bool?;
            entity.FirmName                = (reader["FirmName"] == DBNull.Value) ? null : reader["FirmName"].ToString();
            entity.Brand                   = (reader["Brand"] == DBNull.Value) ? null : reader["Brand"].ToString();
            entity.OgrnNum                 = (reader["OgrnNum"] == DBNull.Value) ? null : reader["OgrnNum"].ToString();
            entity.OgrnName                = (reader["OgrnName"] == DBNull.Value) ? null : reader["OgrnName"].ToString();
            entity.GRAddress               = (reader["GRAddress"] == DBNull.Value) ? null : reader["GRAddress"].ToString();
            entity.InnDate                 = reader["InnDate"] as DateTime?;
            entity.InnName                 = (reader["InnName"] == DBNull.Value) ? null : reader["InnName"].ToString();
            entity.InnNum                  = (reader["InnNum"] == DBNull.Value) ? null : reader["InnNum"].ToString();
            entity.Address_Fact            = (reader["Address_Fact"] == DBNull.Value) ? null : reader["Address_Fact"].ToString();
            entity.Country_Fact            = (reader["Country_Fact"] == DBNull.Value) ? null : reader["Country_Fact"].ToString();
            entity.PostalCode_Fact         = (reader["PostalCode_Fact"] == DBNull.Value) ? null : reader["PostalCode_Fact"].ToString();
            entity.Locality_Fact           = (reader["Locality_Fact"] == DBNull.Value) ? null : reader["Locality_Fact"].ToString();
            entity.Region_Fact             = (reader["Region_Fact"] == DBNull.Value) ? null : reader["Region_Fact"].ToString();
            entity.City_Fact               = (reader["City_Fact"] == DBNull.Value) ? null : reader["City_Fact"].ToString();
            entity.Town_Fact               = (reader["Town_Fact"] == DBNull.Value) ? null : reader["Town_Fact"].ToString();
            entity.Street_Fact             = (reader["Street_Fact"] == DBNull.Value) ? null : reader["Street_Fact"].ToString();
            entity.House_Fact              = (reader["House_Fact"] == DBNull.Value) ? null : reader["House_Fact"].ToString();
            entity.Building_Fact           = (reader["Building_Fact"] == DBNull.Value) ? null : reader["Building_Fact"].ToString();
            entity.Structure_Fact          = (reader["Structure_Fact"] == DBNull.Value) ? null : reader["Structure_Fact"].ToString();
            entity.Facility_Fact           = (reader["Facility_Fact"] == DBNull.Value) ? null : reader["Facility_Fact"].ToString();
            entity.Ownership_Fact          = (reader["Ownership_Fact"] == DBNull.Value) ? null : reader["Ownership_Fact"].ToString();
            entity.Flat_Fact               = (reader["Flat_Fact"] == DBNull.Value) ? null : reader["Flat_Fact"].ToString();
            entity.Gps                     = reader["Gps"] as bool?;
            entity.Taxometr                = reader["Taxometr"] as bool?;
            entity.TODate                  = reader["TODate"] as DateTime?;
            entity.STSNumber               = (reader["STSNumber"] == DBNull.Value) ? null : reader["STSNumber"].ToString();
            entity.STSDate                 = reader["STSDate"] as DateTime?;
            entity.OwnType                 = reader["OwnType"] as int?;
            entity.OwnNumber               = (reader["OwnNumber"] == DBNull.Value) ? null : reader["OwnNumber"].ToString();
            entity.OwnDate                 = reader["OwnDate"] as DateTime?;
            entity.MO                      = reader["MO"] as bool?;
            entity.GUID_MO                 = (reader["GUID_MO"] == DBNull.Value) ? null : reader["GUID_MO"].ToString();
            entity.DATE_MO                 = reader["DATE_MO"] as DateTime?;
            entity.Obsolete                = reader["Obsolete"] as bool?;
            entity.DisableGibddSend        = reader["DisableGibddSend"] as bool?;
            entity.HasAnyChilds            = reader["HasAnyChilds"] as bool?;
            entity.LastRequestSendDate     = reader["LastRequestSendDate"] as DateTime?;
            entity.ObsoleteComment         = (reader["ObsoleteComment"] == DBNull.Value) ? null : reader["ObsoleteComment"].ToString();
            entity.DisableGibddSendComment = (reader["DisableGibddSendComment"] == DBNull.Value) ? null : reader["DisableGibddSendComment"].ToString();
        }
        public static void NewLicenseParams(License newEntity, SqlParameterCollection parameters)
        {
            parameters.AddWithValue("@RegNumber", newEntity.RegNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@BlankSeries", newEntity.BlankSeries ?? (object)DBNull.Value);
            parameters.AddWithValue("@BlankNo", newEntity.BlankNo ?? (object)DBNull.Value);
            parameters.AddWithValue("@OrgName", newEntity.OrgName ?? (object)DBNull.Value);
            parameters.AddWithValue("@Ogrn", newEntity.Ogrn ?? (object)DBNull.Value);
            parameters.AddWithValue("@Inn", newEntity.Inn ?? (object)DBNull.Value);
            parameters.AddWithValue("@Parent", newEntity.Parent ?? (object)DBNull.Value);
            parameters.AddWithValue("@RootParent", newEntity.RootParent ?? (object)DBNull.Value);
            parameters.AddWithValue("@Status", newEntity.Status ?? (object)DBNull.Value);
            parameters.AddWithValue("@Document", newEntity.Document ?? (object)DBNull.Value);

            var signatureParam = parameters.Add("@SignatureBinary", SqlDbType.VarBinary, -1);
            signatureParam.Value = newEntity.SignatureBinary ?? (object)DBNull.Value;

            parameters.AddWithValue("@TaxiId", newEntity.TaxiId ?? (object)DBNull.Value);
            parameters.AddWithValue("@Lfb", newEntity.Lfb ?? (object)DBNull.Value);
            parameters.AddWithValue("@JuridicalAddress", newEntity.JuridicalAddress ?? (object)DBNull.Value);
            parameters.AddWithValue("@PhoneNumber", newEntity.PhoneNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@AddContactData", newEntity.AddContactData ?? (object)DBNull.Value);
            parameters.AddWithValue("@AccountAbbr", newEntity.AccountAbbr ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiBrand", newEntity.TaxiBrand ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiModel", newEntity.TaxiModel ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiStateNumber", newEntity.TaxiStateNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiYear", newEntity.TaxiYear ?? (object)DBNull.Value);
            parameters.AddWithValue("@OutputDate", newEntity.OutputDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@CreationDate", newEntity.CreationDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@TillDate", newEntity.TillDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@TillSuspensionDate", newEntity.TillSuspensionDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@CancellationReason", newEntity.CancellationReason ?? (object)DBNull.Value);
            parameters.AddWithValue("@SuspensionReason", newEntity.SuspensionReason ?? (object)DBNull.Value);
            parameters.AddWithValue("@ChangeReason", newEntity.ChangeReason ?? (object)DBNull.Value);
            parameters.AddWithValue("@InvalidReason", newEntity.InvalidReason ?? (object)DBNull.Value);
            parameters.AddWithValue("@ShortName", newEntity.ShortName ?? (object)DBNull.Value);
            parameters.AddWithValue("@LastName", newEntity.LastName ?? (object)DBNull.Value);
            parameters.AddWithValue("@FirstName", newEntity.FirstName ?? (object)DBNull.Value);
            parameters.AddWithValue("@SecondName", newEntity.SecondName ?? (object)DBNull.Value);
            parameters.AddWithValue("@OgrnDate", newEntity.OgrnDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@Country", newEntity.Country ?? (object)DBNull.Value);
            parameters.AddWithValue("@PostalCode", newEntity.PostalCode ?? (object)DBNull.Value);
            parameters.AddWithValue("@Locality", newEntity.Locality ?? (object)DBNull.Value);
            parameters.AddWithValue("@Region", newEntity.Region ?? (object)DBNull.Value);
            parameters.AddWithValue("@City", newEntity.City ?? (object)DBNull.Value);
            parameters.AddWithValue("@Town", newEntity.Town ?? (object)DBNull.Value);
            parameters.AddWithValue("@Street", newEntity.Street ?? (object)DBNull.Value);
            parameters.AddWithValue("@House", newEntity.House ?? (object)DBNull.Value);
            parameters.AddWithValue("@Building", newEntity.Building ?? (object)DBNull.Value);
            parameters.AddWithValue("@Structure", newEntity.Structure ?? (object)DBNull.Value);
            parameters.AddWithValue("@Facility", newEntity.Facility ?? (object)DBNull.Value);
            parameters.AddWithValue("@Ownership", newEntity.Ownership ?? (object)DBNull.Value);
            parameters.AddWithValue("@Flat", newEntity.Flat ?? (object)DBNull.Value);
            parameters.AddWithValue("@Fax", newEntity.Fax ?? (object)DBNull.Value);
            parameters.AddWithValue("@EMail", newEntity.EMail ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiColor", newEntity.TaxiColor ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiNumberColor", newEntity.TaxiNumberColor ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiVin", newEntity.TaxiVin ?? (object)DBNull.Value);
            parameters.AddWithValue("@ChangeDate", newEntity.ChangeDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@Guid_OD", newEntity.Guid_OD ?? (object)DBNull.Value);
            parameters.AddWithValue("@Date_OD", newEntity.Date_OD ?? (object)DBNull.Value);
            parameters.AddWithValue("@FromPortal", newEntity.FromPortal ?? (object)DBNull.Value);
            parameters.AddWithValue("@FirmName", newEntity.FirmName ?? (object)DBNull.Value);
            parameters.AddWithValue("@Brand", newEntity.Brand ?? (object)DBNull.Value);
            parameters.AddWithValue("@OgrnNum", newEntity.OgrnNum ?? (object)DBNull.Value);
            parameters.AddWithValue("@OgrnName", newEntity.OgrnName ?? (object)DBNull.Value);
            parameters.AddWithValue("@GRAddress", newEntity.GRAddress ?? (object)DBNull.Value);
            parameters.AddWithValue("@InnDate", newEntity.InnDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@InnName", newEntity.InnName ?? (object)DBNull.Value);
            parameters.AddWithValue("@InnNum", newEntity.InnNum ?? (object)DBNull.Value);
            parameters.AddWithValue("@Address_Fact", newEntity.Address_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Country_Fact", newEntity.Country_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@PostalCode_Fact", newEntity.PostalCode_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Locality_Fact", newEntity.Locality_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Region_Fact", newEntity.Region_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@City_Fact", newEntity.City_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Town_Fact", newEntity.Town_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Street_Fact", newEntity.Street_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@House_Fact", newEntity.House_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Building_Fact", newEntity.Building_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Structure_Fact", newEntity.Structure_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Facility_Fact", newEntity.Facility_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Ownership_Fact", newEntity.Ownership_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Flat_Fact", newEntity.Flat_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Gps", newEntity.Gps ?? (object)DBNull.Value);
            parameters.AddWithValue("@Taxometr", newEntity.Taxometr ?? (object)DBNull.Value);
            parameters.AddWithValue("@TODate", newEntity.TODate ?? (object)DBNull.Value);
            parameters.AddWithValue("@STSNumber", newEntity.STSNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@STSDate", newEntity.STSDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@OwnType", newEntity.OwnType ?? (object)DBNull.Value);
            parameters.AddWithValue("@OwnNumber", newEntity.OwnNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@OwnDate", newEntity.OwnDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@MO", newEntity.MO ?? (object)DBNull.Value);
            parameters.AddWithValue("@GUID_MO", newEntity.GUID_MO ?? (object)DBNull.Value);
            parameters.AddWithValue("@DATE_MO", newEntity.DATE_MO ?? (object)DBNull.Value);
            parameters.AddWithValue("@Obsolete", newEntity.Obsolete ?? (object)DBNull.Value);
            parameters.AddWithValue("@DisableGibddSend", newEntity.DisableGibddSend ?? (object)DBNull.Value);
            parameters.AddWithValue("@LastRequestSendDate", newEntity.LastRequestSendDate ?? (object) DBNull.Value);
            parameters.AddWithValue("@ObsoleteComment", newEntity.ObsoleteComment ?? (object)DBNull.Value);
            parameters.AddWithValue("@DisableGibddSendComment", newEntity.DisableGibddSendComment ?? (object)DBNull.Value);
        }
        public static void UpdateLicenseParams(License updateLicense, SqlParameterCollection parameters)
        {
            parameters.AddWithValue("@RegNumber", updateLicense.RegNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@BlankSeries", updateLicense.BlankSeries ?? (object)DBNull.Value);
            parameters.AddWithValue("@BlankNo", updateLicense.BlankNo ?? (object)DBNull.Value);
            parameters.AddWithValue("@OrgName", updateLicense.OrgName ?? (object)DBNull.Value);
            parameters.AddWithValue("@Ogrn", updateLicense.Ogrn ?? (object)DBNull.Value);
            parameters.AddWithValue("@Inn", updateLicense.Inn ?? (object)DBNull.Value);
            parameters.AddWithValue("@Parent", updateLicense.Parent ?? (object)DBNull.Value);
            parameters.AddWithValue("@RootParent", updateLicense.RootParent ?? (object)DBNull.Value);
            parameters.AddWithValue("@Status", updateLicense.Status ?? (object)DBNull.Value);
            parameters.AddWithValue("@Document", updateLicense.Document ?? (object)DBNull.Value);

            var signatureParam = parameters.Add("@SignatureBinary", SqlDbType.VarBinary, -1);
            signatureParam.Value = updateLicense.SignatureBinary ?? (object)DBNull.Value;

            parameters.AddWithValue("@TaxiId", updateLicense.TaxiId ?? (object)DBNull.Value);
            parameters.AddWithValue("@Lfb", updateLicense.Lfb ?? (object)DBNull.Value);
            parameters.AddWithValue("@JuridicalAddress", updateLicense.JuridicalAddress ?? (object)DBNull.Value);
            parameters.AddWithValue("@PhoneNumber", updateLicense.PhoneNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@AddContactData", updateLicense.AddContactData ?? (object)DBNull.Value);
            parameters.AddWithValue("@AccountAbbr", updateLicense.AccountAbbr ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiBrand", updateLicense.TaxiBrand ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiModel", updateLicense.TaxiModel ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiStateNumber", updateLicense.TaxiStateNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiYear", updateLicense.TaxiYear ?? (object)DBNull.Value);
            parameters.AddWithValue("@OutputDate", updateLicense.OutputDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@CreationDate", updateLicense.CreationDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@TillDate", updateLicense.TillDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@TillSuspensionDate", updateLicense.TillSuspensionDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@CancellationReason", updateLicense.CancellationReason ?? (object)DBNull.Value);
            parameters.AddWithValue("@SuspensionReason", updateLicense.SuspensionReason ?? (object)DBNull.Value);
            parameters.AddWithValue("@ChangeReason", updateLicense.ChangeReason ?? (object)DBNull.Value);
            parameters.AddWithValue("@InvalidReason", updateLicense.InvalidReason ?? (object)DBNull.Value);
            parameters.AddWithValue("@ShortName", updateLicense.ShortName ?? (object)DBNull.Value);
            parameters.AddWithValue("@LastName", updateLicense.LastName ?? (object)DBNull.Value);
            parameters.AddWithValue("@FirstName", updateLicense.FirstName ?? (object)DBNull.Value);
            parameters.AddWithValue("@SecondName", updateLicense.SecondName ?? (object)DBNull.Value);
            parameters.AddWithValue("@OgrnDate", updateLicense.OgrnDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@Country", updateLicense.Country ?? (object)DBNull.Value);
            parameters.AddWithValue("@PostalCode", updateLicense.PostalCode ?? (object)DBNull.Value);
            parameters.AddWithValue("@Locality", updateLicense.Locality ?? (object)DBNull.Value);
            parameters.AddWithValue("@Region", updateLicense.Region ?? (object)DBNull.Value);
            parameters.AddWithValue("@City", updateLicense.City ?? (object)DBNull.Value);
            parameters.AddWithValue("@Town", updateLicense.Town ?? (object)DBNull.Value);
            parameters.AddWithValue("@Street", updateLicense.Street ?? (object)DBNull.Value);
            parameters.AddWithValue("@House", updateLicense.House ?? (object)DBNull.Value);
            parameters.AddWithValue("@Building", updateLicense.Building ?? (object)DBNull.Value);
            parameters.AddWithValue("@Structure", updateLicense.Structure ?? (object)DBNull.Value);
            parameters.AddWithValue("@Facility", updateLicense.Facility ?? (object)DBNull.Value);
            parameters.AddWithValue("@Ownership", updateLicense.Ownership ?? (object)DBNull.Value);
            parameters.AddWithValue("@Flat", updateLicense.Flat ?? (object)DBNull.Value);
            parameters.AddWithValue("@Fax", updateLicense.Fax ?? (object)DBNull.Value);
            parameters.AddWithValue("@EMail", updateLicense.EMail ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiColor", updateLicense.TaxiColor ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiNumberColor", updateLicense.TaxiNumberColor ?? (object)DBNull.Value);
            parameters.AddWithValue("@TaxiVin", updateLicense.TaxiVin ?? (object)DBNull.Value);
            parameters.AddWithValue("@ChangeDate", updateLicense.ChangeDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@Guid_OD", updateLicense.Guid_OD ?? (object)DBNull.Value);
            parameters.AddWithValue("@Date_OD", updateLicense.Date_OD ?? (object)DBNull.Value);
            parameters.AddWithValue("@FromPortal", updateLicense.FromPortal ?? (object)DBNull.Value);
            parameters.AddWithValue("@FirmName", updateLicense.FirmName ?? (object)DBNull.Value);
            parameters.AddWithValue("@Brand", updateLicense.Brand ?? (object)DBNull.Value);
            parameters.AddWithValue("@OgrnNum", updateLicense.OgrnNum ?? (object)DBNull.Value);
            parameters.AddWithValue("@OgrnName", updateLicense.OgrnName ?? (object)DBNull.Value);
            parameters.AddWithValue("@GRAddress", updateLicense.GRAddress ?? (object)DBNull.Value);
            parameters.AddWithValue("@InnDate", updateLicense.InnDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@InnName", updateLicense.InnName ?? (object)DBNull.Value);
            parameters.AddWithValue("@InnNum", updateLicense.InnNum ?? (object)DBNull.Value);
            parameters.AddWithValue("@Address_Fact", updateLicense.Address_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Country_Fact", updateLicense.Country_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@PostalCode_Fact", updateLicense.PostalCode_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Locality_Fact", updateLicense.Locality_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Region_Fact", updateLicense.Region_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@City_Fact", updateLicense.City_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Town_Fact", updateLicense.Town_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Street_Fact", updateLicense.Street_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@House_Fact", updateLicense.House_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Building_Fact", updateLicense.Building_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Structure_Fact", updateLicense.Structure_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Facility_Fact", updateLicense.Facility_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Ownership_Fact", updateLicense.Ownership_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Flat_Fact", updateLicense.Flat_Fact ?? (object)DBNull.Value);
            parameters.AddWithValue("@Gps", updateLicense.Gps ?? (object)DBNull.Value);
            parameters.AddWithValue("@Taxometr", updateLicense.Taxometr ?? (object)DBNull.Value);
            parameters.AddWithValue("@TODate", updateLicense.TODate ?? (object)DBNull.Value);
            parameters.AddWithValue("@STSNumber", updateLicense.STSNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@STSDate", updateLicense.STSDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@OwnType", updateLicense.OwnType ?? (object)DBNull.Value);
            parameters.AddWithValue("@OwnNumber", updateLicense.OwnNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@OwnDate", updateLicense.OwnDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@MO", updateLicense.MO ?? (object)DBNull.Value);
            parameters.AddWithValue("@GUID_MO", updateLicense.GUID_MO ?? (object)DBNull.Value);
            parameters.AddWithValue("@DATE_MO", updateLicense.DATE_MO ?? (object)DBNull.Value);
            parameters.AddWithValue("@Obsolete", updateLicense.Obsolete ?? (object)DBNull.Value);
            parameters.AddWithValue("@DisableGibddSend", updateLicense.DisableGibddSend ?? (object)DBNull.Value);
            parameters.AddWithValue("@LastRequestSendDate", updateLicense.LastRequestSendDate ?? (object)DBNull.Value);
            parameters.AddWithValue("@ObsoleteComment", updateLicense.ObsoleteComment ?? (object)DBNull.Value);
            parameters.AddWithValue("@DisableGibddSendComment", updateLicense.DisableGibddSendComment ?? (object)DBNull.Value);

            parameters.AddWithValue("@Id", updateLicense.Id);
        }
        #endregion
    }
}
