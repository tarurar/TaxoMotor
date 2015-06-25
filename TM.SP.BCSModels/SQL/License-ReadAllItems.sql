﻿SELECT [L1].[Id]
	,[L1].[Title]
	,[L1].[RegNumber]
	,[L1].[BlankSeries]
	,[L1].[BlankNo]
	,[L1].[OrgName]
	,[L1].[Ogrn]
	,[L1].[Inn]
	,[L1].[Parent]
	,[L1].[RootParent]
	,[L1].[Status]
	,[L1].[Document]
	,[L1].[SignatureBinary]
	,[L1].[TaxiId]
	,[L1].[Lfb]
	,[L1].[JuridicalAddress]
	,[L1].[PhoneNumber]
	,[L1].[AddContactData]
	,[L1].[AccountAbbr]
	,[L1].[TaxiBrand]
	,[L1].[TaxiModel]
	,[L1].[TaxiStateNumber]
	,[L1].[TaxiYear]
	,[L1].[OutputDate]
	,[L1].[CreationDate]
	,[L1].[TillDate]
	,[L1].[TillSuspensionDate]
	,[L1].[CancellationReason]
	,[L1].[SuspensionReason]
	,[L1].[ChangeReason]
	,[L1].[InvalidReason]
	,[L1].[ShortName]
	,[L1].[LastName]
	,[L1].[FirstName]
	,[L1].[SecondName]
	,[L1].[OgrnDate]
	,[L1].[Country]
	,[L1].[PostalCode]
	,[L1].[Locality]
	,[L1].[Region]
	,[L1].[City]
	,[L1].[Town]
	,[L1].[Street]
	,[L1].[House]
	,[L1].[Building]
	,[L1].[Structure]
	,[L1].[Facility]
	,[L1].[Ownership]
	,[L1].[Flat]
	,[L1].[Fax]
	,[L1].[EMail]
	,[L1].[TaxiColor]
	,[L1].[TaxiNumberColor]
	,[L1].[TaxiVin]
	,[L1].[ChangeDate]
	,[L1].[Guid_OD]
	,[L1].[Date_OD]
	,[L1].[FromPortal]
	,[L1].[FirmName]
	,[L1].[Brand]
	,[L1].[OgrnNum]
	,[L1].[OgrnName]
	,[L1].[GRAddress]
	,[L1].[InnDate]
	,[L1].[InnName]
	,[L1].[InnNum]
	,[L1].[Address_Fact]
	,[L1].[Country_Fact]
	,[L1].[PostalCode_Fact]
	,[L1].[Locality_Fact]
	,[L1].[Region_Fact]
	,[L1].[City_Fact]
	,[L1].[Town_Fact]
	,[L1].[Street_Fact]
	,[L1].[House_Fact]
	,[L1].[Building_Fact]
	,[L1].[Structure_Fact]
	,[L1].[Facility_Fact]
	,[L1].[Ownership_Fact]
	,[L1].[Flat_Fact]
	,[L1].[Gps]
	,[L1].[Taxometr]
	,[L1].[TODate]
	,[L1].[STSNumber]
	,[L1].[STSDate]
	,[L1].[OwnType]
	,[L1].[OwnNumber]
	,[L1].[OwnDate]
	,[L1].[MO]
	,[L1].[GUID_MO]
	,[L1].[DATE_MO]
	,[L1].[Obsolete]
	,[L1].[DisableGibddSend]
	,CAST(CASE 
		WHEN EXISTS (
				SELECT [L2].[Id]
				FROM [dbo].[License] L2
				WHERE [L2].[Parent] = [L1].[Id]
				)
			THEN 1
		ELSE 0
		END AS BIT) AS [HasAnyChilds]
FROM [dbo].[License] L1