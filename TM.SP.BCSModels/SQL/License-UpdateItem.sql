﻿UPDATE [dbo].[License]
SET [RegNumber] = @RegNumber
	,[BlankSeries] = @BlankSeries
	,[BlankNo] = @BlankNo
	,[OrgName] = @OrgName
	,[Ogrn] = @Ogrn
	,[Inn] = @Inn
	,[Parent] = @Parent
	,[RootParent] = @RootParent
	,[Status] = @Status
	,[Document] = @Document
	,[Signature] = @Signature
	,[TaxiId] = @TaxiId
	,[Lfb] = @Lfb
	,[JuridicalAddress] = @JuridicalAddress
	,[PhoneNumber] = @PhoneNumber
	,[AddContactData] = @AddContactData
	,[AccountAbbr] = @AccountAbbr
	,[TaxiBrand] = @TaxiBrand
	,[TaxiModel] = @TaxiModel
	,[TaxiStateNumber] = @TaxiStateNumber
	,[TaxiYear] = @TaxiYear
	,[OutputDate] = @OutputDate
	,[CreationDate] = @CreationDate
	,[TillDate] = @TillDate
	,[TillSuspensionDate] = @TillSuspensionDate
	,[CancellationReason] = @CancellationReason
	,[SuspensionReason] = @SuspensionReason
	,[ChangeReason] = @ChangeReason
	,[InvalidReason] = @InvalidReason
	,[ShortName] = @ShortName
	,[LastName] = @LastName
	,[FirstName] = @FirstName
	,[SecondName] = @SecondName
	,[OgrnDate] = @OgrnDate
	,[Country] = @Country
	,[PostalCode] = @PostalCode
	,[Locality] = @Locality
	,[Region] = @Region
	,[City] = @City
	,[Town] = @Town
	,[Street] = @Street
	,[House] = @House
	,[Building] = @Building
	,[Structure] = @Structure
	,[Facility] = @Facility
	,[Ownership] = @Ownership
	,[Flat] = @Flat
	,[Fax] = @Fax
	,[EMail] = @EMail
	,[TaxiColor] = @TaxiColor
	,[TaxiNumberColor] = @TaxiNumberColor
	,[TaxiVin] = @TaxiVin
	,[ChangeDate] = @ChangeDate
	,[Guid_OD] = @Guid_OD
	,[Date_OD] = @Date_OD
	,[FromPortal] = @FromPortal
	,[FirmName] = @FirmName
	,[Brand] = @Brand
	,[OgrnNum] = @OgrnNum
	,[OgrnName] = @OgrnName
	,[GRAddress] = @GRAddress
	,[InnDate] = @InnDate
	,[InnName] = @InnName
	,[InnNum] = @InnNum
	,[Address_Fact] = @Address_Fact
	,[Country_Fact] = @Country_Fact
	,[PostalCode_Fact] = @PostalCode_Fact
	,[Locality_Fact] = @Locality_Fact
	,[Region_Fact] = @Region_Fact
	,[City_Fact] = @City_Fact
	,[Town_Fact] = @Town_Fact
	,[Street_Fact] = @Street_Fact
	,[House_Fact] = @House_Fact
	,[Building_Fact] = @Building_Fact
	,[Structure_Fact] = @Structure_Fact
	,[Facility_Fact] = @Facility_Fact
	,[Ownership_Fact] = @Ownership_Fact
	,[Flat_Fact] = @Flat_Fact
	,[Gps] = @Gps
	,[Taxometr] = @Taxometr
	,[TODate] = @TODate
	,[STSNumber] = @STSNumber
	,[STSDate] = @STSDate
	,[OwnType] = @OwnType
	,[OwnNumber] = @OwnNumber
	,[OwnDate] = @OwnDate
	,[MO] = @MO
	,[GUID_MO] = @GUID_MO
	,[DATE_MO] = @DATE_MO
	,[Obsolete] = @Obsolete
	,[DisableGibddSend] = @DisableGibddSend
WHERE [Id] = @Id