using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace TM.SP.BCSModels.Taxi
{
    public partial class LicenseEntityService
    {
        public MigratingLicense TakeItemForMigration(int ItemId)
        {
            MigratingLicense retVal = null;

            var thisConn = getSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand {Connection = thisConn};

            const string selectAnyText = @"INSERT INTO [dbo].[LicenseMigrationTicket] ([Status], [StartDate], [LicenseId])
                                            OUTPUT INSERTED.[Id], INSERTED.[Status], INSERTED.[LicenseId]
                                            SELECT TOP 1 @Status, GETDATE(), l.[Id] FROM [dbo].[License] l
                                            LEFT JOIN [dbo].[LicenseMigrationTicket] m on m.[LicenseId] = l.[Id]
                                            WHERE m.[Title] IS NULL AND l.[Status] <> 4
                                            ORDER BY l.[Parent] ASC"; // we need an order instruction to be sure in the parent existance
                
            const string selectItemText = @"INSERT INTO [dbo].[LicenseMigrationTicket] ([Status], [StartDate], [LicenseId])
                                            OUTPUT INSERTED.[Id], INSERTED.[Status], INSERTED.[LicenseId]
                                            SELECT @Status, GETDATE(), l.[Id] FROM [dbo].[License] l
                                            LEFT JOIN [dbo].[LicenseMigrationTicket] m on m.[LicenseId] = l.[Id]
                                            WHERE m.[Title] IS NULL AND l.[Id] = @ItemId AND l.[Status] <> 4
                                            ORDER BY l.[Parent] ASC";

            if (ItemId == 0)
            {
                selectCommand.CommandText = selectAnyText;
            }
            else
            {
                selectCommand.CommandText = selectItemText;
                selectCommand.Parameters.AddWithValue("@ItemId", ItemId);
            }
            selectCommand.Parameters.AddWithValue("@Status", (Int32)MigratingStatus.Reserved);
            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                retVal = new MigratingLicense()
                {
                    TicketId = (System.Int32)thisReader["Id"],
                    Status = thisReader["Status"] == DBNull.Value ? (System.Int32)MigratingStatus.Undefined : (System.Int32)thisReader["Status"],
                    ItemId = (System.Int32)thisReader["LicenseId"],
                    ErrorInfo = String.Empty,
                    StackInfo = String.Empty
                };
            }

            return retVal;
        }

        public void FinishMigration(MigratingLicense migratingLicense)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = getSqlConnection();
                thisConn.Open();

                SqlCommand updateCommand = new SqlCommand() { Connection = thisConn };
                updateCommand.CommandText = @"UPDATE [dbo].[LicenseMigrationTicket]
                                            SET [Status] = @Status, [FinishDate] = GETDATE(), [ErrorInfo] = @ErrorInfo, [StackTrace] = @StackTrace
                                            WHERE [Id] = @Id";

                var errorInfo = String.IsNullOrEmpty(migratingLicense.ErrorInfo) ? (object)DBNull.Value : migratingLicense.ErrorInfo;
                var stackTrace = String.IsNullOrEmpty(migratingLicense.StackInfo) ? (object)DBNull.Value : migratingLicense.StackInfo;

                updateCommand.Parameters.AddWithValue("@Status", migratingLicense.Status);
                updateCommand.Parameters.AddWithValue("@ErrorInfo", errorInfo);
                updateCommand.Parameters.AddWithValue("@StackTrace", stackTrace);
                updateCommand.Parameters.AddWithValue("@Id", migratingLicense.TicketId);

                updateCommand.ExecuteNonQuery();
            }
            finally
            {
                thisConn.Dispose();
            }
        }

        public void UpdateMigrationStatus(MigratingLicense request)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = getSqlConnection();
                thisConn.Open();

                SqlCommand updateCommand = new SqlCommand() { Connection = thisConn };
                updateCommand.CommandText = @"UPDATE [dbo].[LicenseMigrationTicket]
                                            SET [Status] = @Status
                                            WHERE [Id] = @Id";
                updateCommand.Parameters.AddWithValue("@Status", request.Status);
                updateCommand.Parameters.AddWithValue("@Id", request.TicketId);

                updateCommand.ExecuteNonQuery();
            }
            finally
            {
                thisConn.Dispose();
            }
        }

        public void DeleteLicenseDraftForSPTaxiId(int spTaxiId)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = getSqlConnection();
                thisConn.Open();

                var deleteCommand = new SqlCommand {
                    Connection = thisConn, 
                    CommandText = @" DELETE
                                    FROM [dbo].[LicenseMigrationTicket]
                                    WHERE LicenseId IN (
		                                    SELECT Id
		                                    FROM [dbo].[License]
		                                    WHERE [Status] = @Status
			                                    AND [TaxiId] = @TaxiId
		                                    );

                                    DELETE
                                    FROM [dbo].[License]
                                    WHERE [Status] = @Status
	                                    AND [TaxiId] = @TaxiId;
                                    "};
                deleteCommand.Parameters.AddWithValue("@Status", 4);
                deleteCommand.Parameters.AddWithValue("@TaxiId", spTaxiId);

                deleteCommand.ExecuteNonQuery();
            }
            finally
            {
                thisConn.Dispose();
            }
        }

        public License GetLicenseDraftForSPTaxiId(int spTaxiId)
        {
            License entity = new License();
            SqlConnection thisConn = getSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand
            {
                CommandText =
                    "SELECT TOP 1 [Id] , [Title] , [RegNumber] , [BlankSeries] , [BlankNo] , [OrgName] , [Ogrn] , [Inn] , [Parent] , [RootParent] , [Status] , [Document] , [Signature] , [TaxiId] , [Lfb] , [JuridicalAddress] , [PhoneNumber] , [AddContactData] , [AccountAbbr] , [TaxiBrand] , [TaxiModel] , [TaxiStateNumber] , [TaxiYear] , [OutputDate] , [CreationDate] , [TillDate] , [TillSuspensionDate] , [CancellationReason] , [SuspensionReason] , [ChangeReason] , [InvalidReason] , [ShortName] , [LastName] , [FirstName] , [SecondName] , [OgrnDate] , [Country] , [PostalCode] , [Locality] , [Region] , [City] , [Town] , [Street] , [House] , [Building] , [Structure] , [Facility] , [Ownership] , [Flat] , [Fax] , [EMail] , [TaxiColor] , [TaxiNumberColor] , [TaxiVin] , [ChangeDate] , [Guid_OD] , [Date_OD] , [FromPortal] , [FirmName] , [Brand] , [OgrnNum] , [OgrnName] , [GRAddress] , [InnDate] , [InnName] , [InnNum] , [Address_Fact] , [Country_Fact] , [PostalCode_Fact] , [Locality_Fact] , [Region_Fact] , [City_Fact] , [Town_Fact] , [Street_Fact] , [House_Fact] , [Building_Fact] , [Structure_Fact] , [Facility_Fact] , [Ownership_Fact] , [Flat_Fact] , [Gps] , [Taxometr] , [TODate] , [STSNumber] , [STSDate] , [OwnType] , [OwnNumber] , [OwnDate] , [MO] , [GUID_MO] , [DATE_MO] FROM [dbo].[License] WHERE [TaxiId] = @TaxiId AND [Status] = @Status ORDER BY [Id] DESC"
            };
            selectCommand.Parameters.AddWithValue("@TaxiId", spTaxiId);
            selectCommand.Parameters.AddWithValue("@Status", 4);

            selectCommand.Connection = thisConn;
            SqlDataReader thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                entity.Id = (System.Int32)thisReader["Id"];
                entity.Title = (thisReader["Title"] == DBNull.Value) ? null : thisReader["Title"].ToString();
                entity.RegNumber = (thisReader["RegNumber"] == DBNull.Value) ? null : thisReader["RegNumber"].ToString();
                entity.BlankSeries = (thisReader["BlankSeries"] == DBNull.Value) ? null : thisReader["BlankSeries"].ToString();
                entity.BlankNo = (thisReader["BlankNo"] == DBNull.Value) ? null : thisReader["BlankNo"].ToString();
                entity.OrgName = (thisReader["OrgName"] == DBNull.Value) ? null : thisReader["OrgName"].ToString();
                entity.Ogrn = (thisReader["Ogrn"] == DBNull.Value) ? null : thisReader["Ogrn"].ToString();
                entity.Inn = (thisReader["Inn"] == DBNull.Value) ? null : thisReader["Inn"].ToString();
                entity.Parent = thisReader["Parent"] as System.Nullable<System.Int32>;
                entity.RootParent = thisReader["RootParent"] as System.Nullable<System.Int32>;
                entity.Status = thisReader["Status"] as System.Nullable<System.Int32>;
                entity.Document = (thisReader["Document"] == DBNull.Value) ? null : thisReader["Document"].ToString();
                entity.Signature = (thisReader["Signature"] == DBNull.Value) ? null : thisReader["Signature"].ToString();
                entity.TaxiId = thisReader["TaxiId"] as System.Nullable<System.Int32>;
                entity.Lfb = (thisReader["Lfb"] == DBNull.Value) ? null : thisReader["Lfb"].ToString();
                entity.JuridicalAddress = (thisReader["JuridicalAddress"] == DBNull.Value) ? null : thisReader["JuridicalAddress"].ToString();
                entity.PhoneNumber = (thisReader["PhoneNumber"] == DBNull.Value) ? null : thisReader["PhoneNumber"].ToString();
                entity.AddContactData = (thisReader["AddContactData"] == DBNull.Value) ? null : thisReader["AddContactData"].ToString();
                entity.AccountAbbr = (thisReader["AccountAbbr"] == DBNull.Value) ? null : thisReader["AccountAbbr"].ToString();
                entity.TaxiBrand = (thisReader["TaxiBrand"] == DBNull.Value) ? null : thisReader["TaxiBrand"].ToString();
                entity.TaxiModel = (thisReader["TaxiModel"] == DBNull.Value) ? null : thisReader["TaxiModel"].ToString();
                entity.TaxiStateNumber = (thisReader["TaxiStateNumber"] == DBNull.Value) ? null : thisReader["TaxiStateNumber"].ToString();
                entity.TaxiYear = thisReader["TaxiYear"] as System.Nullable<System.Int32>;
                entity.OutputDate = thisReader["OutputDate"] as System.Nullable<System.DateTime>;
                entity.CreationDate = thisReader["CreationDate"] as System.Nullable<System.DateTime>;
                entity.TillDate = thisReader["TillDate"] as System.Nullable<System.DateTime>;
                entity.TillSuspensionDate = thisReader["TillSuspensionDate"] as System.Nullable<System.DateTime>;
                entity.CancellationReason = (thisReader["CancellationReason"] == DBNull.Value) ? null : thisReader["CancellationReason"].ToString();
                entity.SuspensionReason = (thisReader["SuspensionReason"] == DBNull.Value) ? null : thisReader["SuspensionReason"].ToString();
                entity.ChangeReason = (thisReader["ChangeReason"] == DBNull.Value) ? null : thisReader["ChangeReason"].ToString();
                entity.InvalidReason = (thisReader["InvalidReason"] == DBNull.Value) ? null : thisReader["InvalidReason"].ToString();
                entity.ShortName = (thisReader["ShortName"] == DBNull.Value) ? null : thisReader["ShortName"].ToString();
                entity.LastName = (thisReader["LastName"] == DBNull.Value) ? null : thisReader["LastName"].ToString();
                entity.FirstName = (thisReader["FirstName"] == DBNull.Value) ? null : thisReader["FirstName"].ToString();
                entity.SecondName = (thisReader["SecondName"] == DBNull.Value) ? null : thisReader["SecondName"].ToString();
                entity.OgrnDate = thisReader["OgrnDate"] as System.Nullable<System.DateTime>;
                entity.Country = (thisReader["Country"] == DBNull.Value) ? null : thisReader["Country"].ToString();
                entity.PostalCode = (thisReader["PostalCode"] == DBNull.Value) ? null : thisReader["PostalCode"].ToString();
                entity.Locality = (thisReader["Locality"] == DBNull.Value) ? null : thisReader["Locality"].ToString();
                entity.Region = (thisReader["Region"] == DBNull.Value) ? null : thisReader["Region"].ToString();
                entity.City = (thisReader["City"] == DBNull.Value) ? null : thisReader["City"].ToString();
                entity.Town = (thisReader["Town"] == DBNull.Value) ? null : thisReader["Town"].ToString();
                entity.Street = (thisReader["Street"] == DBNull.Value) ? null : thisReader["Street"].ToString();
                entity.House = (thisReader["House"] == DBNull.Value) ? null : thisReader["House"].ToString();
                entity.Building = (thisReader["Building"] == DBNull.Value) ? null : thisReader["Building"].ToString();
                entity.Structure = (thisReader["Structure"] == DBNull.Value) ? null : thisReader["Structure"].ToString();
                entity.Facility = (thisReader["Facility"] == DBNull.Value) ? null : thisReader["Facility"].ToString();
                entity.Ownership = (thisReader["Ownership"] == DBNull.Value) ? null : thisReader["Ownership"].ToString();
                entity.Flat = (thisReader["Flat"] == DBNull.Value) ? null : thisReader["Flat"].ToString();
                entity.Fax = (thisReader["Fax"] == DBNull.Value) ? null : thisReader["Fax"].ToString();
                entity.EMail = (thisReader["EMail"] == DBNull.Value) ? null : thisReader["EMail"].ToString();
                entity.TaxiColor = (thisReader["TaxiColor"] == DBNull.Value) ? null : thisReader["TaxiColor"].ToString();
                entity.TaxiNumberColor = (thisReader["TaxiNumberColor"] == DBNull.Value) ? null : thisReader["TaxiNumberColor"].ToString();
                entity.TaxiVin = (thisReader["TaxiVin"] == DBNull.Value) ? null : thisReader["TaxiVin"].ToString();
                entity.ChangeDate = thisReader["ChangeDate"] as System.Nullable<System.DateTime>;
                entity.Guid_OD = (thisReader["Guid_OD"] == DBNull.Value) ? null : thisReader["Guid_OD"].ToString();
                entity.Date_OD = thisReader["Date_OD"] as System.Nullable<System.DateTime>;
                entity.FromPortal = thisReader["FromPortal"] as System.Nullable<System.Boolean>;
                entity.FirmName = (thisReader["FirmName"] == DBNull.Value) ? null : thisReader["FirmName"].ToString();
                entity.Brand = (thisReader["Brand"] == DBNull.Value) ? null : thisReader["Brand"].ToString();
                entity.OgrnNum = (thisReader["OgrnNum"] == DBNull.Value) ? null : thisReader["OgrnNum"].ToString();
                entity.OgrnName = (thisReader["OgrnName"] == DBNull.Value) ? null : thisReader["OgrnName"].ToString();
                entity.GRAddress = (thisReader["GRAddress"] == DBNull.Value) ? null : thisReader["GRAddress"].ToString();
                entity.InnDate = thisReader["InnDate"] as System.Nullable<System.DateTime>;
                entity.InnName = (thisReader["InnName"] == DBNull.Value) ? null : thisReader["InnName"].ToString();
                entity.InnNum = (thisReader["InnNum"] == DBNull.Value) ? null : thisReader["InnNum"].ToString();
                entity.Address_Fact = (thisReader["Address_Fact"] == DBNull.Value) ? null : thisReader["Address_Fact"].ToString();
                entity.Country_Fact = (thisReader["Country_Fact"] == DBNull.Value) ? null : thisReader["Country_Fact"].ToString();
                entity.PostalCode_Fact = (thisReader["PostalCode_Fact"] == DBNull.Value) ? null : thisReader["PostalCode_Fact"].ToString();
                entity.Locality_Fact = (thisReader["Locality_Fact"] == DBNull.Value) ? null : thisReader["Locality_Fact"].ToString();
                entity.Region_Fact = (thisReader["Region_Fact"] == DBNull.Value) ? null : thisReader["Region_Fact"].ToString();
                entity.City_Fact = (thisReader["City_Fact"] == DBNull.Value) ? null : thisReader["City_Fact"].ToString();
                entity.Town_Fact = (thisReader["Town_Fact"] == DBNull.Value) ? null : thisReader["Town_Fact"].ToString();
                entity.Street_Fact = (thisReader["Street_Fact"] == DBNull.Value) ? null : thisReader["Street_Fact"].ToString();
                entity.House_Fact = (thisReader["House_Fact"] == DBNull.Value) ? null : thisReader["House_Fact"].ToString();
                entity.Building_Fact = (thisReader["Building_Fact"] == DBNull.Value) ? null : thisReader["Building_Fact"].ToString();
                entity.Structure_Fact = (thisReader["Structure_Fact"] == DBNull.Value) ? null : thisReader["Structure_Fact"].ToString();
                entity.Facility_Fact = (thisReader["Facility_Fact"] == DBNull.Value) ? null : thisReader["Facility_Fact"].ToString();
                entity.Ownership_Fact = (thisReader["Ownership_Fact"] == DBNull.Value) ? null : thisReader["Ownership_Fact"].ToString();
                entity.Flat_Fact = (thisReader["Flat_Fact"] == DBNull.Value) ? null : thisReader["Flat_Fact"].ToString();
                entity.Gps = thisReader["Gps"] as System.Nullable<System.Boolean>;
                entity.Taxometr = thisReader["Taxometr"] as System.Nullable<System.Boolean>;
                entity.TODate = thisReader["TODate"] as System.Nullable<System.DateTime>;
                entity.STSNumber = (thisReader["STSNumber"] == DBNull.Value) ? null : thisReader["STSNumber"].ToString();
                entity.STSDate = thisReader["STSDate"] as System.Nullable<System.DateTime>;
                entity.OwnType = thisReader["OwnType"] as System.Nullable<System.Int32>;
                entity.OwnNumber = (thisReader["OwnNumber"] == DBNull.Value) ? null : thisReader["OwnNumber"].ToString();
                entity.OwnDate = thisReader["OwnDate"] as System.Nullable<System.DateTime>;
                entity.MO = thisReader["MO"] as System.Nullable<System.Boolean>;
                entity.GUID_MO = (thisReader["GUID_MO"] == DBNull.Value) ? null : thisReader["GUID_MO"].ToString();
                entity.DATE_MO = thisReader["DATE_MO"] as System.Nullable<System.DateTime>;
            }
            else
            {
                throw new Exception("Data not found");
            }
            thisReader.Close();
            return entity;
        }

        public License GetAnyLicenseForSPTaxiId(int spTaxiId)
        {
            License entity = new License();
            SqlConnection thisConn = getSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand
            {
                CommandText =
                    "SELECT TOP 1 [Id] , [Title] , [RegNumber] , [BlankSeries] , [BlankNo] , [OrgName] , [Ogrn] , [Inn] , [Parent] , [RootParent] , [Status] , [Document] , [Signature] , [TaxiId] , [Lfb] , [JuridicalAddress] , [PhoneNumber] , [AddContactData] , [AccountAbbr] , [TaxiBrand] , [TaxiModel] , [TaxiStateNumber] , [TaxiYear] , [OutputDate] , [CreationDate] , [TillDate] , [TillSuspensionDate] , [CancellationReason] , [SuspensionReason] , [ChangeReason] , [InvalidReason] , [ShortName] , [LastName] , [FirstName] , [SecondName] , [OgrnDate] , [Country] , [PostalCode] , [Locality] , [Region] , [City] , [Town] , [Street] , [House] , [Building] , [Structure] , [Facility] , [Ownership] , [Flat] , [Fax] , [EMail] , [TaxiColor] , [TaxiNumberColor] , [TaxiVin] , [ChangeDate] , [Guid_OD] , [Date_OD] , [FromPortal] , [FirmName] , [Brand] , [OgrnNum] , [OgrnName] , [GRAddress] , [InnDate] , [InnName] , [InnNum] , [Address_Fact] , [Country_Fact] , [PostalCode_Fact] , [Locality_Fact] , [Region_Fact] , [City_Fact] , [Town_Fact] , [Street_Fact] , [House_Fact] , [Building_Fact] , [Structure_Fact] , [Facility_Fact] , [Ownership_Fact] , [Flat_Fact] , [Gps] , [Taxometr] , [TODate] , [STSNumber] , [STSDate] , [OwnType] , [OwnNumber] , [OwnDate] , [MO] , [GUID_MO] , [DATE_MO] FROM [dbo].[License] WHERE [TaxiId] = @TaxiId ORDER BY [Id] DESC"
            };
            selectCommand.Parameters.AddWithValue("@TaxiId", spTaxiId);

            selectCommand.Connection = thisConn;
            SqlDataReader thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                entity.Id = (System.Int32)thisReader["Id"];
                entity.Title = (thisReader["Title"] == DBNull.Value) ? null : thisReader["Title"].ToString();
                entity.RegNumber = (thisReader["RegNumber"] == DBNull.Value) ? null : thisReader["RegNumber"].ToString();
                entity.BlankSeries = (thisReader["BlankSeries"] == DBNull.Value) ? null : thisReader["BlankSeries"].ToString();
                entity.BlankNo = (thisReader["BlankNo"] == DBNull.Value) ? null : thisReader["BlankNo"].ToString();
                entity.OrgName = (thisReader["OrgName"] == DBNull.Value) ? null : thisReader["OrgName"].ToString();
                entity.Ogrn = (thisReader["Ogrn"] == DBNull.Value) ? null : thisReader["Ogrn"].ToString();
                entity.Inn = (thisReader["Inn"] == DBNull.Value) ? null : thisReader["Inn"].ToString();
                entity.Parent = thisReader["Parent"] as System.Nullable<System.Int32>;
                entity.RootParent = thisReader["RootParent"] as System.Nullable<System.Int32>;
                entity.Status = thisReader["Status"] as System.Nullable<System.Int32>;
                entity.Document = (thisReader["Document"] == DBNull.Value) ? null : thisReader["Document"].ToString();
                entity.Signature = (thisReader["Signature"] == DBNull.Value) ? null : thisReader["Signature"].ToString();
                entity.TaxiId = thisReader["TaxiId"] as System.Nullable<System.Int32>;
                entity.Lfb = (thisReader["Lfb"] == DBNull.Value) ? null : thisReader["Lfb"].ToString();
                entity.JuridicalAddress = (thisReader["JuridicalAddress"] == DBNull.Value) ? null : thisReader["JuridicalAddress"].ToString();
                entity.PhoneNumber = (thisReader["PhoneNumber"] == DBNull.Value) ? null : thisReader["PhoneNumber"].ToString();
                entity.AddContactData = (thisReader["AddContactData"] == DBNull.Value) ? null : thisReader["AddContactData"].ToString();
                entity.AccountAbbr = (thisReader["AccountAbbr"] == DBNull.Value) ? null : thisReader["AccountAbbr"].ToString();
                entity.TaxiBrand = (thisReader["TaxiBrand"] == DBNull.Value) ? null : thisReader["TaxiBrand"].ToString();
                entity.TaxiModel = (thisReader["TaxiModel"] == DBNull.Value) ? null : thisReader["TaxiModel"].ToString();
                entity.TaxiStateNumber = (thisReader["TaxiStateNumber"] == DBNull.Value) ? null : thisReader["TaxiStateNumber"].ToString();
                entity.TaxiYear = thisReader["TaxiYear"] as System.Nullable<System.Int32>;
                entity.OutputDate = thisReader["OutputDate"] as System.Nullable<System.DateTime>;
                entity.CreationDate = thisReader["CreationDate"] as System.Nullable<System.DateTime>;
                entity.TillDate = thisReader["TillDate"] as System.Nullable<System.DateTime>;
                entity.TillSuspensionDate = thisReader["TillSuspensionDate"] as System.Nullable<System.DateTime>;
                entity.CancellationReason = (thisReader["CancellationReason"] == DBNull.Value) ? null : thisReader["CancellationReason"].ToString();
                entity.SuspensionReason = (thisReader["SuspensionReason"] == DBNull.Value) ? null : thisReader["SuspensionReason"].ToString();
                entity.ChangeReason = (thisReader["ChangeReason"] == DBNull.Value) ? null : thisReader["ChangeReason"].ToString();
                entity.InvalidReason = (thisReader["InvalidReason"] == DBNull.Value) ? null : thisReader["InvalidReason"].ToString();
                entity.ShortName = (thisReader["ShortName"] == DBNull.Value) ? null : thisReader["ShortName"].ToString();
                entity.LastName = (thisReader["LastName"] == DBNull.Value) ? null : thisReader["LastName"].ToString();
                entity.FirstName = (thisReader["FirstName"] == DBNull.Value) ? null : thisReader["FirstName"].ToString();
                entity.SecondName = (thisReader["SecondName"] == DBNull.Value) ? null : thisReader["SecondName"].ToString();
                entity.OgrnDate = thisReader["OgrnDate"] as System.Nullable<System.DateTime>;
                entity.Country = (thisReader["Country"] == DBNull.Value) ? null : thisReader["Country"].ToString();
                entity.PostalCode = (thisReader["PostalCode"] == DBNull.Value) ? null : thisReader["PostalCode"].ToString();
                entity.Locality = (thisReader["Locality"] == DBNull.Value) ? null : thisReader["Locality"].ToString();
                entity.Region = (thisReader["Region"] == DBNull.Value) ? null : thisReader["Region"].ToString();
                entity.City = (thisReader["City"] == DBNull.Value) ? null : thisReader["City"].ToString();
                entity.Town = (thisReader["Town"] == DBNull.Value) ? null : thisReader["Town"].ToString();
                entity.Street = (thisReader["Street"] == DBNull.Value) ? null : thisReader["Street"].ToString();
                entity.House = (thisReader["House"] == DBNull.Value) ? null : thisReader["House"].ToString();
                entity.Building = (thisReader["Building"] == DBNull.Value) ? null : thisReader["Building"].ToString();
                entity.Structure = (thisReader["Structure"] == DBNull.Value) ? null : thisReader["Structure"].ToString();
                entity.Facility = (thisReader["Facility"] == DBNull.Value) ? null : thisReader["Facility"].ToString();
                entity.Ownership = (thisReader["Ownership"] == DBNull.Value) ? null : thisReader["Ownership"].ToString();
                entity.Flat = (thisReader["Flat"] == DBNull.Value) ? null : thisReader["Flat"].ToString();
                entity.Fax = (thisReader["Fax"] == DBNull.Value) ? null : thisReader["Fax"].ToString();
                entity.EMail = (thisReader["EMail"] == DBNull.Value) ? null : thisReader["EMail"].ToString();
                entity.TaxiColor = (thisReader["TaxiColor"] == DBNull.Value) ? null : thisReader["TaxiColor"].ToString();
                entity.TaxiNumberColor = (thisReader["TaxiNumberColor"] == DBNull.Value) ? null : thisReader["TaxiNumberColor"].ToString();
                entity.TaxiVin = (thisReader["TaxiVin"] == DBNull.Value) ? null : thisReader["TaxiVin"].ToString();
                entity.ChangeDate = thisReader["ChangeDate"] as System.Nullable<System.DateTime>;
                entity.Guid_OD = (thisReader["Guid_OD"] == DBNull.Value) ? null : thisReader["Guid_OD"].ToString();
                entity.Date_OD = thisReader["Date_OD"] as System.Nullable<System.DateTime>;
                entity.FromPortal = thisReader["FromPortal"] as System.Nullable<System.Boolean>;
                entity.FirmName = (thisReader["FirmName"] == DBNull.Value) ? null : thisReader["FirmName"].ToString();
                entity.Brand = (thisReader["Brand"] == DBNull.Value) ? null : thisReader["Brand"].ToString();
                entity.OgrnNum = (thisReader["OgrnNum"] == DBNull.Value) ? null : thisReader["OgrnNum"].ToString();
                entity.OgrnName = (thisReader["OgrnName"] == DBNull.Value) ? null : thisReader["OgrnName"].ToString();
                entity.GRAddress = (thisReader["GRAddress"] == DBNull.Value) ? null : thisReader["GRAddress"].ToString();
                entity.InnDate = thisReader["InnDate"] as System.Nullable<System.DateTime>;
                entity.InnName = (thisReader["InnName"] == DBNull.Value) ? null : thisReader["InnName"].ToString();
                entity.InnNum = (thisReader["InnNum"] == DBNull.Value) ? null : thisReader["InnNum"].ToString();
                entity.Address_Fact = (thisReader["Address_Fact"] == DBNull.Value) ? null : thisReader["Address_Fact"].ToString();
                entity.Country_Fact = (thisReader["Country_Fact"] == DBNull.Value) ? null : thisReader["Country_Fact"].ToString();
                entity.PostalCode_Fact = (thisReader["PostalCode_Fact"] == DBNull.Value) ? null : thisReader["PostalCode_Fact"].ToString();
                entity.Locality_Fact = (thisReader["Locality_Fact"] == DBNull.Value) ? null : thisReader["Locality_Fact"].ToString();
                entity.Region_Fact = (thisReader["Region_Fact"] == DBNull.Value) ? null : thisReader["Region_Fact"].ToString();
                entity.City_Fact = (thisReader["City_Fact"] == DBNull.Value) ? null : thisReader["City_Fact"].ToString();
                entity.Town_Fact = (thisReader["Town_Fact"] == DBNull.Value) ? null : thisReader["Town_Fact"].ToString();
                entity.Street_Fact = (thisReader["Street_Fact"] == DBNull.Value) ? null : thisReader["Street_Fact"].ToString();
                entity.House_Fact = (thisReader["House_Fact"] == DBNull.Value) ? null : thisReader["House_Fact"].ToString();
                entity.Building_Fact = (thisReader["Building_Fact"] == DBNull.Value) ? null : thisReader["Building_Fact"].ToString();
                entity.Structure_Fact = (thisReader["Structure_Fact"] == DBNull.Value) ? null : thisReader["Structure_Fact"].ToString();
                entity.Facility_Fact = (thisReader["Facility_Fact"] == DBNull.Value) ? null : thisReader["Facility_Fact"].ToString();
                entity.Ownership_Fact = (thisReader["Ownership_Fact"] == DBNull.Value) ? null : thisReader["Ownership_Fact"].ToString();
                entity.Flat_Fact = (thisReader["Flat_Fact"] == DBNull.Value) ? null : thisReader["Flat_Fact"].ToString();
                entity.Gps = thisReader["Gps"] as System.Nullable<System.Boolean>;
                entity.Taxometr = thisReader["Taxometr"] as System.Nullable<System.Boolean>;
                entity.TODate = thisReader["TODate"] as System.Nullable<System.DateTime>;
                entity.STSNumber = (thisReader["STSNumber"] == DBNull.Value) ? null : thisReader["STSNumber"].ToString();
                entity.STSDate = thisReader["STSDate"] as System.Nullable<System.DateTime>;
                entity.OwnType = thisReader["OwnType"] as System.Nullable<System.Int32>;
                entity.OwnNumber = (thisReader["OwnNumber"] == DBNull.Value) ? null : thisReader["OwnNumber"].ToString();
                entity.OwnDate = thisReader["OwnDate"] as System.Nullable<System.DateTime>;
                entity.MO = thisReader["MO"] as System.Nullable<System.Boolean>;
                entity.GUID_MO = (thisReader["GUID_MO"] == DBNull.Value) ? null : thisReader["GUID_MO"].ToString();
                entity.DATE_MO = thisReader["DATE_MO"] as System.Nullable<System.DateTime>;
            }
            else
            {
                throw new Exception("Data not found");
            }
            thisReader.Close();
            return entity;
        }
    }
}
