﻿CREATE TABLE [dbo].[RequestAccount] (
	[Title] AS [Name],
	[FullName] NVARCHAR(255) NULL,
	[Name] NVARCHAR(255) NULL,
	[BrandName] NVARCHAR(255) NULL,
	[Brand] NVARCHAR(255) NULL,
	[Ogrn] NVARCHAR(255) NULL,
	[OgrnAuthority] NVARCHAR(255) NULL,
	[OgrnAuthorityAddress] NVARCHAR(255) NULL,
	[OgrnNum] NVARCHAR(255) NULL,
	[OgrnDate] DATETIME NULL,
	[Inn] NVARCHAR(255) NULL,
	[InnAuthority] NVARCHAR(255) NULL,
	[InnNum] NVARCHAR(255) NULL,
	[InnDate] DATETIME NULL,
	[Kpp] NVARCHAR(255) NULL,
	[Okpo] NVARCHAR(255) NULL,
	[OrgFormCode] NVARCHAR(255) NULL,
	[Okved] NVARCHAR(255) NULL,
	[Okfs] NVARCHAR(255) NULL,
	[BankName] NVARCHAR(255) NULL,
	[BankBik] NVARCHAR(255) NULL,
	[CorrAccount] NVARCHAR(255) NULL,
	[SetAccount] NVARCHAR(255) NULL,
	[Phone] NVARCHAR(255) NULL,
	[Fax] NVARCHAR(255) NULL,
	[EMail] NVARCHAR(255) NULL,
	[WebSite] NVARCHAR(255) NULL,
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[MessageId] VARCHAR(36) NOT NULL,
	[PostalAddress] INT NULL,
	[FactAddress] INT NULL,
	[RequestContact] INT NULL,
	[SingleStrPostalAddress] AS ([dbo].[fn_BuildAddressSingleString](PostalAddress)),
	[SingleStrFactAddress] AS ([dbo].[fn_BuildAddressSingleString](FactAddress)),
	[Author] NVARCHAR(255) NULL
	CONSTRAINT [FK_RequestAccount_PostalAddress] FOREIGN KEY ([PostalAddress]) REFERENCES [dbo].[Address]([Id]),
	CONSTRAINT [FK_RequestAccount_FactAddress] FOREIGN KEY ([FactAddress]) REFERENCES [dbo].[Address]([Id]),
	CONSTRAINT [FK_RequestAccount_RequestContact] FOREIGN KEY ([RequestContact]) REFERENCES [dbo].[RequestContact]([Id_Auto])
	);